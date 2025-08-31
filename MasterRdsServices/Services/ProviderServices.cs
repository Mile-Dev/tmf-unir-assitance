using AutoMapper;
using MasterRdsServices.Domain.Dto.Provider.Query;
using MasterRdsServices.Domain.Entities;
using CsvHelperCommon = SharedServices.CsvHelper;
using StorageS3Services.Common.Interfaces;

namespace MasterRdsServices.Services
{
    public class ProviderServices(IS3Service repository, ILogger<ProviderServices> logger, IMapper mapper, IConfiguration configuration) : IProviderServices
    {
        private readonly IS3Service _repository = repository;
        private readonly ILogger<ProviderServices> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly string _bucketName = configuration["AWS:S3:BucketNameTypeProvider"]
                ?? throw new ArgumentNullException("BucketName is not configured");
        private readonly string _directorioprovider= configuration["AWS:S3:DirectorioTypeProvider"]
                ?? throw new ArgumentNullException("BucketName is not configured");
        private readonly string filenametypeprovider = configuration["AWS:S3:FilenameTypeProvider"]
                ?? throw new ArgumentNullException("BucketName is not configured");

        public async Task<List<ProviderTypeGetDto>> GetRecords()
        {
            _logger.LogInformation("Provider type filter information retrieval");
            var records = await _repository.GetFileAsStreamAsync(_bucketName, _directorioprovider, filenametypeprovider);
            var listRecords = await CsvHelperCommon.CsvHelper.ReadCsvFileAsync<ProviderType>(records, ";");
            var result = _mapper.Map<List<ProviderTypeGetDto>>(listRecords);
            return result;
        }
    }
}
