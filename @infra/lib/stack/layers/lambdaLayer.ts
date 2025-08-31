import * as lambda from "aws-cdk-lib/aws-lambda";
import * as cdk from 'aws-cdk-lib';
import * as crypto from 'node:crypto';

import { Construct } from "constructs";
import path = require("path");


export interface LambdaLayersStackProps extends cdk.StackProps {
    region: string;
    client: string;
    project: string;
    partner: string;
    stage: string;
    account_num: string;

}
export class LambdaLayersStack extends cdk.Stack {

    public readonly arnLayerTrackingMok: string;

    constructor(scope: Construct, id: string, props: LambdaLayersStackProps) {
        super(scope, id, props);

        const layerJWT = new lambda.LayerVersion(this, 'layerJWT', {
            layerVersionName: `${props.project}-jwt`,
            removalPolicy: cdk.RemovalPolicy.RETAIN,
            code: lambda.Code.fromAsset('src/layers/jwt', {
                bundling: {
                    image: lambda.Runtime.PYTHON_3_13.bundlingImage,  // Imagen Docker con Python 3.9
                    user: "root",
                    command: [
                        "bash",
                        "-c",
                        [
                            "ls",
                            `pip install -r requirements.txt -t /asset-output/python`,
                            "ls /asset-output/python"
                        ].join(" && "),
                    ],
                }
            }),
            compatibleRuntimes: [lambda.Runtime.PYTHON_3_13],
            description: 'Layer con dependencias/utilidades comunes para Lambdas Python'
        });

        const layerTrackingMok = new lambda.LayerVersion(this, 'trackingMok', {
            layerVersionName: `${props.project}-trackingMok`,
            removalPolicy: cdk.RemovalPolicy.RETAIN,
            code: lambda.Code.fromAsset('src/layers/trackingMok', {
                bundling: {
                    image: lambda.Runtime.PYTHON_3_13.bundlingImage,  // Imagen Docker con Python 3.9
                    user: "root",
                    command: [
                        "bash",
                        "-c",
                        [
                            "ls",
                            `pip install -r requirements.txt -t /asset-output/python`,
                            "ls /asset-output/python"
                        ].join(" && "),
                    ],
                }
            }),
            compatibleRuntimes: [lambda.Runtime.PYTHON_3_13],
            description: 'Layer con dependencias/utilidades comunes para Lambdas Python'
        });

        this.arnLayerTrackingMok = layerTrackingMok.layerVersionArn

    }


}