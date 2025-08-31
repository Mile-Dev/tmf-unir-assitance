from dataclasses import dataclass, field

@dataclass
class LocationCountry:
    partition_key: str  # Equivalente a PartitionKey (PK)
    clasification_key: str  # Equivalente a ClasificationKey (SK)
    Name: str = field(default_factory=str)  # Equivalente a Name
    IsoDos: str = field(default_factory=str)  # Equivalente a IsoDos
    NameSpanish: str = field(default_factory=str)  # Equivalente a NameSpanish
    Capital: str = field(default_factory=str)    # Equivalente a Capital
    Continent: str = field(default_factory=str)  # Equivalente a Continent
    Indicativo: str = field(default_factory=str)  # Equivalente a Indicativo
    Ambulancias: str = field(default_factory=str)  # Equivalente a Ambulancias
    Bomberos: str = field(default_factory=str)  # Equivalente a Bomberos
    Policia: str = field(default_factory=str)  # Equivalente a Policia
    MDxkhab: str = field(default_factory=str)    # Equivalente a MDxkhab
    UsoHorario: str = field(default_factory=str)   # Equivalente a UsoHorario
    Divisa: str = field(default_factory=str)    # Equivalente a Divisa
    Simbolo: str = field(default_factory=str)  # Equivalente a Simbolo

    def to_dynamo_dict(self):
        """
        Convierte la clase a un diccionario con los nombres de campos correctos para DynamoDB.
        Omite los campos que estan vacios o None.
        """
        data = {
            'PK': self.partition_key,
            'SK': self.clasification_key,
            'Name': self.Name,
            'IsoDos': self.IsoDos,
            'NameSpanish': self.NameSpanish,
            'Capital': self.Capital,
            'Continent': self.Continent,
            'Indicativo': self.Indicativo,
            'Ambulancias': self.Ambulancias,
            'Bomberos': self.Bomberos,
            'Policia': self.Policia,
            'MDxkhab': self.MDxkhab,
            'UsoHorario': self.UsoHorario,
            'Divisa': self.Divisa,
            'Simbolo': self.Simbolo,
            }
        return{k: v for k, v in data.items() if v not in (None, '', 'None') }
