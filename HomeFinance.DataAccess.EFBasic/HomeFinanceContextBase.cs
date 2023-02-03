using HomeFinance.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HomeFinanace.DataAccess.Core.DBModels;

namespace HomeFinance.DataAccess.EFBasic;

public abstract class HomeFinanceContextBase : IdentityDbContext<HomeFinanceUser>
{
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Operation> Operations { get; set; }
    public DbSet<RepeatableOperation> RepeatableOperations { get; set; }


    public HomeFinanceContextBase(DbContextOptions options)
        : base(options)
    {

    }
}