using BookstorePlatform.Web.Areas.Identity.Data;
using BookstorePlatform.Web.Classes;
using BookstorePlatform.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

EnsureSqliteDataDirectoryExists(builder);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    EpicBookStoreDb.ConfigureDbContext(options, builder.Configuration));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<StoreUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IEmailSender, BookstorePlatform.Web.Services.Identity.NoOpEmailSender>();

builder.Services.AddCoreAdmin("admin");
builder.Services.AddScoped<Browse>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.UseCoreAdminCustomUrl("adminpanel");

await InitializeDatabaseAsync(app);

app.Run();

static async Task InitializeDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var provider = app.Configuration["Database:Provider"] ?? "Sqlite";

    if (string.Equals(provider, "Sqlite", StringComparison.OrdinalIgnoreCase)
        && app.Configuration.GetValue<bool>("Database:EnsureCreated", true))
    {
        await db.Database.EnsureCreatedAsync();
        return;
    }

    if (app.Configuration.GetValue<bool>("Database:AutoMigrate", false))
        await db.Database.MigrateAsync();
}

static void EnsureSqliteDataDirectoryExists(WebApplicationBuilder builder)
{
    if (!string.Equals(builder.Configuration["Database:Provider"] ?? "Sqlite", "Sqlite", StringComparison.OrdinalIgnoreCase))
        return;

    var sqliteCs = builder.Configuration.GetConnectionString("Sqlite");
    if (string.IsNullOrEmpty(sqliteCs))
        return;

    var cb = new SqliteConnectionStringBuilder(sqliteCs);
    var path = cb.DataSource;
    if (!Path.IsPathRooted(path))
        path = Path.Combine(builder.Environment.ContentRootPath, path);

    var dir = Path.GetDirectoryName(path);
    if (!string.IsNullOrEmpty(dir))
        Directory.CreateDirectory(dir);
}

