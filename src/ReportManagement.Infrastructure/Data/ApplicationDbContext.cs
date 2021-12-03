using Microsoft.EntityFrameworkCore;
using ReportManagement.Domain.Models;
using System.Reflection;

namespace ReportManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }
        protected ApplicationDbContext()
            :base()
        {
        }
        public DbSet<ReportModel> Reports { get; set; } = null!;
       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
