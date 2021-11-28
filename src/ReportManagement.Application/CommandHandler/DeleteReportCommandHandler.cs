using AutoMapper;
using MediatR;
using ReportManagement.Application.Common;
using ReportManagement.Application.Events;
using ReportManagement.Domain.Models;
using ReportManagement.Domain.Repositorys;

namespace ReportManagement.Application.CommandHandler
{
    public class DeleteReportCommandHandler : IRequestHandler<DeleteReportCommand>
    {
        private readonly IMapper _mapper;
        private readonly IWriteReportRepository _reportRepository;
        private readonly IReadReportRepository _readReportRepository;
        private readonly IMediator _mediator;
        public DeleteReportCommandHandler(IMapper mapper, 
            IWriteReportRepository reportRepository,
            IReadReportRepository readReportRepository,
            IMediator mediator)
        {
            _mapper = mapper;
            _reportRepository = reportRepository;
            _readReportRepository = readReportRepository;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
        {
            // ReportModel reportModel = _mapper.Map<ReportModel>(request);
            ReportModel reportModel = await _readReportRepository.GetByIdAsync(request.Id);
           // ReportModel reportModel = _mapper.Map<ReportModel>(request);
            _reportRepository.Delete(reportModel);
           await _mediator.Publish(new DeleteReportEvents() { Id = reportModel.Id, Description = typeof(DeleteReportCommand).Name });

            return Unit.Value;
        }
    }
}
