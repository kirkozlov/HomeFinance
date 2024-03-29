using HomeFinance.DataAccess;
using HomeFinance.DataAccess.Sqlite;
//using HomeFinance.DataAccess.MsSql;
using HomeFinance.Domain.Models;
using HomeFinance.Domain.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HomeFinance.DataAccess.EFBasic;
using HomeFinance.DataAccess.EFBasic.Services;
using HomeFinanceApi;
using HomeFinanceApi.Controllers;
using HomeFinanceApi.BackgroundWorker;
using HomeFinance.Domain.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccess(builder.Configuration);

builder.Services.AddDefaultIdentity<HomeFinanceUser>()
    .AddEntityFrameworkStores<HomeFinanceContext>();


builder.Services.Configure<IdentityOptions>(options => 
    { 
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 4;

    });
builder.Services.AddScoped<HomeFinanceContextBase, HomeFinanceContext>();
builder.Services.AddScoped<IGateway, Gateway>();

builder.Services.AddCors();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var key = Encoding.UTF8.GetBytes(builder.Configuration["ApplicationSettings:JWT_Secret"]);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    //x.RequireHttpsMetadata = false;
    x.SaveToken = false;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer=false,
        ValidateAudience=false,
        ClockSkew=TimeSpan.Zero,
    };
});

builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IRepeatableService, RepeatableService>();
builder.Services.AddHostedService<RepeatableExecution>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    HomeFinanceContextBase dc = scope.ServiceProvider.GetRequiredService<HomeFinanceContextBase>();
    dc.Database.Migrate(); // This may create the migration history table, but no further ones.
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(builder=>builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
TagApiSet.Map(app);
WalletApiSet.Map(app); 
OperationApiSet.Map(app);
TransientOperationApiSet.Map(app);
RepeatableOperationApiSet.Map(app);
UserPreferencesApiSet.Map(app);
app.MapGet("", () =>
{
    return Results.Redirect("http://lemon-dune-01d325903.2.azurestaticapps.net");
});
app.MapGet("api/test", () =>
{
    return Results.Ok(DateTime.Now);
});
app.Run();