using PhoneConsultationService.Domain.Dto;
using PhoneConsultationService.Domain.Interfaces;

namespace PhoneConsultationService.Services;

    public class Cid11ApiClientServices(ICid11ApiClientRepository repository, ILogger<Cid11ApiClientServices> logger)
{
    private readonly ICid11ApiClientRepository _repository = repository;
    private readonly ILogger<Cid11ApiClientServices> _logger = logger;

    public async Task<List<Cie11Dto>> ReadCie11DtosFromJsonFileAsync()
    {
        _logger.LogInformation("Connection Api Cie-11");
        return await _repository.ReadCie11DtosFromJsonFileAsync();
    }

}

