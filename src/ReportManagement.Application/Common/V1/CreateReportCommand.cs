using MediatR;
using ReportManagement.Application.Request.V1;

namespace ReportManagement.Application.Common.V1
{
    public class CreateReportCommand : IRequest<CreateReportRequest>
    {
        public string Name { get; set; } = String.Empty;
    }
}
