using Microsoft.EntityFrameworkCore;
using ReportManagement.Infrastructure.Data;

namespace ReportManagement.API.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                   
                        appContext.Database.EnsureCreated();
                        appContext.Database.Migrate();
                }
            }
            return host;
        }
    }
}
