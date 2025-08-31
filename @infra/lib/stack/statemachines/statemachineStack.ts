import * as lambda from "aws-cdk-lib/aws-lambda";
import * as cdk from "aws-cdk-lib";
import * as sfn from 'aws-cdk-lib/aws-stepfunctions';
import * as tasks from 'aws-cdk-lib/aws-stepfunctions-tasks';
import * as logs from 'aws-cdk-lib/aws-logs';

import { Construct } from "constructs";
import path = require("path");

export interface StatemachinetackProps extends cdk.StackProps {
  region: string;
  client: string;
  project: string;
  partner: string;
  stage: string;
  account_num: String;
  trackingMokServicesLambda: lambda.IFunction;
  SnsNotificationLambda: lambda.IFunction;
}
export class StatemachineStack extends cdk.Stack {


  constructor(scope: Construct, id: string, props: StatemachinetackProps) {
    super(scope, id);

    // Create a sample Lambda function
    const demoLambda = new lambda.Function(this, 'DemoLambda', {
      runtime: lambda.Runtime.NODEJS_20_X,
      code: lambda.Code.fromInline('exports.handler = async (event) => { return "Hello from Lambda"; };'),
      handler: 'index.handler',
    });

    // First Express Step Function (Simple Pass)

    const state1_snsNotification = new tasks.LambdaInvoke(this, 'SnsNotification', {
      lambdaFunction: props.SnsNotificationLambda,
      outputPath: '$.Payload'  // usar la salida de la Lambda
    });

    const state1_trackingMok = new tasks.LambdaInvoke(this, 'TrackingMok', {
      lambdaFunction: props.trackingMokServicesLambda,
      outputPath: '$.Payload'  // usar la salida de la Lambda
    })
    // .addCatch(state1_snsNotification, {
    //     errors: [sfn.Errors.ALL], // Catch all errors
    //     resultPath: '$.errorInfo', // Store error details
    //   });;



    const state1_pass = new sfn.Pass(this, 'FirstState', {
      result: sfn.Result.fromString('Completed first step'),
    });

    const state1_succeed = new sfn.Succeed(this, 'state1_succeed', {
      outputPath: '$.Payload'
    });

    const state1_checkMok = new sfn.Choice(this, 'isMOK?', {
      // outputPath: '$.Payload'
    })
      .when(sfn.Condition.stringEquals('$.logData.sourceCode', "MOK"), state1_trackingMok)
      // .otherwise(state1_pass);
      // .otherwise(state1_snsNotification);
      // state1_pass.next(state1_checkMok)
      // .addCatch(state1_snsNotification, {
      //     errors: [sfn.Errors.ALL], // Catch all errors
      //     resultPath: '$.errorInfo', // Store error details
      //   });;
      .otherwise(
        new sfn.Pass(this, 'HandleMissingSourceCode')
      )
      .afterwards();
    state1_trackingMok.next(state1_snsNotification)

    const definition1 = state1_checkMok


    const logGroupState1 = new logs.LogGroup(this, 'LogGroupState1')
    const firstStateMachine = new sfn.StateMachine(this, 'trackingMokExpressStateMachine', {
      definitionBody: sfn.DefinitionBody.fromChainable(definition1),
      stateMachineName: "trackingMokExpressStateMachine",
      stateMachineType: sfn.StateMachineType.STANDARD,
      logs: {
        destination: logGroupState1,
        level: sfn.LogLevel.ALL,
      },

    });

    // // Second Express Step Function (with Lambda invocation)
    // const lambdaTask = new tasks.LambdaInvoke(this, 'InvokeLambdaTask', {
    //   lambdaFunction: demoLambda,
    //   outputPath: '$.Payload',
    // });

    // const successState = new sfn.Succeed(this, 'SuccessState');
    // const definition = lambdaTask.next(successState);

    // const secondStateMachine = new sfn.StateMachine(this, 'updateEventExpressStateMachine', {
    //   definitionBody: sfn.DefinitionBody.fromChainable(definition),
    //   stateMachineName: "updateEventExpressStateMachine",
    //   stateMachineType: sfn.StateMachineType.EXPRESS,
    // });


  }
}
