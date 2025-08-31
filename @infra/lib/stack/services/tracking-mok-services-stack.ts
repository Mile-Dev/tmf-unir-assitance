// lib/stacks/tracking-mok-services-stack.ts
import * as cdk from "aws-cdk-lib";
import * as lambda from "aws-cdk-lib/aws-lambda";
import { Construct } from "constructs";
import * as path from "path";
import {
  LambdaCreator,
  CommonLambdaStackProps,
} from "../../construct/lambda/lambda-creator";

export class TrackingMokServicesStack extends cdk.Stack {
  public readonly trackingMokServicesLambda: lambda.IFunction;
  public readonly arnTrackingMokServicesLambda: string;

  constructor(scope: Construct, id: string, props: CommonLambdaStackProps) {
    super(scope, id);

    const lambdaCreator = new LambdaCreator();

    const sharedServicesPath = path.join(__dirname, "../../../../SharedServices/");

    const trackingMokServicesLambda = lambdaCreator.createLambda(
      this,
      "TrackingMokServicesFunction",
      props,
      "TrackingMokServices",
      {
        volumes: [
          { containerPath: "/SharedServices/", hostPath: sharedServicesPath },
        ],
      }
    );

    this.trackingMokServicesLambda = trackingMokServicesLambda;
    this.arnTrackingMokServicesLambda = trackingMokServicesLambda.functionArn;
  }
}