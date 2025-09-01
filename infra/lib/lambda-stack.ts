import * as cdk from 'aws-cdk-lib';
import * as path from 'path';
import { Duration, Stack, StackProps, CfnOutput } from 'aws-cdk-lib';
import { Construct } from 'constructs';
import * as lambda from 'aws-cdk-lib/aws-lambda';
import * as iam from 'aws-cdk-lib/aws-iam';
import * as apigw from 'aws-cdk-lib/aws-apigateway';


export class LambdaStack extends cdk.Stack {
  constructor(scope: Construct, id: string, props?: cdk.StackProps) {
    super(scope, id, props);

    const envName = (this.node.tryGetContext('env') ?? 'dev').toString();
    const ENV = envName.toUpperCase();
    const account = cdk.Stack.of(this).account;
    const region  = cdk.Stack.of(this).region;

    const serviceName = 'PhoneConsultationService';
    const configPath  = '/tw/${ENV}/${serviceName}/';

     const publishDir = path.join(
      __dirname,
      '../../',
      serviceName,
      'bin',
      'Release',
      'net8.0',
      'publish'
    );

      const fn = new lambda.Function(this, 'PhoneConsultationServiceFn', {
      runtime: lambda.Runtime.DOTNET_8,
      handler: 'PhoneConsultationService::PhoneConsultationService.LambdaEntryPoint::FunctionHandlerAsync',
      code: lambda.Code.fromAsset(publishDir),
      architecture: lambda.Architecture.ARM_64,
      memorySize: 1024,
      timeout: Duration.seconds(30),
      environment: {
        ASPNETCORE_ENVIRONMENT: ENV,
        CONFIG_PREFIX: configPath,
        SERVICE_NAME: serviceName,
      },
    });

    fn.addToRolePolicy(new iam.PolicyStatement({
      actions: ['ssm:GetParameter','ssm:GetParameters','ssm:GetParametersByPath'],
      resources: ['arn:aws:ssm:${region}:${account}:parameter${configPath}*'],
    }));

     // === API Gateway REST (proxy completo a la Lambda) ===
    const api = new apigw.RestApi(this, 'PhoneConsultationApi', {
      restApiName: 'tw-${envName}-phone-api',
      description: 'API REST para PhoneConsultationService (DEV)',
      deployOptions: {
        stageName: envName, // "dev"
        metricsEnabled: true,
        loggingLevel: apigw.MethodLoggingLevel.INFO,
        dataTraceEnabled: false
      },
      defaultCorsPreflightOptions: {
        allowOrigins: apigw.Cors.ALL_ORIGINS,
        allowMethods: apigw.Cors.ALL_METHODS,
        allowHeaders: apigw.Cors.DEFAULT_HEADERS,
      }
    });

    const lambdaIntegration = new apigw.LambdaIntegration(fn, {
      proxy: true,              // ⇐ proxy total hacia ASP.NET Core
      allowTestInvoke: true,
      timeout: Duration.seconds(29),
    });

    const rootProxy = api.root.addResource('{proxy+}');
    rootProxy.addMethod('ANY', lambdaIntegration);
    api.root.addMethod('ANY', lambdaIntegration);

    new CfnOutput(this, 'ApiUrl', { value: api.urlForPath('/') });
  }
}
