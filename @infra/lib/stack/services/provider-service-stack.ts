// lib/stacks/provider-service-stack.ts
import * as cdk from "aws-cdk-lib";
import * as lambda from "aws-cdk-lib/aws-lambda";
import { Construct } from "constructs";
import * as path from "path";
import {
  LambdaCreator,
  CommonLambdaStackProps,
} from "../../construct/lambda/lambda-creator";

export class ProviderServiceStack extends cdk.Stack {
  public readonly providerServiceLambda: lambda.IFunction;
  public readonly arnProviderServiceLambda: string;

  constructor(scope: Construct, id: string, props: CommonLambdaStackProps) {
    super(scope, id);

    const lambdaCreator = new LambdaCreator();

    const sharedServicesPath = path.join(
      __dirname,
      "../../../../SharedServices/"
    );

    const providerServiceLambda = lambdaCreator.createLambda(
      this,
      "ProviderServiceFunction",
      props,
      "ProviderService",
      {
        volumes: [
          { containerPath: "/SharedServices/", hostPath: sharedServicesPath },
        ],
        policyStatements: [
          new cdk.aws_iam.PolicyStatement({
            actions: [
              "dynamodb:DescribeTable",
              "dynamodb:GetItem",
              "dynamodb:Query",
            ],
            resources: [
              `arn:aws:dynamodb:${props.region}:${props.account_num}:table/ProviderData`,
              `arn:aws:dynamodb:${props.region}:${props.account_num}:table/ProviderData/index/idCountry-index`,
              `arn:aws:dynamodb:${props.region}:${props.account_num}:table/ProviderData/index/geohashPK-index`,
              `arn:aws:dynamodb:${props.region}:${props.account_num}:table/ProviderData/index/idCity-index`,
              `arn:aws:dynamodb:${props.region}:${props.account_num}:table/ProviderData/index/nameProvider-index`,
            ],
          }),
        ],
      }
    );

    this.providerServiceLambda = providerServiceLambda;
    this.arnProviderServiceLambda = providerServiceLambda.functionArn;
  }
}
