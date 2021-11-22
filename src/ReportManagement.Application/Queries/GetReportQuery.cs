using MediatR;
using ReportManagement.Application.Dtos;

namespace ReportManagement.Application.Queries
{
    public class GetReportQuery: IRequest<ReportDto>
    {
        public Guid Id { get; set; }
    }
}
