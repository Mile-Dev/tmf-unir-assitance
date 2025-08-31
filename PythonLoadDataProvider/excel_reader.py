import pandas as pd

class ExcelReader:
    def __init__(self, file_path):
        self.file_path = file_path
        self.xls = pd.ExcelFile(file_path)

    def get_last_sheets(self, number_of_sheets=3):
        """Extrae las ultimas 'number_of_sheets' hojas del archivo Excel como DataFrames."""
        sheet_names = self.xls.sheet_names
        last_sheets = sheet_names[-number_of_sheets:]
        return [self.xls.parse(sheet_name) for sheet_name in last_sheets]

    def sheets_to_dict(self, sheets):
        """Convierte DataFrames a listas de diccionarios."""
        return [sheet.to_dict('records') for sheet in sheets]