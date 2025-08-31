from dataclasses import dataclass, field
from typing import Optional

@dataclass
class Provider:
    partition_key: str  # Equivalente a PartitionKey (PK)
    clasification_key: str  # Equivalente a ClasificationKey (SK)
    id_provider: str  # Equivalente a IdProvider
    name: str = field(default_factory=str)  # Equivalente a NameProvider
    score: Optional[int] = None  # Equivalente a Score, puede ser None (int?)
    priorityLevel: Optional[int] = None  # Equivalente a Score, puede ser None (int?)
    typePro: str = field(default_factory=str)  # Equivalente a TypeProvider
    details: str = field(default_factory=str)  # Equivalente a Details
    tags: str = field(default_factory=str)  # Equivalente a Details
   # id_fiscal: Optional[str] = None  # Equivalente a IdFiscalProvider
   # nit: Optional[str] = None  # Equivalente a Nit

    def to_dynamo_dict(self):
        """
        Convierte la clase a un diccionario con los nombres de campos correctos para DynamoDB.
        Omite los campos que estan vacios o None.
        """
        data = {
            'PK': self.partition_key,
            'SK': self.clasification_key,
            'idProvider': self.id_provider,
            'nameProvider': self.name,
            'score': self.score,
            'typeProvider': self.typePro,
            'priorityLevel': self.priorityLevel,
            'details': self.details,
            'tags': self.tags,
           # 'idFiscalProvider': self.id_fiscal,
           # 'nit': self.nit,
        }
        return {k: v for k, v in data.items() if v not in (None, '', 'None') }
