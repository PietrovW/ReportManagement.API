namespace ReportManagement.API.Models.V1
{
    public record Report
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
    }
}
