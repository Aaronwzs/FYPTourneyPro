using Volo.Abp.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace FYPTourneyPro.Data;

public class FYPTourneyProDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public FYPTourneyProDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        
        /* We intentionally resolving the FYPTourneyProDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<FYPTourneyProDbContext>()
            .Database
            .MigrateAsync();

    }
}
