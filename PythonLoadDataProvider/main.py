# -*- coding: utf-8 -*-
import uuid
import math

from excel_reader import ExcelReader    
from dynamodb_reader import Dynamodb
from Entities.provider_provider import Provider
from Entities.provider_location import ProviderLocation
from Entities.provider_contact import ProviderContact, ListData  
from datetime import datetime
from geohash_util import Geohash2

def safe_str(value):
    if value is None or (isinstance(value, float) and math.isnan(value)) or value == '':
        return None  # Retorna None si el valor es inválido
    
    # Si es un número flotante y no tiene decimales significativos, lo convertimos a entero
    if isinstance(value, float) and value.is_integer():
        return str(int(value))  # Convertir a entero antes de convertir a cadena

    return str(value)

def safe_double(value):
    try:
         return float(value)
    except (ValueError, TypeError):
         return None

def main():
    # Configuracion de la lectura de Excel
    excel_path = f'C:\\Users\\Milena\\Documents\\Data Providers\\PROVIDERS_NEW_2025_VF_CRIS_SOL.xlsx'
    reader = ExcelReader(excel_path)
    last_sheets = reader.get_last_sheets()
    # data = [item for sublist in last_sheets for item in sublist]
    data = reader.sheets_to_dict(last_sheets)
    
    current_time = datetime.now().isoformat()

    print("Lista provedor")
    print(data[0])
    
    # Listas donde se almacenarán las instancias de Provider, locatio   n y contact
    providers = []
    providersLocation = []
    providersContact = []
    
    dynamo_db_instance = Dynamodb('ProviderData', 'us-east-1')
    
    for itemprovider in data[0]:
        if dynamo_db_instance.check_if_provider_exists(itemprovider['nameProvider']):
            print(f"El proveedor {itemprovider['nameProvider']} ya existe, omitiendo.")
            continue  # Si ya existe, saltar esta iteración
        
        itemprovider['idProvider'] =   itemprovider['idAsignado']
        uuidprovider = "provider#" + str(itemprovider['idProvider'])
        itemprovider['partitionKey'] = uuidprovider
        itemprovider['clasificationkey'] = uuidprovider
        provider_instance = Provider(partition_key = itemprovider['partitionKey'], 
                                     clasification_key = itemprovider['clasificationkey'], 
                                     id_provider = itemprovider['idProvider'], 
                                     name = itemprovider['nameProvider'], 
                                     priorityLevel = itemprovider['priorityLevel'],
                                     score = itemprovider['score'], 
                                     typePro = itemprovider['typeProvider'],
                                     details=safe_str(itemprovider['details']),
                                     tags=safe_str(itemprovider['tags'])
                                     )
        
        # Agregar la instancia a la lista de providers
        providers.append(provider_instance)
        
        # Configuracion de DynamoDB   
    dynamo_db_instance.batch_insert_items(providers)

    print("Lista localizacion")    
    print(data[1])
    
    unique_providers = list({location['nameProvider'] for location in data[1]})

    for name_provider in unique_providers:
        provider_list = dynamo_db_instance.get_items_by_field('nameProvider', name_provider)
        providersLocation = []
        if provider_list and len(provider_list) > 0:
           provider = provider_list[0]
           provider_locations = [location for location in data[1] if location['nameProvider'] == name_provider]
           for itemproviderLocation in provider_locations:
             
                # Generar UUID para la localización
                itemproviderLocation['idLocation'] =  itemproviderLocation['idAsignado']
                uuidproviderlocation = "location#" + str(itemproviderLocation['idLocation'])
               
               # Crear una instancia de Geohash2 y obtener el geohash
                geohash_instance = Geohash2(safe_double(itemproviderLocation['latitude']), safe_double(itemproviderLocation['longitude']))
                geo = geohash_instance.get_geohash(precision=6)
             
                # Crear la instancia de ProviderLocation mapeando los datos
                providerLocation_instance = ProviderLocation(
                    partition_key=provider.get('PK'),
                    clasification_key=uuidproviderlocation,
                    id_provider=provider.get('idProvider'),
                    id_location=itemproviderLocation['idLocation'],
                    name=provider.get('nameProvider'),
                    typePro=provider.get('typeProvider'),
                    id_country=itemproviderLocation['idCountry'],
                    country=itemproviderLocation['country'],
                    id_city=itemproviderLocation['idCity'],
                    city=safe_str(itemproviderLocation['city']),
                    address=safe_str(itemproviderLocation.get('address', '')),  # Usar .get() para evitar errores si falta la clave
                    longitude=str(itemproviderLocation['longitude']),
                    latitude=str(itemproviderLocation['latitude']),
                    details=safe_str(itemproviderLocation['details']),
                    score=provider.get('score'),
                    geohash=geo,
                    geohashPK="geohashprefix", 
                    created_at=current_time,
                    updated_at=current_time
                )

                # Agregar la instancia a la lista de providers location
                providersLocation.append(providerLocation_instance)

                       # Configuracion de DynamoDB   
           dynamo_db_instance.batch_insert_items(providersLocation)

    print("Lista contacto")
    print(data[2])
    
    unique_providers_contact = list({contact['nameProvider'] for contact in data[2]})

    for name_provider_contact in unique_providers_contact:
        provider_list_con = dynamo_db_instance.get_items_by_field('nameProvider', name_provider_contact)
        if provider_list_con and len(provider_list_con) > 0:
           provider = provider_list_con[0]
           provider_contacts = [contact for contact in data[2] if contact['nameProvider'] == name_provider_contact]
           list_data_contact  = []
           for contact in provider_contacts:
                list_data_contact.append(ListData(
                    key=str(contact['key']),
                    value=str(contact['value'])
                ))
           id_contact = str(uuid.uuid4())
           provider_contact_instance = ProviderContact(
                     partition_key=provider.get('PK'),
                     clasification_key=f"contact#{id_contact}",
                    id_provider=provider.get('idProvider'),
                    id_contact=id_contact,
                    list_data=list_data_contact,
                    details=safe_str(provider_contacts[0].get('details', '')),  # Usamos el primer contacto para detalles
                    created_at=current_time,
                    updated_at=current_time
            )
           providersContact.append(provider_contact_instance)
      
    dynamo_db_instance.batch_insert_items_contact(providersContact)

    print("Finalizacion carga")

if __name__ == '__main__':
    main()