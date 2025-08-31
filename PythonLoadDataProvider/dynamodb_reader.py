import boto3

from boto3 import session
from boto3.dynamodb.conditions import Attr, BeginsWith
from botocore.exceptions import ClientError

class Dynamodb:
    def __init__(self, table_name, region='us-east-1'):
        """Inicializa el objeto DynamoDB con la tabla y región especificada."""
        self.table_name = table_name
        self.region = region
        self.table = self.setup_dynamodb()

    def setup_dynamodb(self):
        """Configura y retorna un objeto tabla de DynamoDB."""
        session = boto3.Session(profile_name = 'assistence-dev', region_name=self.region )
        dynamodb = session.resource('dynamodb')
        return dynamodb.Table(self.table_name)

    def load_data_to_dynamodb(self, data):
        """Carga una lista de registros en la tabla DynamoDB."""
        try:
            for record in data:
                self.table.put_item(Item=record.to_dynamo_dict())
            print("Datos cargados en DynamoDB con éxito.")
        except ClientError as e:
            print(f"Error al cargar datos en DynamoDB: {e.response['Error']['Message']}")
    
    def get_item(self, partition_key_value, sort_key_value=None):
        """Obtiene un item de la tabla usando la Partition Key y Sort Key (opcional)."""
        key = {'PK': partition_key_value}
        if sort_key_value:
            key['SK'] = sort_key_value
        
        try:
            response = self.table.get_item(Key=key)
            return response.get('Item', None)
        except ClientError as e:
            print(f"Error al obtener el item: {e.response['Error']['Message']}")
            return None

    def get_items_by_field(self, field_name, field_value):
        """Obtiene items que coincidan con un valor de campo específico usando un scan."""
        try:
            response = self.table.scan(
                  FilterExpression=Attr(field_name).eq(field_value) & Attr('SK').begins_with('provider#')
            )
            return response.get('Items', [])
        except ClientError as e:
            print(f"Error al obtener items por campo: {e.response['Error']['Message']}")
            return []

    def update_item(self, partition_key_value, sort_key_value, update_expression, expression_attribute_values):
        """Actualiza un item en la tabla."""
        key = {'PK': partition_key_value, 'SK': sort_key_value}
        try:
            response = self.table.update_item(
                Key=key,
                UpdateExpression=update_expression,
                ExpressionAttributeValues=expression_attribute_values,
                ReturnValues="UPDATED_NEW"
            )
            print("Item actualizado con éxito:", response)
            return response
        except ClientError as e:
            print(f"Error al actualizar el item: {e.response['Error']['Message']}")
            return None

    def delete_item(self, partition_key_value, sort_key_value):
        """Elimina un item de la tabla usando la Partition Key y Sort Key."""
        key = {'PK': partition_key_value, 'SK': sort_key_value}
        try:
            response = self.table.delete_item(Key=key)
            print("Item eliminado con éxito.")
            return response
        except ClientError as e:
            print(f"Error al eliminar el item: {e.response['Error']['Message']}")
            return None
        
    def batch_insert_items(self, items):
        """
        Inserta múltiples items en la tabla de DynamoDB usando batch_write_item.
        Puede insertar hasta 25 items a la vez.
        """
        first_provider_name = items[0].name if items else 'N/A'
        try:
            table = self.table

            # Lote para las inserciones
            with table.batch_writer() as batch:
                for item in items:
                   batch.put_item(Item=item.to_dynamo_dict())     
            print(f"{len(items)} registros insertados exitosamente en DynamoDB.{first_provider_name}" )
        except ClientError as e:
            print(f"Error al insertar los registros: {e.response['Error']['Message']} proveddor {first_provider_name}")
       
    def batch_insert_items_contact(self, items):
        """
        Inserta múltiples items en la tabla de DynamoDB usando batch_write_item.
        Puede insertar hasta 25 items a la vez.
        """
        try:
            table = self.table

            # Lote para las inserciones
            with table.batch_writer() as batch:
                for item in items:
                   print(item.to_dynamo_dict())  # <-- Añade esto
                   batch.put_item(Item=item.to_dynamo_dict())     
            print(f"{len(items)} registros insertados exitosamente en DynamoDB." )
        except ClientError as e:
            print(f"Error al insertar los registros: {e.response['Error']['Message']} proveddor")
        
    def check_if_provider_exists(self, name_provider):
        """Verifica si un proveedor con el nombre dado ya existe en la tabla."""
        try:
            response = self.table.scan(
                FilterExpression=Attr('nameProvider').eq(name_provider) & Attr('SK').begins_with('provider#')
            )
            items = response.get('Items', [])
            return len(items) > 0  # Retorna True si ya existe, False si no
        except ClientError as e:
            print(f"Error al verificar el proveedor: {e.response['Error']['Message']}")
            return False  