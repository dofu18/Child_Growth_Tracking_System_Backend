using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace InfrastructureLayer.Database
{
    public static class DbInitializer
    {
        public static IApplicationBuilder UseInitializeDatabase(this IApplicationBuilder application)
        {
            using var serviceScope = application.ApplicationServices.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetService<ChildGrowthDbContext>();

            // only call this method when there are pending migrations
            if (dbContext != null && dbContext.Database.GetPendingMigrations().Any())
            {
                Console.WriteLine("Applying  Migrations...");
                dbContext.Database.Migrate();
            }

            return application;
        }
    }
}
