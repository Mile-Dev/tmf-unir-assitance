using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace SharedServices.CsvHelper
{
    public class CsvHelper
    {
        public static async Task<List<T>> ReadCsvFileAsync<T, TMap>(Stream stream, string delimiter)
            where T : class
            where TMap : ClassMap
        {
            try
            {
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = delimiter,
                    MissingFieldFound = null,
                    HeaderValidated = null,
                    TrimOptions = TrimOptions.Trim,
                });
                csv.Context.RegisterClassMap<TMap>();
                var records = new List<T>();
                await foreach (var record in csv.GetRecordsAsync<T>())  
                {
                    records.Add(record);
                }
                return records;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error processing CSV file", ex);
            }
        }

        public static async Task<IEnumerable<T>> ReadCsvFileAsync<T>(Stream stream, string delimiter)
        {
            try
            {
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = delimiter,
                    MissingFieldFound = null,
                    HeaderValidated = null,
                    TrimOptions = TrimOptions.Trim,
                });
                var records = new List<T>();
                await foreach (var record in csv.GetRecordsAsync<T>())
                {
                    records.Add(record);
                }
                return records;
            }
            catch (Exception ex)
            {
                // Log and handle exceptions appropriately
                throw new InvalidOperationException("Error processing CSV file", ex);
            }
        }
    }
}
