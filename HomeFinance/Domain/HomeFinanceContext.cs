using HomeFinance.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HomeFinance.Models;

namespace HomeFinance.Domain;

public class HomeFinanceContext : IdentityDbContext<HomeFinanceUser>
{
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Operation> Operations { get; set; }

    public HomeFinanceContext(DbContextOptions<HomeFinanceContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<HomeFinance.Models.OperationViewModel> OperationViewModel { get; set; }
}
