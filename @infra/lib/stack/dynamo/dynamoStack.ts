import { Stack } from "aws-cdk-lib";
import { Construct } from "constructs";
import * as cdk from "aws-cdk-lib";

import { DynamoConstruct, TableDefinition } from "../../construct/dynamo/dynamo";
import * as dynamodb from "aws-cdk-lib/aws-dynamodb";

export interface DynamoStackProps extends cdk.StackProps {
    region: string;
    client: string;
    project: string;
    partner: string;
    stage: string;
    account_num: string;
}

export class DynamoStack extends Stack {
    constructor(scope: Construct, id: string, props: DynamoStackProps) {
        super(scope, id, props);

        const tables: TableDefinition[] = [
            {
                tableName: "AssistancesLoggerDB",
                partitionKey: { name: "PK", type: dynamodb.AttributeType.STRING },
                sortKey: { name: "SK", type: dynamodb.AttributeType.STRING },
            },
            {
                tableName: "CountriesAndCities",
                partitionKey: { name: "PK", type: dynamodb.AttributeType.STRING },
                sortKey: { name: "SK", type: dynamodb.AttributeType.STRING },
            },
            {
                tableName: "EventDraft",
                partitionKey: { name: "PK", type: dynamodb.AttributeType.STRING },
                sortKey: { name: "SK", type: dynamodb.AttributeType.STRING },
                // globalSecondaryIndexes: [
                //     {
                //         indexName: "SK-createdAt-index",
                //         partitionKey: { name: "SK", type: dynamodb.AttributeType.STRING },
                //         sortKey: { name: "createdAt ", type: dynamodb.AttributeType.STRING },
                //     },
                // ],
            },
            {
                tableName: "IssuanceMok",
                partitionKey: { name: "PK", type: dynamodb.AttributeType.STRING },
            },
            {
                tableName: "PhoneConsultations",
                partitionKey: { name: "PK", type: dynamodb.AttributeType.STRING },
                sortKey: { name: "SK", type: dynamodb.AttributeType.STRING },
            },
            {
                tableName: "ProviderData",
                partitionKey: { name: "PK", type: dynamodb.AttributeType.STRING },
                sortKey: { name: "SK", type: dynamodb.AttributeType.STRING },
                billingMode: dynamodb.BillingMode.PAY_PER_REQUEST,
                globalSecondaryIndexes: [
                    {
                        indexName: "geohashPK-index",
                        partitionKey: { name: "geohashPK", type: dynamodb.AttributeType.STRING },
                    },
                    {
                        indexName: "idCountry-index",
                        partitionKey: { name: "idCountry", type: dynamodb.AttributeType.STRING },
                    },
                    {
                        indexName: "idCity-index",
                        partitionKey: { name: "idCity", type: dynamodb.AttributeType.STRING },
                    },
                    {
                        indexName: "nameProvider-index",
                        partitionKey: { name: "nameProvider", type: dynamodb.AttributeType.STRING },
                    },
                ]
            },
            {
                tableName: "ProviderDataLoad",
                partitionKey: { name: "PK", type: dynamodb.AttributeType.STRING },
                sortKey: { name: "SK", type: dynamodb.AttributeType.STRING },
            },
        ];

        const dynamo = new DynamoConstruct(this, "DynamoDBTables", { tables });

        if (props.stage) cdk.Tags.of(this).add("Enviroment", props.stage);
        if (props.partner) cdk.Tags.of(this).add("Author", props.partner);
        if (props.client) cdk.Tags.of(this).add("Client", props.client);
        if (props.stage) cdk.Tags.of(this).add("Stage", props.stage.toUpperCase());
        if (props.partner) cdk.Tags.of(this).add("Partner", props.partner);
        if (props.project) cdk.Tags.of(this).add("Project", props.project);
    }
}
