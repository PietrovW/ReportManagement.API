using AutoMapper;
using MediatR;
using ReportManagement.Application.Dtos;
using ReportManagement.Application.Queries;
using ReportManagement.Domain.Models;
using ReportManagement.Domain.Repositorys;

namespace ReportManagement.Application.QuerieHandler
{
    public class GetReportListQueryHandler : IRequestHandler<GetReportListQuery, List<ReportDto>>
    {
        private IMapper _mapper;
        private IReportRepository _reportRepository;
        public GetReportListQueryHandler(IMapper mapper, IReportRepository reportRepository)
        {
            _mapper = mapper;
            _reportRepository = reportRepository;
        }
        public Task<List<ReportDto>> Handle(GetReportListQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<ReportModel> reportModels= _reportRepository.List(x=>x.Name== request.Name);
            return Task.FromResult(_mapper.Map<List<ReportDto>>(reportModels));
        }
    }
}
