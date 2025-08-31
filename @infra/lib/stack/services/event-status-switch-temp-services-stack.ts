// lib/stacks/event-status-switch-temp-services-stack.ts
import * as cdk from "aws-cdk-lib";
import * as lambda from "aws-cdk-lib/aws-lambda";
import { Construct } from "constructs";
import * as path from "path";
import {
  LambdaCreator,
  CommonLambdaStackProps,
} from "../../construct/lambda/lambda-creator";

export class EventStatusSwitchTempServicesStack extends cdk.Stack {
  public readonly eventStatusSwitchTempServicesLambda: lambda.IFunction;

  constructor(scope: Construct, id: string, props: CommonLambdaStackProps) {
    super(scope, id);

    const lambdaCreator = new LambdaCreator();

    const eventStatusSwitchTempServicesLambda = lambdaCreator.createLambda(
      this,
      "EventStatusSwitchTempServicesFunction",
      props,
      "EventStatusSwitchTempServices",
      {
        vpcConfig: {
          vpc: props.vpc,
          securityGroups: [props.lambdaSg],
        },
        policyStatements: [
          new cdk.aws_iam.PolicyStatement({
            actions: [
              "kms:*",
              "sns:Publish",
              "dynamodb:*", // Original was very broad, consider narrowing
              "sqs:sendmessage",
            ],
            resources: ["*"],
          }),
        ],
      }
    );

    this.eventStatusSwitchTempServicesLambda =
      eventStatusSwitchTempServicesLambda;
  }
}