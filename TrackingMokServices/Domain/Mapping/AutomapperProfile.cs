using AutoMapper;
using TrackingMokServices.Domain.Dto;
using TrackingMokServices.Domain.Entities;

namespace TrackingMokServices.Domain.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<ResponseEventMok, EventMok>()
                .ReverseMap();

        }
    }
}
