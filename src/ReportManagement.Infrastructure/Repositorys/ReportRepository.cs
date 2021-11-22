using ReportManagement.Domain.Models;
using ReportManagement.Domain.Repositorys;
using ReportManagement.Infrastructure.Data;

namespace ReportManagement.Infrastructure.Repositorys
{
    public class ReportRepository: BaseRepository<ReportModel>, IReportRepository
    {
        public ReportRepository(ApplicationDbContext dbContext) :base(dbContext)
        {

        }
    }
}
