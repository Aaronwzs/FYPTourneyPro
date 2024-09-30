using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FYPTourneyPro.Data;

public class FYPTourneyProDbContextFactory : IDesignTimeDbContextFactory<FYPTourneyProDbContext>
{
    public FYPTourneyProDbContext CreateDbContext(string[] args)
    {
        FYPTourneyProEfCoreEntityExtensionMappings.Configure();
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<FYPTourneyProDbContext>()
            .UseNpgsql(configuration.GetConnectionString("Default"));

        return new FYPTourneyProDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}