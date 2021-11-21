using Microsoft.EntityFrameworkCore;

namespace ReportManagement.Infrastructure.Data
{
    internal class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
