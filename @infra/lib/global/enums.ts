/**
 * @module Enum
 */


/**
 * @description - Athena Tables permissions
 * 
 * @enum {string} Permissions
 */
export enum Permissions {
  ALTER = 'ALTER',
  CREATE_DATABASE = 'CREATE_DATABASE',
  CREATE_TABLE = 'CREATE_TABLE',
  DATA_LOCATION_ACCESS = 'DATA_LOCATION_ACCESS',
  DELETE = 'DELETE',
  DESCRIBE = 'DESCRIBE',
  DROP = 'DROP',
  INSERT = 'INSERT',
  SELECT = 'SELECT',
  ASSOCIATE = 'ASSOCIATE',
  CREATE_TABLE_READ_WRITE = 'CREATE_TABLE_READ_WRITE',
}


/**
 * @description - Nombre del stage a desplegar
 * 
 * @enum {string} Stage
 */
export enum Stage {
  ALPHA = 'alpha',
  BETA = 'beta',
  THETA = 'theta',
  DEV = 'dev',
  QA = 'qa',
  PROD = 'prod',
}



/**
 * @description - Nombre de las etapas/Buckets dentro del Lake
 * 
 * @enum {string} DataTier
 */
export enum DataTier {
  RAW = 'raw',
  STAGING = 'staging',
  ANALYTICS = 'analytics',
  LOG = 'log',
  MULTI = 'multi',
  SERVICE = 'glueService'
}


/**
 * @description - Nombre de los Buckets de servicio dentro del Lake
 * 
 * @enum {string} ServiceTier
 */
export enum ServiceTier {
  ML = 'ml',
  DATASET = 'dataset',
  ARTIFACTS = 'artifacts',
  JOBSCRIPTS = 'jobscripts'
}




/**
 * @description - Listado de regiones disponibles en AWS
 * 
 * @enum {string} AWSRegion
 */
export enum AWSRegion {
  OHIO = 'us-east-2',
  NVIRGINIA = 'us-east-1',
  NCALIFORNIA = 'us-west-1',
  OREGON = 'us-west-2',
  AFRICA = 'af-south-1',
  HONGKONG = 'ap-east-1',
  MUMBAI = 'ap-south-1',
  OSAKA = 'ap-northeast-3',
  SEOUL = 'ap-northeast-2',
  SINGAPORE = 'ap-southeast-1',
  SYDNEY = 'ap-southeast-2',
  TOKYO = 'ap-northeast-1',
  CANADA = 'ca-central-1',
  BEIJING = 'cn-north-1',
  FRANKFURT = 'eu-central-1',
  LONDON = 'eu-west-2',
  PARIS = 'eu-west-3',
  MILAN = 'eu-south-1',
  UAE = 'me-central-1',
  SAOPAULO = 'sa-east-1'
}



/**
 * @description - Namespace con la info relacionada a instancias de sagemaker
 * 
 * @namespace
 */
export namespace SagemakerInstances {

  /**
   * @description - These instances come with a balance of CPUs and Memory. The more CPU cores, the higher the memory size 
   * that comes with the instances. These are general purpose instances and can be used as your initial training instance 
   * for testing.
   * 
   * @enum {string} GENERAL
   */
  export enum GENERAL {
    T3_MEDIUM = "ml.t3.medium",
    T3_LARGE = "ml.t3.large",
    T3_XLARGE = "ml.t3.xlarge",
    T3_2XLARGE = "ml.t3.2xlarge",
    M5_LARGE = "ml.m5.large",
    M5_XLARGE = "ml.m5.xlarge",
    M5_2XLARGE = "ml.m5.2xlarge",
    M5_4XLARGE = "ml.m5.4xlarge",
    M5_8XLARGE = "ml.m5.8xlarge",
    M5_12XLARGE = "ml.m5.12xlarge",
    M5_16XLARGE = "ml.m5.16xlarge",
    M5_24XLARGE = "ml.m5.24xlarge",
    M5D_LARGE = "ml.m5d.large",
    M5D_XLARGE = "ml.m5d.xlarge",
    M5D_2XLARGE = "ml.m5d.2xlarge",
    M5D_4XLARGE = "ml.m5d.4xlarge",
    M5D_8XLARGE = "ml.m5d.8xlarge",
    M5D_12XLARGE = "ml.m5d.12xlarge",
    M5D_16XLARGE = "ml.m5d.16xlarge",
    M5D_24XLARGE = "ml.m5d.24xlarge"
  }


  /**
   * @description - In these instances the balance is tipped towards CPU power. These instances are more appropriate 
   * for training jobs that consume more processing power and less memory. No GPU
   * 
   * @enum {string} COMPUTE
   */
  export enum COMPUTE {
    C5_LARGE = "ml.c5.large",
    C5_XLARGE = "ml.c5.xlarge",
    C5_2XLARGE = "ml.c5.2xlarge",
    C5_4XLARGE = "ml.c5.4xlarge",
    C5_9XLARGE = "ml.c5.9xlarge",
    C5_12XLARGE = "ml.c5.12xlarge",
    C5_18XLARGE = "ml.c5.18xlarge"
  }


  /**
   * @description - 
   * 
   * @enum {string} MEMORY
   */
  export enum MEMORY {
    R5_LARGE = "ml.r5.large",
    R5_XLARGE = "ml.r5.xlarge",
    R5_2XLARGE = "ml.r5.2xlarge",
    R5_4XLARGE = "ml.r5.4xlarge",
    R5_8XLARGE = "ml.r5.8xlarge",
    R5_12XLARGE = "ml.r5.12xlarge",
    R5_16XLARGE = "ml.r5.16xlarge",
    R5_24XLARGE = "ml.r5.24xlarge",
  }


  /**
   * @description - GPUs are more efficient in computations as they can do it in parallel. However, 
   * in order to use these instances, we have to make sure that the training algorithm supports GPUs. Although, 
   * these instances are generally more expensive compared to previously mentioned instances, the reduction in time 
   * may result in lower overall price.
   * 
   * @enum {string} ACCELERATED
   */
  export enum ACCELERATED {
    P3_2XLARGE = "ml.p3.2xlarge",
    P3_8XLARGE = "ml.p3.8xlarge",
    P3_16XLARGE = "ml.p3.16xlarge",
    P2_XLARGE = "ml.p2.xlarge",
    P2_8XLARGE = "ml.p2.8xlarge",
    P2_16XLARGE = "ml.p2.16xlarge",
    G4DN_XLARGE = "ml.g4dn.xlarge",
    G4DN_2XLARGE = "ml.g4dn.2xlarge",
    G4DN_4XLARGE = "ml.g4dn.4xlarge",
    G4DN_8XLARGE = "ml.g4dn.8xlarge",
    G4DN_12XLARGE = "ml.g4dn.12xlarge",
    G4DN_16XLARGE = "ml.g4dn.16xlarge",
  }


  export enum InferenceAcceleration {
    EIA2_MEDIUM = "ml.eia2.medium",
    EIA2_LARGE = "ml.eia2.large",
    EIA2_XLARGE = "ml.eia2.xlarge",
    EIA1_MEDIUM = "ml.eia1.medium",
    EIA1_LARGE = "ml.eia1.large",
    EIA1_XLARGE = "ml.eia1.xlarge"
  }


}

export enum GlueWorkerType {
  STANDARD = 'Standard',
  G1_X = 'G.1X',
  G2_X = 'G.2X'
}

export enum GlueVersion {
  V_2 = '2.0',
  V_3 = '3.0',
  V_4 = '4.0'
}

export enum JobType{
  PythonShell = "pythonshell",
  Spark = "glueetl"

}

/**
 * @description - Namespace para uso de redshift node types y otra info relacionada
 *
 * @namespace
 */
export namespace redshiftNodeTypes {
  /**
   * @description - Amazon Redshift offers different node types to accommodate your workloads,
   * and we recommend choosing RA3 or DC2 depending on the required performance, data size, and growth.
   * RA3 nodes with managed storage allow you to optimize your data warehouse by scaling and paying for compute and managed storage independently
   * DC2 nodes enable compute-intensive data warehouses with local SSD storage included.
   * @see https://docs.aws.amazon.com/redshift/latest/mgmt/working-with-clusters.html
   *
   * @enum {string} NodeTypes
   */
  export enum RSNodeTypes {
    DC2_LARGE = "dc2.large",
    DC2_8XLARGE = "dc2.8xlarge",
    RA3_LARGE = "ra3.large",
    RA3_XLPLUS = "ra3.xlplus",
    RA3_4XLARGE = "ra3.4xlarge",
    RA3_16XLARGE = "ra3.16xlarge",
  }

  /**
   * @description - The type of the cluster. When cluster type is specified as
   * single-node, the NumberOfNodes parameter is not required.
   * multi-node, the NumberOfNodes parameter is required.
   *
   * @enum {string} ClusterType
   */
  export enum ClusterType {
    SINGLE_MODE = "single-node",
    MULTI_MODE = "multi-node",
  }
}

/**
 * @description - Namespace para uso de tipos de instancias de replicacion para DMS y otra
 *                Info relacionada a DMS
 * @namespace
 */
export namespace DMS {

  /**
   * @description - Valid Engine versions for the replication instance
   * 
   * @enum {string} EngineVersion
   */
  export enum EngineVersion {
    V3_4_7 = '3.4.7',
    V3_4_6 = '3.4.6',
    V3_4_5 = '3.4.5',
    V3_5_1 = '3.5.1'
  }

  /**
   * @description - Endpoint types
   * 
   * @enum {string} EndpointType
   */
  export enum EndpointType {
    SOURCE = 'source',
    TARGET = 'target'
  }


  export enum EndpointEngineName {
    MYSQL = 'mysql',
    ORACLE = 'oracle',
    POSTGRES = 'postgres',
    MARIADB = 'mariadb',
    AURORA = 'aurora',
    AURORA_POSTGRESQL = 'aurora-postgresql',
    OPENSEARCH = 'opensearch',
    REDSHIFT = 'redshift',
    S3 = 's3',
    DB2="db2", 
    AZUREDB="azuredb", 
    SYBASE="sybase", 
    DYNAMODB="dynamodb", 
    MONGODB="mongodb", 
    KINESIS="kinesis", 
    KAFKA="kafka", 
    ELASTICSEARCH="elasticsearch", 
    DOCDB="docdb", 
    SQLSERVER="sqlserver", 
    NEPTUNE="neptune",
  }

  /**
   * @description - Migration Type for DMS Tasks
   * 
   * @enum {string} MigrationType
   */
  export enum MigrationType {
    FULL_LOAD='full-load',
    CDC='cdc',
    FULL_LOAD_AND_CDC='full-load-and-cdc'
  }

  /**
   * @description - Instance types for the replication instance
   * 
   * @enum {string} InstanceType
   */
  export enum InstanceType {
    DMS_T3_MICRO = 'dms.t3.micro',
    DMS_T3_SMALL = 'dms.t3.small',
    DMS_T3_MEDIUM = 'dms.t3.medium',
    DMS_T3_LARGE = 'dms.t3.large',
    DMS_C5_LARGE = 'dms.c5.large',
    DMS_C5_XLARGE = 'dms.c5.xlarge',
    DMS_C5_2XLARGE = 'dms.c5.2xlarge',
    DMS_C5_4XLARGE = 'dms.c5.4xlarge',
    DMS_C5_9XLARGE = 'dms.c5.9xlarge',
    DMS_C5_12XLARGE = 'dms.c5.12xlarge',
    DMS_C5_18XLARGE = 'dms.c5.18xlarge',
    DMS_C5_24XLARGE = 'dms.c5.24xlarge',
    DMS_R5_LARGE = 'dms.r5.large',
    DMS_R5_XLARGE = 'dms.r5.xlarge',
    DMS_R5_2XLARGE = 'dms.r5.2xlarge',
    DMS_R5_4XLARGE = 'dms.r5.4xlarge',
    DMS_R5_9XLARGE = 'dms.r5.9xlarge',
    DMS_R5_12XLARGE = 'dms.r5.12xlarge',
    DMS_R5_18XLARGE = 'dms.r5.18xlarge',
    DMS_R5_24XLARGE = 'dms.r5.24xlarge',

  }

}

/**
 * @description - Tipo del secret a crear,
 * 
 * tipos disponibles:
 *  DMS:
 *    Estructura:
 *      - engineName
 *      - username
 *      - port
 *      - databaseName
 *      - serverName
 *      - password
 * 
 *  
 * 
 * @enum {string} Stage
 */
export enum SecretType {
  DMS_SECRET = 'dms',
  RDS_SECRET = 'rds',
  //TODO: Implement other types
}














