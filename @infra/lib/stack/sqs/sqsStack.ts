import { Stack } from "aws-cdk-lib";
import { Construct } from "constructs";
import * as cdk from "aws-cdk-lib";
import * as sqs from "aws-cdk-lib/aws-sqs";
import * as lambda from "aws-cdk-lib/aws-lambda";
import { SqsEventSource } from 'aws-cdk-lib/aws-lambda-event-sources';


export interface SqsStackProps extends cdk.StackProps {
    region: string;
    client: string;
    project: string;
    partner: string;
    stage: string;
    account_num: string;
}

export class SqsStack extends Stack {

    public readonly AssistancesLoggerSqs: sqs.Queue;

    constructor(scope: Construct, id: string, props: SqsStackProps) {
        super(scope, id, props);

        const AssistancesLoggerSqs = new sqs.Queue(this, 'AssistancesLoggerSqs', {
            queueName: "AssistancesLoggerSQS",
            retentionPeriod: cdk.Duration.days(4),
            visibilityTimeout: cdk.Duration.seconds(300),
            encryption: sqs.QueueEncryption.KMS_MANAGED
        })

        const lambdaAssistancesLoggerWriter = lambda.Function.fromFunctionArn(
            this,
            "lambdaAssistancesLoggerWriter",
            `arn:aws:lambda:${props.region}:${props.account_num}:function:${props.client}-${props.project}-AssistancesLoggerWriter`
        );

        lambdaAssistancesLoggerWriter.addEventSource(
            new SqsEventSource(AssistancesLoggerSqs, {
                batchSize: 1,
            })
        );

        this.AssistancesLoggerSqs = AssistancesLoggerSqs

        if (props.stage) cdk.Tags.of(this).add("Enviroment", props.stage);
        if (props.partner) cdk.Tags.of(this).add("Author", props.partner);
        if (props.client) cdk.Tags.of(this).add("Client", props.client);
        if (props.stage) cdk.Tags.of(this).add("Stage", props.stage.toUpperCase());
        if (props.partner) cdk.Tags.of(this).add("Partner", props.partner);
        if (props.project) cdk.Tags.of(this).add("Project", props.project);
    }
}
