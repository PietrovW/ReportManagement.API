namespace ReportManagement.API.Models
{
    public record Report
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
    }
}
