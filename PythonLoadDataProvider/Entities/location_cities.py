from dataclasses import dataclass, field

@dataclass
class LocationCities:
    partition_key: str  # Equivalente a PartitionKey (PK)
    clasification_key: str  # Equivalente a ClasificationKey (SK)
    Name: str = field(default_factory=str)  # Equivalente a Name
    Ciudad: str = field(default_factory=str)  # Equivalente a Ciudad
    Country: str = field(default_factory=str)  # Equivalente a Country
    AdminName: str = field(default_factory=str)    # Equivalente a AdminName
    Place: str = field(default_factory=str)  # Equivalente a Place
    Latitude: str = field(default_factory=str)  # Equivalente a Latitude
    Longitude: str = field(default_factory=str)  # Equivalente a Longitude
    Population: str = field(default_factory=str)  # Equivalente a Population
    IdCity: str = field(default_factory=str)  # Equivalente a IdCity
    PopulationType: str = field(default_factory=str)    # Equivalente a PopulationType

    def to_dynamo_dict(self):
        """
        Convierte la clase a un diccionario con los nombres de campos correctos para DynamoDB.
        Omite los campos que estan vacios o None.
        """
        data = {
            'PK': self.partition_key,
            'SK': self.clasification_key,
            'Name': self.Name,
            'Ciudad': self.Ciudad,
            'Country': self.Country,
            'AdminName': self.AdminName,
            'Place': self.Place,
            'Latitude': self.Latitude,
            'Longitude': self.Longitude,
            'Population': self.Population,
            'IdCity': self.IdCity,
            'PopulationType': self.PopulationType,           
            }
        return{k: v for k, v in data.items() if v not in (None, '', 'None') }
