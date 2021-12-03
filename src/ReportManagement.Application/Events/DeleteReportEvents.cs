using MediatR;

namespace ReportManagement.Application.Events
{
    public class DeleteReportEvents : INotification
    {
        public Guid Id { get; set; }
        public string Description { get; set; }=String.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
