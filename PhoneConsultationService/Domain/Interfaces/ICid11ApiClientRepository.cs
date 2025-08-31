using PhoneConsultationService.Domain.Dto;

namespace PhoneConsultationService.Domain.Interfaces
{
    public interface ICid11ApiClientRepository
    {
        Task<List<Cie11Dto>> ReadCie11DtosFromJsonFileAsync();
    }
}
