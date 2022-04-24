using AutoMapper;
using ReportManagement.Application.Common.V1;
using ReportManagement.Application.Dtos.V1;
using ReportManagement.Domain.Models;

namespace ReportManagement.Application.AutoMapper
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<ReportDto, ReportModel>()
                .ConstructUsing(i => new ReportModel())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<ReportModel, ReportDto > ()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<ReportModel, CreateReportCommand>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<CreateReportCommand, ReportModel>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<ReportModel, ReportDto>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
