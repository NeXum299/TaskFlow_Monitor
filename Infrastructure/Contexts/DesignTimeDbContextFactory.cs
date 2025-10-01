using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow_Monitor.Infrastructure.Contexts
{
    public class DesignTimeDbContextFactory: IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<MyDbContext>();
            var connectionString = configuration.GetConnectionString("MyDbContext");

            builder.UseNpgsql(connectionString);

            return new MyDbContext(builder.Options);
        }
    }
}
