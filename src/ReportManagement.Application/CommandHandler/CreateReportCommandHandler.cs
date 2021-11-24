using AutoMapper;
using MediatR;
using ReportManagement.Application.Common;
using ReportManagement.Domain.Models;
using ReportManagement.Domain.Repositorys;

namespace ReportManagement.Application.CommandHandler
{
    public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, Guid>
    {
        private IMapper _mapper;
        private IReportRepository _reportRepository;
        public CreateReportCommandHandler(IMapper mapper, IReportRepository reportRepository)
        {
            _mapper = mapper;
            _reportRepository = reportRepository;
        }
        public Task<Guid> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            ReportModel reportModel = _mapper.Map<ReportModel>(request);
            Guid id= _reportRepository.Insert(reportModel);
            return Task.FromResult(id);
        }
    }
}
