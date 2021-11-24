﻿using Amazon.CDK;

namespace Infra
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new DeployStack(app, "PipelineStack", new StackProps());

            app.Synth();
        }
    }
}
