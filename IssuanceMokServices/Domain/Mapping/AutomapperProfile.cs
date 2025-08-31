using AutoMapper;
using IssuanceMokServices.Domain.Dto;
using IssuanceMokServices.Domain.Entities;
using SharedServices.JsonHelper;

namespace IssuanceMokServices.Domain.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {

            CreateMap<UploadEntities, UploadResponse>()
                .ReverseMap();

            CreateMap<UploadEntities, UploadRequest>()
                .ReverseMap();

            CreateMap<UploadEntities, UploadResponseQuery>()
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => JsonConverterHelper.FromJson(src.Metadata) ?? new Dictionary<string, object>()))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PartitionKey))
                .ReverseMap();

        }
    }
}
