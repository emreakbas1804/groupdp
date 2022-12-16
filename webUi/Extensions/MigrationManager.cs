using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static webUi.Program;

namespace webUi.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope=host.Services.CreateScope( ))
            {
                 using (var GroupDbContext= scope.ServiceProvider.GetRequiredService<GroupDbContext>())
                 {
                      try
                      {
                        GroupDbContext.Database.Migrate();
                    }
                      catch (System.Exception)
                      {
                          
                          throw;
                      }
                 }
            }
            return host;
        }
    }
}