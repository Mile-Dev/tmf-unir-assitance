using System.Text.Json;

namespace MasterRdsServices.Common
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Serializar Objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objJsonConvert"></param>
        /// <returns></returns>
        public static T ToJsonDeserialize<T>(this string objJsonConvert, JsonSerializerOptions jsonOptions) => JsonSerializer.Deserialize<T>(objJsonConvert, jsonOptions);

        /// <summary>
        /// Serializar Objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objJsonConvert"></param>
        /// <returns></returns>
        public static string ToJsonSerialize<T>(this T objJsonConvert, JsonSerializerOptions jsonOptions) => JsonSerializer.Serialize(objJsonConvert, jsonOptions);

    }
}
