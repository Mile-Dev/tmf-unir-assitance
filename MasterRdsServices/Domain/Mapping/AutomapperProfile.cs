using AutoMapper;
using MasterRdsServices.Domain.Dto;
using MasterRdsServices.Domain.Dto.Icd.Query;
using MasterRdsServices.Domain.Dto.Location.Query;
using MasterRdsServices.Domain.Dto.Provider.Query;
using MasterRdsServices.Domain.Entities;
using MasterRdsServices.Domain.Entities.Dynamodb;

namespace MasterRdsServices.Domain.Mapping;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<Category, CategoriesQueryDto>()
         .ReverseMap();

        CreateMap<GeneralType, GeneralTypesQueryDto>()
            .ForMember(dest => dest.CategoriesName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
         .ReverseMap();

        CreateMap<EventStatus, EventStatusQueryDto>()
         .ReverseMap();

        CreateMap<VoucherStatus, VoucherStatusQueryDto>()
        .ReverseMap();

        CreateMap<EventProviderStatus, EventProviderStatusQueryDto>()
        .ReverseMap();

        CreateMap<ProviderType, ProviderTypeGetDto>()
        .ReverseMap();

        CreateMap<RecordsIcd, IcdDto>()
        .ReverseMap();

        CreateMap<City, CityDto>()
         .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ClasificationKey))
         .ForMember(dest => dest.IdCountry, opt => opt.MapFrom(src => src.PartitionKey))
         .ReverseMap();

        CreateMap<Country, CountryDto>()
         .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ClasificationKey))
         .ForMember(dest => dest.Iso2, opt => opt.MapFrom(src => src.IsoDos))
         .ReverseMap();
    }
}
