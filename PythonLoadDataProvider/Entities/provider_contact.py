from dataclasses import dataclass, field
from typing import List, Optional

@dataclass
class ListData:
    key: str = field(default_factory=str)
    value: str = field(default_factory=str)

@dataclass
class ProviderContact:
    partition_key: str  # Equivalente a PartitionKey (PK)
    clasification_key: str  # Equivalente a ClasificationKey (SK)
    id_provider: str = field(default_factory=str)  # Equivalente a IdProvider
    id_contact: str = field(default_factory=str)  # Equivalente a IdContact
    list_data: List[ListData] = field(default_factory=list)  # Lista de ListData
    details: str = field(default_factory=str)  # Equivalente a Details
    created_at: str = field(default_factory=str)  # Equivalente a CreatedAt
    updated_at: str = field(default_factory=str)  # Equivalente a UpdatedAt

    def to_dynamo_dict(self):
        """
        Convierte la clase a un diccionario con los nombres de campos correctos para DynamoDB.
        Omite los campos que estan vacios o None.
        """
        data = {
            'PK': self.partition_key,
            'SK': self.clasification_key,
            'idProvider': self.id_provider,
            'idContact': self.id_contact,
            'listData': [{'key': item.key, 'value': item.value} for item in self.list_data],
            'details': self.details,
            'createdAt': self.created_at,
            'updatedAt': self.updated_at,
            }
        return{k: v for k, v in data.items() if v not in (None, '', 'None') }
