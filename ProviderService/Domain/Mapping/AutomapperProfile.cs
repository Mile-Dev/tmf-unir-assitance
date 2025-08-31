using AutoMapper;
using ProviderService.Domain.Dto.Provider;
using ProviderService.Domain.Dto.Provider.Created;
using ProviderService.Domain.Dto.Provider.Query;
using ProviderService.Domain.Dto.ProviderAgreement.Created;
using ProviderService.Domain.Dto.ProviderAgreement.Query;
using ProviderService.Domain.Dto.ProviderContact.Query;
using ProviderService.Domain.Dto.ProviderLocation.Created;
using ProviderService.Domain.Dto.ProviderPaymentMethod;
using ProviderService.Domain.Dto.ProviderPaymentMethod.Query;
using ProviderService.Domain.Entities;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<Provider, ProviderIdDto>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdProvider));

        CreateMap<Provider, ProviderGetDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdProvider));

        CreateMap<Provider, ProviderLocationGetDto>();

        CreateMap<Provider, ProviderAgreementDto>();

        CreateMap<Provider, ProviderContactDto>();

        CreateMap<Provider, ProviderPaymentMethodDto>();

        CreateMap<Provider, ProviderSearchGetDto>();

        #region Location

        CreateMap<ProviderLocation, ProviderLocationIdDto>();

        CreateMap<ProviderLocation, ProviderLocationGetDto>();

        #endregion

        #region PaymentMethod
        CreateMap<ProviderPaymentMethod, ProviderPaymentMethodGetDto>();

        CreateMap<ListData, ListDataDto>();
        #endregion

        #region ProviderContact

        CreateMap<ProviderContact, ProviderContactGetDto>();

        CreateMap<ListData, ListDataDto>();
        #endregion
      
        #region ProviderContact

        CreateMap<ProviderAgreement, ProviderAgreementIdDto>();

        CreateMap<ProviderAgreement, ProviderAgreementGetDto>();
        #endregion

    }
}


