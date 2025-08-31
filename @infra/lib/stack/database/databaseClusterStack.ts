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
}

export class DatabaseStack extends cdk.Stack {

  constructor(scope: Construct, id: string, props: DatabaseProps) {
    super(scope, id, props);

    const {vpc,client,project,stage,isPubliclyAccessible} = props;
  
    // Define el nombre fijo para el secreto
    const secretName = `${project}-sm`;

    // Crea un secreto en AWS Secrets Manager
    const dbCredentialsSecret = new secretsmanager.Secret(this, 'DBCredentialsSecret', {
      secretName: secretName,
      generateSecretString: {
        secretStringTemplate: JSON.stringify({ username: 'dbadmin' }), // Cambia 'admin' según sea necesario
        generateStringKey: 'password',
        excludeCharacters: '"@()#;=[]{}/\\\'',
      },
    });

    // Crea un cluster de Aurora con PostgreSQL
    const cluster = new rds.DatabaseCluster(this, `SglCluster${stage}`, {
      engine: rds.DatabaseClusterEngine.auroraPostgres({
        version: rds.AuroraPostgresEngineVersion.VER_15_2, // Especifica la versión de PostgreSQL
      }),
      clusterIdentifier: `${project}-cluster`,
      credentials: rds.Credentials.fromSecret(dbCredentialsSecret), // Genera credenciales automáticamente
      writer: rds.ClusterInstance.provisioned('writer', {
        instanceType: ec2.InstanceType.of(ec2.InstanceClass.T4G, ec2.InstanceSize.MEDIUM),
        publiclyAccessible: isPubliclyAccessible,
        
      }),
      // readers: [
      //   // will be put in promotion tier 1 and will scale with the writer
      //   // rds.ClusterInstance.serverlessV2('reader1', { scaleWithWriter: true }),
      //   // will be put in promotion tier 2 and will not scale with the writer
      //   // rds.ClusterInstance.serverlessV2('reader2'),
      //   rds.ClusterInstance.provisioned('reader1', {
      //     instanceType: ec2.InstanceType.of(ec2.InstanceClass.T4G, ec2.InstanceSize.MEDIUM),
      //   })
      // ],
      vpc,
      securityGroups: [ props.securityGroup ],
      vpcSubnets: {
         // Usa los IDs de subred directamente para especificar las subredes
         subnetType: isPubliclyAccessible ? ec2.SubnetType.PUBLIC: ec2.SubnetType.PRIVATE_WITH_EGRESS,
      },
      defaultDatabaseName: 'SGLC',
      iamAuthentication: true, // Habilita la autenticación IAM
    });


  }
}
