using MediatR;

namespace ReportManagement.Application.Common
{
    public class DeleteReportCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
