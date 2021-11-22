using AutoMapper;
using MediatR;
using ReportManagement.Application.Dtos;
using ReportManagement.Application.Queries;
using ReportManagement.Domain.Models;
using ReportManagement.Domain.Repositorys;

namespace ReportManagement.Application.QuerieHandler
{
    public class GetReportQueryHandler : IRequestHandler<GetReportQuery, ReportDto>
    {
        private IMapper _mapper;
        private IReportRepository _reportRepository;
        public GetReportQueryHandler(IMapper mapper, IReportRepository reportRepository)
        {
            _mapper = mapper;
            _reportRepository = reportRepository;
        }
        public Task<ReportDto> Handle(GetReportQuery request, CancellationToken cancellationToken)
        {
            ReportModel reportModels = _reportRepository.GetById(request.Id);
            return Task.FromResult(_mapper.Map<ReportDto>(reportModels));
        }
    }
}
