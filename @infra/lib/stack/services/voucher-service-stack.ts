// lib/stacks/voucher-service-stack.ts
import * as cdk from "aws-cdk-lib";
import * as lambda from "aws-cdk-lib/aws-lambda";
import { Construct } from "constructs";
import * as path from "path";
import {
  LambdaCreator,
  CommonLambdaStackProps,
} from "../../construct/lambda/lambda-creator";

export class VoucherServiceStack extends cdk.Stack {
  public readonly voucherServiceLambda: lambda.IFunction;

  constructor(scope: Construct, id: string, props: CommonLambdaStackProps) {
    super(scope, id);

    const lambdaCreator = new LambdaCreator();

    const voucherServiceLambda = lambdaCreator.createLambda(
      this,
      "VoucherServiceFunction",
      props,
      "VoucherService"
      // No specific volumes or policies mentioned in the original for this one
    );
    this.voucherServiceLambda = voucherServiceLambda;
  }
}