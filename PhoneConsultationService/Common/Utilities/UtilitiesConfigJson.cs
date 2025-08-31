using System.Text.Json;

namespace PhoneConsultationService.Common.Utilities
{
    public static class UtilitiesConfigJson
    {
        private static readonly JsonSerializerOptions s_writeOptions = new()
        {
            WriteIndented = true
        };

        private static readonly JsonSerializerOptions s_readOptions = new()
        {
            PropertyNamingPolicy= JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public static string Serialize<T>(T value)
        {
            return JsonSerializer.Serialize(value, s_writeOptions);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, s_readOptions);
        }
    }
}
