#!/usr/bin/env node
import * as cdk from "aws-cdk-lib";
import { LambdaStack } from "../lib/lambda-stack";

const app = new cdk.App();

// Context provided via: npx cdk deploy -c env=dev
const envName = app.node.tryGetContext("env") ?? "dev";

new LambdaStack(app, `tw-${envName}-phone`, {
  env: {
    account: "590183946089",
    region: "us-west-1",
  },
});
