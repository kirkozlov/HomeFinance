using HomeFinance.DataAccess.Core.DBModels;
using HomeFinance.DataAccess.EFBasic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeFinance.DataAccess.Sqlite;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<HomeFinanceContext>
{
    public HomeFinanceContext CreateDbContext(string[] args)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = System.IO.Path.Join(path, "main.db");
        var builder = new DbContextOptionsBuilder();
        builder.UseSqlite($"Data Source={dbPath}");
        return new HomeFinanceContext(builder.Options);
    }
}

public static class DBExtension
{
    public static IServiceCollection AddDataAccess(this IServiceCollection self, ConfigurationManager configuration)
    {
        var directory = @"C:\home\site\HomeFinanceDB";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        self.AddDbContext<HomeFinanceContext>(options =>
        {
            
            options.UseSqlite($@"Data Source = {directory}\main.db").UseLazyLoadingProxies();
        });
        return self;
    }
}


public class HomeFinanceContext : HomeFinanceContextBase
{

    public HomeFinanceContext(DbContextOptions options):base(options)
    {
            
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<Tag>().HasKey(t => new { t.Name, t.OperationType });
    }

}