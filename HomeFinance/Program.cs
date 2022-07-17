using HomeFinance.DataAccess;
using HomeFinance.DataAccess.EFBasic;
using HomeFinance.DataAccess.Sqlite;
using HomeFinance.Domain;
using HomeFinance.Domain.Models;
using HomeFinance.Domain.Repositories;
using HomeFinance.Domain.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
//using Microsoft.AspNetCore.Authentication.Cookies;




var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("HomeFinanceContextConnection");
builder.Services.AddDbContext<HomeFinanceContext>(options =>
{
    //options.UseSqlServer(connectionString);
    options.UseSqlite($@"Data Source = C:\Users\User\AppData\Local\dbtest.db");
    options.UseLazyLoadingProxies();
});
builder.Services.AddDefaultIdentity<HomeFinanceUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<HomeFinanceContext>();

//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Password.RequireDigit = false;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequiredLength = 4;

//});

builder.Services.AddScoped<HomeFinanceContextBase, HomeFinanceContext>();
builder.Services.AddScoped<IGateway, Gateway>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseRequestLocalization();

System.Globalization.CultureInfo customCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentCulture = customCulture;
CultureInfo.DefaultThreadCurrentUICulture = customCulture;



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
app.Run();
