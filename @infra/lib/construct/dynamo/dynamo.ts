import * as cdk from "aws-cdk-lib";
import { Construct } from "constructs";
import * as dynamodb from "aws-cdk-lib/aws-dynamodb";

export interface TableDefinition {
  tableName: string;
  partitionKey: dynamodb.Attribute;
  sortKey?: dynamodb.Attribute;
  billingMode?: dynamodb.BillingMode;
  removalPolicy?: cdk.RemovalPolicy;
  globalSecondaryIndexes?: {
    indexName: string;
    partitionKey: dynamodb.Attribute;
    sortKey?: dynamodb.Attribute;
    projectionType?: dynamodb.ProjectionType;
  }[];
  tags?: { [key: string]: string };
}

export interface DynamoConstructProps extends cdk.StackProps {
  tables: TableDefinition[];
}

/**
 * Construct para crear múltiples tablas DynamoDB con configuración avanzada y valores por defecto.
 */
export class DynamoConstruct extends Construct {
  public readonly tables: { [key: string]: dynamodb.Table } = {};

  constructor(scope: Construct, id: string, props: DynamoConstructProps) {
    super(scope, id);

    props.tables.forEach((tableDef) => {
      const table = new dynamodb.Table(this, `Table-${tableDef.tableName}`, {
        tableName: tableDef.tableName,
        partitionKey: tableDef.partitionKey,
        sortKey: tableDef.sortKey, // puede ser undefined
        billingMode: tableDef.billingMode ?? dynamodb.BillingMode.PAY_PER_REQUEST,
        removalPolicy: tableDef.removalPolicy ?? cdk.RemovalPolicy.DESTROY, // por defecto DESTROY para dev/test
      });

      // Crear índices secundarios globales si están definidos y no vacíos
      if (tableDef.globalSecondaryIndexes && tableDef.globalSecondaryIndexes.length > 0) {
        tableDef.globalSecondaryIndexes.forEach((gsi) => {
          table.addGlobalSecondaryIndex({
            indexName: gsi.indexName,
            partitionKey: gsi.partitionKey,
            sortKey: gsi.sortKey,
            projectionType: gsi.projectionType ?? dynamodb.ProjectionType.ALL,
          });
        });
      }

      // Agregar tags solo si están definidos y no vacíos
      if (tableDef.tags && Object.keys(tableDef.tags).length > 0) {
        Object.entries(tableDef.tags).forEach(([key, value]) => {
          cdk.Tags.of(table).add(key, value);
        });
      }

      this.tables[tableDef.tableName] = table;
    });
  }
}
