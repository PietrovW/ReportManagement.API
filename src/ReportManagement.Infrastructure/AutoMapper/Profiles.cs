using AutoMapper;
using ReportManagement.Application.Dtos;
using ReportManagement.Domain.Models;

namespace ReportManagement.Infrastructure.AutoMapper
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<ReportDto, ReportModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
        }
    }
}
