import * as cdk from 'aws-cdk-lib';
import * as ec2 from 'aws-cdk-lib/aws-ec2'; // Importa el módulo EC2
import { Construct } from 'constructs';

export interface NetworkingProps extends cdk.StackProps {
  cidr: string;
  client:string;
  project:string;
  stage:string;
  numberNatGateways: number;
}

export class NetworkingStack extends cdk.Stack {
  public readonly vpc: ec2.Vpc;
  public readonly dbSg: ec2.SecurityGroup;
  public readonly lambdaSg: ec2.SecurityGroup;
  constructor(scope: Construct, id: string, props: NetworkingProps) {
    super(scope, id, props);

    const { cidr } = props

    


    // Crear VPC con 2 AZs
    const vpc = new ec2.Vpc(this, `${props.client}-${props.project}-vpc-${props.stage}`, {
      vpcName: `${props.client}-${props.project}-vpc-${props.stage}`,
      maxAzs: 3,
      ipAddresses: ec2.IpAddresses.cidr(cidr),
      subnetConfiguration: [{
        cidrMask: 19,
        name: 'Public',
        subnetType: ec2.SubnetType.PUBLIC,
      }, {
        cidrMask: 19,
        name: 'Private',
        subnetType: ec2.SubnetType.PRIVATE_WITH_EGRESS,
      }
      ],
      natGateways: props.numberNatGateways,
    });

    const DB_PORT = 5432


    const securityGroup = new ec2.SecurityGroup(this, `SecurityGropForAurora${props.stage}`, {
      vpc,
      description: 'Aurora SG',
      allowAllOutbound: true,   //Change to false if you want to restrict outbound traffic
      securityGroupName: `${props.client}-${props.project}-aurora-sg-${props.stage}`
    });

    securityGroup.connections.allowInternally(ec2.Port.allTraffic(),'Allow Every Member of this SG To Access the DB')

    const securityLambdaGroup = new ec2.SecurityGroup(this, `SecurityGropForLambda${props.stage}`, {
      vpc,
      description: 'Lambda SG',
      allowAllOutbound: true,   //Change to false if you want to restrict outbound traffic
      securityGroupName: `${props.client}-${props.project}-lambda-sg-${props.stage}`
    });

    securityGroup.addIngressRule(securityLambdaGroup,cdk.aws_ec2.Port.MYSQL_AURORA,"Allow lambdas Access the DB")


    this.vpc = vpc
    this.dbSg = securityGroup
    this.lambdaSg = securityLambdaGroup
  }
}