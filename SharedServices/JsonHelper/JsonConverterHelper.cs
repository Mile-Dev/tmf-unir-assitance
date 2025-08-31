using System.Text.Json;

namespace SharedServices.JsonHelper
{
    public static class JsonConverterHelper
    {
        public static string ToJson(Dictionary<string, object> dict)
        {
            var normalized = NormalizeJsonElements(dict);
            return JsonSerializer.Serialize(normalized);
        }

        public static Dictionary<string, object>? FromJson(string json)
        {
            return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        }

        public static Dictionary<string, object> NormalizeJsonElements(Dictionary<string, object> input)
        {
            var output = new Dictionary<string, object>();

            foreach (var kvp in input)
            {
                output[kvp.Key] = NormalizeUnknown(kvp.Value);
            }

            return output;
        }

        private static object NormalizeUnknown(object value)
        {
            return value switch
            {
                JsonElement je => NormalizeJsonElement(je),
                Dictionary<string, object> dict => NormalizeJsonElements(dict),
                List<object> list => list.Select(NormalizeUnknown).ToList(),
                _ => value
            };
        }

        private static object NormalizeJsonElement(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.Object =>
                    JsonSerializer.Deserialize<Dictionary<string, object>>(element.GetRawText())!
                        .ToDictionary(k => k.Key, k => NormalizeUnknown(k.Value)),

                JsonValueKind.Array =>
                    element.EnumerateArray().Select(NormalizeJsonElement).ToList(),

                JsonValueKind.String => element.GetString()!,
                JsonValueKind.Number => element.TryGetInt64(out var l) ? l : element.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                _ => null!
            };
        }
    }
}
