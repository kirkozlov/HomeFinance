using HomeFinance.DataAccess.EFBasic;
using HomeFinance.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HomeFinance.DataAccess.Sqlite
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<HomeFinanceContext>
    {
        public HomeFinanceContext CreateDbContext(string[] args)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var dbPath = System.IO.Path.Join(path, "dbtest.db");
            var builder = new DbContextOptionsBuilder();
            builder.UseSqlite($"Data Source={dbPath}");
            return new HomeFinanceContext(builder.Options);
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

            builder.Entity<Operation>().HasOne(x => x.Wallet).WithMany().HasForeignKey(x => x.WalletId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Operation>().HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RepeatableOperation>().HasOne(x => x.Wallet).WithMany().HasForeignKey(x => x.WalletId).OnDelete(DeleteBehavior.Restrict);
        }

    }
}