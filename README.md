## Testing AWS CDK - SPA Deployment using APIGateway => Lambda

This repo contains code to spin up new AWS lambda function, deploy a .NET Core single page app to the lambda, spin up a new APIGateway, and point it at the lambda function - i.e. another addition to the 17,000 ways to deploy a SPA for (basically) free.

### AWS Cloud Development Kit

CDK is programmatic AWS resource creation. You can use it in various languages - this example is C#. It's effectively a nice bit of sugar for creating and running CloudFormation templates.

Some advantages:
- Can use imperative logic (if/for) to create resources
- You can test your infra creation code the same way you test your other apps
- Reuse across projects
- Familiar IDE/intellisense etc.

Good stuff in here:
- https://cdkworkshop.com/40-dotnet.html
- https://docs.aws.amazon.com/cdk/api/latest/docs/aws-construct-library.html
- https://docs.aws.amazon.com/cdk/latest/guide/home.html

### Prerequisites

- Ensure you've got Node 10.13.0 or later
- Ensure you've got credentials and region for the relevant AWS account using `aws configure`. You'll need a bunch of policies, depending on what you're spinning up.
- Install the CDK Toolkit - `npm install -g aws-cdk`

### First run
- In a terminal, navigate to the `infra` folder
- Bootstrap the resources you'll need to run CDK - `cdk bootstrap aws://ACCOUNT-NUMBER/REGION`
  - Save yourself clicks in the console by getting this info ^^^ using `aws sts get-caller-identity`

### Run
- Just run the `./deploy.bat` script to package the lambda and deploy all the things.

### Useful commands
Run these in the `infra` directory:

* `cdk deploy`       deploy this stack to your default AWS account/region
* `cdk diff`         compare deployed stack with current state
* `cdk synth`        emits the synthesized CloudFormation template

### Resources

#### APIGateway
- It's the fancy new HTTP v2 gateway type. 

#### SPA
- It's just the standard `dotnet new react` template app with a LambdaEntryPoint added. 

### Next steps
- Add custom domain via CDK
- Combine infra + lambda solutions