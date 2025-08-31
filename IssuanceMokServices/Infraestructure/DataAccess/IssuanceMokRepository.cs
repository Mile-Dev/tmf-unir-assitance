using Amazon.DynamoDBv2.DataModel;
using IssuanceMokServices.Domain.Entities;
using IssuanceMokServices.Domain.Interfaces;
using Microsoft.CodeAnalysis;

namespace IssuanceMokServices.Infraestructure.DataAccess
{
    public class IssuanceMokRepository(IDynamoDBContext context, ILogger<IssuanceMokRepository> logger) : IIssuanceMokRepository
    {
        private readonly IDynamoDBContext _context = context;

        private readonly ILogger<IssuanceMokRepository> _logger = logger;

        public async Task<UploadEntities> GetByIdAsync(string id)
        {
            UploadEntities uploadEntities;
            try
            {
                uploadEntities = await _context.LoadAsync<UploadEntities>(id);
                _logger.LogInformation("Issuance Mok {} is get", uploadEntities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail to persist to DynamoDb Table IssuanceMok in method GetByIdAsync");
                throw;
            }

            return uploadEntities;
        }

        public async Task<UploadEntities> SaveMetadataUploadAsync(UploadEntities metadata)
        {
            try
            {
                await _context.SaveAsync(metadata);
                _logger.LogInformation("Issuance Mok {} is added", metadata);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail to persist to DynamoDb Table IssuanceMok in method SaveMetadataUploadAsync");
                throw;
            }

            return metadata;
        }
    }
}
