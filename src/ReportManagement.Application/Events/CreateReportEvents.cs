using MediatR;

namespace ReportManagement.Application.Events
{
    public class CreateReportEvents: INotification
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
         
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
