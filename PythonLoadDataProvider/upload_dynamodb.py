import math
import boto3
import json
import sys
import os

sys.path.append(os.path.abspath("."))

from excel_reader import ExcelReader
from dynamodb_reader import Dynamodb
from Entities.location_country import LocationCountry
from Entities.location_cities import LocationCities

def safe_str(value):
    if value is None or (isinstance(value, float) and math.isnan(value)) or value == '':
        return None  # Retorna None si el valor es inválido
    
    # Si es un número flotante y no tiene decimales significativos, lo convertimos a entero
    if isinstance(value, float) and value.is_integer():
        return str(int(value))  # Convertir a entero antes de convertir a cadena

    return str(value)

def load_data_to_dynamodb(file_path):
    """Carga datos desde un archivo JSON a DynamoDB"""
    reader = ExcelReader(file_path)
    last_sheets = reader.get_last_sheets()
    # data = [item for sublist in last_sheets for item in sublist]
    data = reader.sheets_to_dict(last_sheets)
    
    countries = []
    cities = []

    dynamo_db_instance = Dynamodb('CountriesAndCities', 'us-east-2')
    print("Lista Country")    

    for itemcountry in data[0]:
        country_instance = LocationCountry(partition_key = itemcountry['PK'],
                                           clasification_key = itemcountry['SK'],
                                           Name = safe_str(itemcountry['Name']),
                                           IsoDos = safe_str(itemcountry['IsoDos']),
                                           NameSpanish = safe_str(itemcountry['NameSpanish']),
                                           Capital = safe_str(itemcountry['Capital']),
                                           Continent = safe_str(itemcountry['Continent']),
                                           Indicativo = safe_str(itemcountry['Indicativo']),
                                           Ambulancias = safe_str(itemcountry['Ambulancias']),
                                           Bomberos = safe_str(itemcountry['Bomberos']),
                                           Policia = safe_str(itemcountry['Policia']),
                                           MDxkhab = safe_str(itemcountry['MDxkhab']),
                                           UsoHorario = safe_str(itemcountry['UsoHorario']),
                                           Divisa = safe_str(itemcountry['Divisa']),
                                           Simbolo = safe_str(itemcountry['Simbolo'])                                           
                                           )
        countries.append(country_instance)
    dynamo_db_instance.batch_insert_items_contact(countries)

    print("Finalizada carga Country")

    print("Lista Cities")

    for itemcitie in data[1]:
        cities_instance = LocationCities(partition_key = itemcitie['PK'],
                                          clasification_key = itemcitie['SK'],
                                          Name = safe_str(itemcitie['Name']),
                                          Ciudad = safe_str(itemcitie['Ciudad']),
                                          Country = safe_str(itemcitie['Country']),
                                          AdminName = safe_str(itemcitie['AdminName']),
                                          Place = safe_str(itemcitie['Place']),
                                          Latitude = safe_str(itemcitie['Latitude']),
                                          Longitude = safe_str(itemcitie['Longitude']),
                                          Population = safe_str(itemcitie['Population']),
                                          IdCity = safe_str(itemcitie['IdCity']),
                                          PopulationType = safe_str(itemcitie['PopulationType'])                                                                                   
                                           )
        cities.append(cities_instance)
    dynamo_db_instance.batch_insert_items_contact(cities)

    print("Finalizada carga Cities")
       

if __name__ == "__main__":
    excel_path = f'C:\\Users\\Milena\\Documents\\Data Providers\\Paises.xlsx'
    load_data_to_dynamodb(excel_path)