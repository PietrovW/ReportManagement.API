using MediatR;

namespace ReportManagement.Application.Common
{
    public class CreateReportCommand : IRequest<Guid>
    {
        public string Name { get; set; } = String.Empty;
    }
}
