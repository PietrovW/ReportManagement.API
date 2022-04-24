using AutoMapper;
using MediatR;
using ReportManagement.Application.Common.V1;
using ReportManagement.Application.Events;
using ReportManagement.Application.Request.V1;
using ReportManagement.Domain.Models;
using ReportManagement.Domain.Repositorys;

namespace ReportManagement.Application.CommandHandler.V1
{
    public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, CreateReportRequest>
    {
        private readonly IMapper _mapper;
        private readonly IWriteReportRepository _reportRepository;
        private readonly IMediator _mediator;
        public CreateReportCommandHandler(IMapper mapper, IWriteReportRepository reportRepository, IMediator mediator)
        {
            _mapper = mapper;
            _reportRepository = reportRepository;
            _mediator = mediator;
        }
        public async Task<CreateReportRequest> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            ReportModel reportModel = _mapper.Map<ReportModel>(request);
            Guid id= _reportRepository.Insert(reportModel);
           await _mediator.Publish(new CreateReportEvents() { Id = id, Name = request.Name , Description= typeof(CreateReportCommand).Name });
            
            return new CreateReportRequest() { Name = request.Name };
        }
    }
}
