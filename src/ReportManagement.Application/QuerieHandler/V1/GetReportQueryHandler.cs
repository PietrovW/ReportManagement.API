using AutoMapper;
using MediatR;
using ReportManagement.Application.Dtos.V1;
using ReportManagement.Application.Queries.V1;
using ReportManagement.Domain.Models;
using ReportManagement.Domain.Repositorys;

namespace ReportManagement.Application.QuerieHandler.V1
{
    public class GetReportQueryHandler : IRequestHandler<GetReportQuery, ReportDto>
    {
        private IMapper _mapper;
        private IReadReportRepository _reportRepository;
        public GetReportQueryHandler(IMapper mapper, IReadReportRepository reportRepository)
        {
            _mapper = mapper;
            _reportRepository = reportRepository;
        }
        public async Task<ReportDto> Handle(GetReportQuery request, CancellationToken cancellationToken)
        {
            ReportModel reportModels = await _reportRepository.GetByIdAsync(request.Id);
            return _mapper.Map<ReportDto>(reportModels);
        }
    }
}
