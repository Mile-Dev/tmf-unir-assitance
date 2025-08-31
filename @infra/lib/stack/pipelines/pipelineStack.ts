import { Stack } from "aws-cdk-lib";
import { Construct } from "constructs";
import * as cdk from "aws-cdk-lib";
import { Vpc } from 'aws-cdk-lib/aws-ec2';
import * as ec2 from 'aws-cdk-lib/aws-ec2';

import {CodePipelineConstruct} from "../../construct/pipeline/pipeline";

export interface pipelineStackProps extends cdk.StackProps {
  region: string;
  client: string;
  project: string;
  partner: string;
  stage: string;
  account_num: string;
  stageName: string;
  repoName: string;
  connectionArn: string;
  repoOwner:string;
  repoBranch: string;
  service: string;
  vpc?: ec2.IVpc;
  securityGroup?: ec2.SecurityGroup;

}

export class pipelineStack extends Stack {
  constructor(scope: Construct, id: string, props: pipelineStackProps) {
    super(scope, id);

    new CodePipelineConstruct(this,"CodePipelineConstruct",props)

    if (props.stage) cdk.Tags.of(this).add("Enviroment", props.stage);
    if (props.partner) cdk.Tags.of(this).add("Author", props.partner);
    if (props.client) cdk.Tags.of(this).add("Client", props.client);
    if (props.stage) cdk.Tags.of(this).add("Stage", props.stage.toUpperCase());
    if (props.partner) cdk.Tags.of(this).add("Partner", props.partner);
    if (props.project) cdk.Tags.of(this).add("Project", props.project);
  }
}

