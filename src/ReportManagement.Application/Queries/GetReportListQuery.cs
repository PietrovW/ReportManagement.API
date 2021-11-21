using MediatR;
using ReportManagement.Application.Dtos;

namespace ReportManagement.Application.Queries
{
    public class GetReportListQuery: IRequest<List<ReportDto>>
    {
        public string? Name { get; set; }
    }
}
