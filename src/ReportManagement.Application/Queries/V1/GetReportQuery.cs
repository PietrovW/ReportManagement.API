using MediatR;
using ReportManagement.Application.Dtos.V1;

namespace ReportManagement.Application.Queries.V1
{
    public class GetReportQuery: IRequest<ReportDto>
    {
        public Guid Id { get; set; }
    }
}
