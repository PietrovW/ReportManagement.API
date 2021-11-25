using AutoMapper;
using MediatR;
using ReportManagement.Application.Common;
using ReportManagement.Application.Events;
using ReportManagement.Domain.Models;
using ReportManagement.Domain.Repositorys;

namespace ReportManagement.Application.CommandHandler
{
    public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, Guid>
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
        public Task<Guid> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            ReportModel reportModel = _mapper.Map<ReportModel>(request);
            Guid id= _reportRepository.Insert(reportModel);
            _mediator.Publish(new CreateReportEvents() { Id = id, Name = request.Name , Description= typeof(CreateReportCommand).Name });
            
            return Task.FromResult(id);
        }
    }
}
