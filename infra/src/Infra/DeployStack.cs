using Amazon.CDK;
using Amazon.CDK.AWS.APIGatewayv2;
using Amazon.CDK.AWS.APIGatewayv2.Integrations;
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
        }
    }
}
