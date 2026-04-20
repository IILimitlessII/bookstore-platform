using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookstorePlatform.Web.Data;

public static class EpicBookStoreDb
{
    public static void ConfigureDbContext(DbContextOptionsBuilder options, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("Sqlite")
            ?? throw new InvalidOperationException("Connection string 'Sqlite' not found.");
        options.UseSqlite(cs);
    }
}
