import * as cdk from 'aws-cdk-lib';
import * as ec2 from 'aws-cdk-lib/aws-ec2';
import * as rds from 'aws-cdk-lib/aws-rds';
import * as secretsmanager from 'aws-cdk-lib/aws-secretsmanager';
import { Construct } from 'constructs';

interface DatabaseProps extends cdk.StackProps {
  vpc: ec2.IVpc;
  client:string;
  project:string;
  stage:string;
  isPubliclyAccessible:boolean;
  securityGroup: ec2.SecurityGroup;
  completeSecretARN: string;
}

export class DatabaseStack extends cdk.Stack {

  constructor(scope: Construct, id: string, props: DatabaseProps) {
    super(scope, id, props);
  
    // Cargar el secreto existente por ARN o nombre
    // Si usas ARN completo:
    const secret = secretsmanager.Secret.fromSecretCompleteArn(this, 'DbSecret', props.completeSecretARN);
    // Si prefieres usar el nombre del secreto, usa:
    // const secret = secretsmanager.Secret.fromSecretNameV2(this, 'DbSecret', props.secretArnOrName);


    const database = new rds.DatabaseInstance(this, 'RdsInstance', {
      engine: rds.DatabaseInstanceEngine.mysql({ version: rds.MysqlEngineVersion.VER_8_0 }),
      instanceType: ec2.InstanceType.of(ec2.InstanceClass.T4G, ec2.InstanceSize.MICRO),
      vpc: props.vpc,
      securityGroups: [ props.securityGroup ],
      vpcSubnets: {
         // Usa los IDs de subred directamente para especificar las subredes
         subnetType: ec2.SubnetType.PRIVATE_WITH_EGRESS,
      },
      multiAz: false,
      deletionProtection: false,
      publiclyAccessible: false,
      credentials: rds.Credentials.fromSecret(secret),  // Usar secreto existente aquí
      databaseName: "assists",
      instanceIdentifier: "asistencia-viajero-database"
    });



  }
}
