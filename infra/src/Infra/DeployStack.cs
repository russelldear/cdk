using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGatewayv2;
using Amazon.CDK.AWS.APIGatewayv2.Integrations;
using Amazon.CDK.AWS.CodeBuild;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;

namespace Infra
{
    public class DeployStack : Stack
    {
        public readonly CfnOutput ApiGatewayEndpoint;

        internal DeployStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var handler = new Function(this, "SinglePageAppHandler", new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset("../publish/SinglePageApp.zip"),
                Handler = "SinglePageApp::SinglePageApp.LambdaEntryPoint::FunctionHandlerAsync"
            });

            var singlePageAppIntegration = new LambdaProxyIntegration(new LambdaProxyIntegrationProps
            {
                Handler = handler
            });

            var httpApi = new HttpApi(this, "SinglePageApp", new HttpApiProps());

            httpApi.AddRoutes(new AddRoutesOptions
            {
                Path = "/{proxy+}",
                Methods = new [] {HttpMethod.ANY},
                Integration = singlePageAppIntegration
            });

            ApiGatewayEndpoint = new CfnOutput(this, "GatewayUrl", new CfnOutputProps
            {
                Value = httpApi.ApiEndpoint
            });

            var githubSource = new GitHubSourceProps
            {
                Owner = "russelldear",
                Repo = "cdk",
                Webhook = true,
                WebhookFilters = new[]
                {
                    FilterGroup.InEventOf(EventAction.PUSH).AndBranchIs("master"),
                    FilterGroup.InEventOf(EventAction.PULL_REQUEST_MERGED).AndBranchIs("master"),
                }
            };

            var codeBuildRole = new Role(this, "SinglePageAppCodeBuildRole", new RoleProps
            {
                AssumedBy = new AnyPrincipal(),
                ManagedPolicies = new []
                {
                    ManagedPolicy.FromAwsManagedPolicyName("AWSLambda_FullAccess")
                },
                
            });

            var codeBuild = new Project(this, "SinglePageAppCodeBuild", new ProjectProps
            {
                Source = Source.GitHub(githubSource),
                BuildSpec = BuildSpec.FromObject(new YamlNode
                {
                    {"version", 0.2},
                    {"phases", new YamlNode
                        {{"build", new YamlNode
                            {{"commands", new []
                                {
                                    "echo \"Deploying " + handler.FunctionName + "\"",
                                    "cd src",
                                    "dotnet tool install --global Amazon.Lambda.Tools --version 5.0.1",
                                    "dotnet lambda deploy-function " + handler.FunctionName + " --function-runtime dotnetcore3.1"
                                }
                            }}
                        }}
                    }
                }),
                Environment = new BuildEnvironment
                {
                    BuildImage = LinuxBuildImage.AMAZON_LINUX_2_3
                },
                Role = codeBuildRole
            });
        }
    }

    public class YamlNode : Dictionary<string, object>
    {}
}
