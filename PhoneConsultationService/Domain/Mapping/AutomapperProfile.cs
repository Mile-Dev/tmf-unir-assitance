using AutoMapper;
using PhoneConsultationService.Common.Constans;
using PhoneConsultationService.Domain.Dto;
using PhoneConsultationService.Domain.Entities;
using PhoneConsultationService.Domain.Enum;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        //Source to destination.
        CreateMap<PhoneConsultationDto, PhoneConsultation>()
            .ForMember(dest => dest.HistoryClinica,
                                opt => opt.MapFrom(src => $"{src.Name.ToUpper()}{src.LastName.ToUpper()}{src.DateBirth:yyyyMMdd}".Trim()))
            .ForMember(dest => dest.PartitionKey, opt => opt.MapFrom(src => src.IdEvent))
            .ForMember(dest => dest.ClasificationKey, opt => opt.MapFrom(src => $"{Constans.PhoneConsultationStartWith}{src.PhoneRecordId}"))
            .ForMember(dest => dest.DateBirth, opt => opt.MapFrom(src => src.DateBirth))   
        .ReverseMap();

        CreateMap<AttachmentDto, Attachment>()
        .ReverseMap();

        CreateMap<PhoneConsultation, PhoneConsultationGetDto>()
         .ForMember(dest => dest.IdEvent, opt => opt.MapFrom(src => src.PartitionKey));

        CreateMap<AssignTriage, string>().ConvertUsing(src => src.GetDescription());
        CreateMap<string, AssignTriage>().ConvertUsing(src => Enum.Parse<AssignTriage>(src));

        CreateMap<DecisionType, string>().ConvertUsing(src => src.ToString());
        CreateMap<string, DecisionType>().ConvertUsing(src => Enum.Parse<DecisionType>(src));
    }
}