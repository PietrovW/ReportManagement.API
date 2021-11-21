using ReportManagement.Domain.Models;
using ReportManagement.Domain.Repositorys;

namespace ReportManagement.Infrastructure.Repositorys
{
    public class ReportRepository: BaseRepository<ReportModel>, IReportRepository
    {
    }
}
