using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InfrastructureLayer.Database
{
    public class ChildGrowthDbContextFactory : IDesignTimeDbContextFactory<ChildGrowthDbContext>
    {
        //public ChildGrowthDbContext CreateDbContext(string[] args)
        //{
        //    var configuration = new ConfigurationBuilder()
        //      .AddJsonFile("appsettings.json")
        //      .Build();

        //    Console.WriteLine($"Using ConnectionString: {configuration.GetConnectionString("DefaultConnection")}");

        //    var optionsBuilder = new DbContextOptionsBuilder<ChildGrowthDbContext>();
        //    optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection") ?? "");

        //    return new ChildGrowthDbContext(optionsBuilder.Options);
        //}
        public ChildGrowthDbContext CreateDbContext(string[] args)
        {
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "ControllerLayer");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(configPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ChildGrowthDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);

            return new ChildGrowthDbContext(optionsBuilder.Options);
        }
    }
}
