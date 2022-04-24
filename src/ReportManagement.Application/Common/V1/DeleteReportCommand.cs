using MediatR;

namespace ReportManagement.Application.Common.V1
{
    public class DeleteReportCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
