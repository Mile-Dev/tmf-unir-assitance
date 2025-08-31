using PhoneConsultationService.Common.Utilities;
using PhoneConsultationService.Domain.Dto;
using PhoneConsultationService.Domain.Interfaces;
using System.Text.Json;

namespace PhoneConsultationService.Infraestructure.DataCie11
{
    public class Cid11ApiClientRepository : ICid11ApiClientRepository
    {
        public async Task<List<Cie11Dto>> ReadCie11DtosFromJsonFileAsync()
        {
            string filePath = "./cid11.json";
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file does not exist.");
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Utilizar CamelCase para las propiedades C# (PascalCase)
                    PropertyNameCaseInsensitive = true, // Hacer que la comparación sea insensible a mayúsculas y minúsculas
                };

                string jsonString = await File.ReadAllTextAsync(filePath);
                var Cie11Dtos = UtilitiesConfigJson.Deserialize<List<Cie11Dto>>(jsonString);
                return Cie11Dtos ?? [];
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                throw new Exception("An error occurred while reading the JSON file.", ex);
            }
        }


    
    }
}
