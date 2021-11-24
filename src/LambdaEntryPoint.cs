using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Amazon.Lambda.AspNetCoreServer;

namespace SinglePageApp
{
    // API Gateway REST API                         -> : APIGatewayProxyFunction
    // API Gateway HTTP API payload version 1.0     -> : APIGatewayProxyFunction
    // API Gateway HTTP API payload version 2.0     -> : APIGatewayHttpApiV2ProxyFunction
    public class LambdaEntryPoint : APIGatewayHttpApiV2ProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder.UseStartup<Startup>();
        }

        protected override void Init(IHostBuilder builder)
        {
        }
    }
}
