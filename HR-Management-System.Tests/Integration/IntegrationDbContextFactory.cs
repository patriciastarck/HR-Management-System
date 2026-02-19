using HR_Management_System.Models;
using Microsoft.EntityFrameworkCore;

namespace HR_Management_System.Tests.Integration;

public static class IntegrationDbContextFactory
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=hr_test;Username=postgres;Password=postgres";

    public static RhContext Create()
    {
        var options = new DbContextOptionsBuilder<RhContext>()
            .UseNpgsql(ConnectionString)
            .EnableSensitiveDataLogging()
            .Options;

        var context = new RhContext(options);

        context.Database.EnsureDeleted();
        context.Database.Migrate();

        return context;
    }
}
