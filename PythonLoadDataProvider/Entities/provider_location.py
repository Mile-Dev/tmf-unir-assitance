from dataclasses import dataclass, field
from typing import Optional

@dataclass
class ProviderLocation:
    partition_key: str  # Equivalente a PartitionKey (PK)
    clasification_key: str  # Equivalente a ClasificationKey (SK)
    id_provider: str = field(default_factory=str)  # Equivalente a IdProvider
    id_location: str = field(default_factory=str)  # Equivalente a IdLocation
    name: str = field(default_factory=str)  # Equivalente a NameProvider
    typePro: str = field(default_factory=str)  # Equivalente a TypeProvider
    id_country: str = field(default_factory=str)  # Equivalente a IdProvider
    country: str = field(default_factory=str)  # Equivalente a Country
    id_city: str = field(default_factory=str)  # Equivalente a IdProvider
    city: str = field(default_factory=str)  # Equivalente a City
    address: str = field(default_factory=str)  # Equivalente a Address
    longitude: str = field(default_factory=str)  # Equivalente a Longitude
    latitude: str = field(default_factory=str)  # Equivalente a Latitude
    details: str = field(default_factory=str)  # Equivalente a Details
    score: Optional[int] = None  # Equivalente a Score
    geohash: str = field(default_factory=str)  # Equivalente a 
    geohashPK: str = field(default_factory=str)  # Equivalente a 
    created_at: str = field(default_factory=str)  # Equivalente a CreatedAt
    updated_at: str = field(default_factory=str)  # Equivalente a UpdatedAt

    def to_dynamo_dict(self):
        """
        Convierte los nombres de los campos a los que corresponden en DynamoDB usando el mapeo.
        Omite los campos que estan vacios o None.
        """
           # Crear un diccionario de todos los campos
        data = {
            'PK': self.partition_key,
            'SK': self.clasification_key,
            'idProvider': self.id_provider,
            'idLocation': self.id_location,
            'nameProvider': self.name,
            'typeProvider': self.typePro,
            'idCountry': self.id_country,            
            'country': self.country,
            'idCity': self.id_city,
            'city': self.city,
            'address': self.address,
            'longitude': self.longitude,
            'latitude': self.latitude,
            'details': self.details,
            'score': self.score,
            'geohash': self.geohash,
            'geohashPK': self.geohashPK,
            'createdAt': self.created_at,
            'updatedAt': self.updated_at,
        }
        return {k: v for k, v in data.items() if v not in (None, '', 'None') }
