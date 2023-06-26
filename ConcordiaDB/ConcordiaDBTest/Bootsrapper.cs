namespace ConcordiaDBTest;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using ConcordiaDBLibrary.Data;

public static class Bootstrapper
{
    public static async Task MigrateAsync(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ConcordiaContext>();
        await dbContext.Database.MigrateAsync();
    }
}