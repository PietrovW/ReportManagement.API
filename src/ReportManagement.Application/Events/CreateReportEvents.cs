using MediatR;

namespace ReportManagement.Application.Events
{
    public class CreateReportEvents: INotification
    {
        public Guid Id { get; set; }
        public string Name { get; set; } =String.Empty;
        public string Description { get; set; } = String.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
