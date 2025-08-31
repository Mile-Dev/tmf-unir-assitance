import * as path from 'path';
import { Duration, Stack, StackProps, CfnOutput } from 'aws-cdk-lib';
import { Construct } from 'constructs';
import * as lambda from 'aws-cdk-lib/aws-lambda';
import * as iam from 'aws-cdk-lib/aws-iam';
import { DotNetFunction } from '@aws-cdk/aws-lambda-dotnet-alpha';

export class LambdaStack extends Stack {
  constructor(scope: Construct, id: string, props?: StackProps) {
    super(scope, id, props);

    const envName = this.node.tryGetContext('env') ?? 'dev';

    // Helper to create a .NET 8 Lambda from a csproj
    const createDotNetLambda = (logicalId: string, projectRelPath: string, serviceName: string) => {
      const fn = new DotNetFunction(this, logicalId, {
        projectPath: path.join(__dirname, `../../${projectRelPath}`),
        runtime: lambda.Runtime.DOTNET_8,
        architecture: lambda.Architecture.ARM_64,
        memorySize: 1024,
        timeout: Duration.seconds(30),
        environment: {
          ASPNETCORE_ENVIRONMENT: envName.toUpperCase(),      // e.g., DEV, QA, PROD
          CONFIG_PREFIX: `/tw/${envName}/${serviceName}/`      // SSM/Secrets prefix convention
        }
        // If your csproj contains aws-lambda-tools-defaults.json with "function-handler",
        // DotNetFunction will use it automatically. Otherwise set 'handler' here.
      });

      // Allow reading config from SSM Parameter Store and Secrets Manager
      fn.addToRolePolicy(new iam.PolicyStatement({
        actions: [
          "ssm:GetParameter",
          "ssm:GetParameters",
          "ssm:GetParametersByPath"
        ],
        resources: ["*"]
      }));

      fn.addToRolePolicy(new iam.PolicyStatement({
        actions: ["secretsmanager:GetSecretValue", "secretsmanager:DescribeSecret"],
        resources: ["*"]
      }));

      new CfnOutput(this, `${logicalId}Name`, { value: fn.functionName });
      return fn;
    };

    // Adjust relative paths to match your repo layout
    createDotNetLambda('EventServicesFn', 'EventServices/EventServices.csproj', 'EventServices');
    createDotNetLambda('PhoneConsultationServiceFn', 'PhoneConsultationService/PhoneConsultationService.csproj', 'PhoneConsultationService');
    createDotNetLambda('ProviderServiceFn', 'ProviderService/ProviderService.csproj', 'ProviderService');
  }
}
