using HomeFinanace.DataAccess.Core.DBModels;
using HomeFinance.DataAccess.EFBasic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeFinance.DataAccess.MsSql;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<HomeFinanceContext>
{
    public HomeFinanceContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../HomeFinanceApi/appsettings.json").Build();
        var builder = new DbContextOptionsBuilder();
        var connectionString = configuration.GetConnectionString("HomeFinanceContextConnection");
        builder.UseSqlServer(connectionString);
        return new HomeFinanceContext(builder.Options);
    }
}

public static class DBExtension
{
    public static IServiceCollection AddDataAccess(this IServiceCollection self, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("HomeFinanceContextConnection");
        self.AddDbContext<HomeFinanceContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.UseLazyLoadingProxies();
        });
        return self;
    }
}

public class HomeFinanceContext : HomeFinanceContextBase
{
    public HomeFinanceContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<Operation>().HasOne(x => x.Wallet).WithMany().HasForeignKey(x => x.WalletId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<RepeatableOperation>().HasOne(x => x.Wallet).WithMany().HasForeignKey(x => x.WalletId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Tag>().HasOne(i=>i.HomeFinanceUser).WithMany().HasForeignKey(x => x.HomeFinanceUserId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Tag>().HasKey(t => new { t.Name, t.OperationType });
    }
}
