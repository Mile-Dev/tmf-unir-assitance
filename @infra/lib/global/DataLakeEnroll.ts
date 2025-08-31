import * as glue from '@aws-cdk/aws-glue-alpha';
import * as iam from 'aws-cdk-lib/aws-iam';
import * as lf from 'aws-cdk-lib/aws-lakeformation';
import { Construct } from "constructs";


/**
 * 
 * @description - Funcion que permite dar permisos sobre una base de datos del catalogo a un rol en particular
 * 
 * @param {Construct} scope 
 * @param {iam.IRole} role 
 * @param {clue.Database} catalogDb 
 * @param {string[]} basePermissions 
 */
export function GiveDatabaseCatalogPermissions(scope: Construct, role:iam.IRole, catalogDb: glue.Database, basePermissions:string[]) : void {


    const dbPermissions = new lf.CfnPermissions(scope, `${role.node.id.substring(role.node.id.length - 10)}-${catalogDb.node.id}-lf-permission`, {
        dataLakePrincipal: {
          dataLakePrincipalIdentifier: role.roleArn,
        },
        resource: {
          databaseResource: {
            name: catalogDb.databaseName
          },
        },
        permissions: basePermissions,
      });
      dbPermissions.node.addDependency(catalogDb);
}



/**
 * 
 * @description - Funcion que permite dar permisos sobre todas las tablas en la DB del catalogo a un rol o user en particular
 * 
 * @param {Construct} scope 
 * @param {string} catalogId - Id del catalogo 
 * @param {string} dataLakePrincipalArn - arn del role o user a quien se dara permisos
 * @param {string} dbname - nombre de la DB que contiene las tablas
 * @param {string[]} permissions 
 * @param {string[]} grantablePremissions
 * @return {lf.CfnPermissions} - Set de permisos LakeFormation
 */
export function createLakeFormationAllTablePermission(scope: Construct, catalogId: string, dataLakePrincipalArn: string, dbname: glue.Database, permissions: string[], grantablePremissions: string[]){

  return new lf.CfnPermissions(scope, `${dataLakePrincipalArn.substring(dataLakePrincipalArn.length -10)}-alltable-${dbname}-lf-permission`, {
    dataLakePrincipal: {
      dataLakePrincipalIdentifier: dataLakePrincipalArn,
    },
    resource: {
      tableResource: {
        catalogId: catalogId,
        databaseName: dbname.databaseName,
        tableWildcard: { },
      },
    },
    permissions: permissions,
    permissionsWithGrantOption: grantablePremissions
  })
}



