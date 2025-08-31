using AutoMapper;
using MasterRdsServices.Domain.Dto.Icd.Query;
using MasterRdsServices.Domain.Entities;
using MasterRdsServices.Domain.Mapping;
using StorageS3Services.Common.Interfaces;
using CsvHelperCommon = SharedServices.CsvHelper;

namespace MasterRdsServices.Services
{
    public class IcdServices(IS3Service repository, ILogger<IcdServices> logger, 
                                              IMapper mapper, IConfiguration configuration) : IIcdServices
    {
        private readonly IS3Service _repository = repository;
        private readonly ILogger<IcdServices> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly string _bucketName = configuration["AWS:S3:BucketNameIcd"]
                ?? throw new ArgumentNullException("BucketName is not configured");
        private readonly string _directorioprovider = configuration["AWS:S3:DirectorioIcd"]
                ?? throw new ArgumentNullException("BucketName is not configured");
        private readonly string filenametypeprovider = configuration["AWS:S3:FilenameIcd"]
                ?? throw new ArgumentNullException("BucketName is not configured");

        public async Task<List<IcdDto>> GetRecords(string? name)
        {
            _logger.LogInformation("Get records of Icd10");
            var records = await _repository.GetFileAsStreamAsync(_bucketName, _directorioprovider, filenametypeprovider);
            var listRecordsIcd = await CsvHelperCommon.CsvHelper.ReadCsvFileAsync<RecordsIcd, IcdMap>(records, ";");
            IEnumerable<RecordsIcd> filteredRecords;
            if (!string.IsNullOrWhiteSpace(name))
            {
                filteredRecords = listRecordsIcd.Where(item => item.Id.Contains(name, StringComparison.OrdinalIgnoreCase)
                                                            || item.DescriptionShort.Contains(name, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                filteredRecords = listRecordsIcd.Take(51);
            }
            var result = _mapper.Map<List<IcdDto>>(filteredRecords.ToList());
            return result;
        }
    }
}
