using HomeFinance.Domain.Utils;
using Microsoft.AspNetCore.Authorization;
using HomeFinance.Domain.DomainModels;
using TimeZoneConverter;
using HomeFinanceApi.Dto;
using System.Threading.RateLimiting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeFinanceApi.Controllers;

public static class UserPreferencesApiSet
{

    public static void Map(WebApplication app)
    {
        app.MapGet("api/preferences/", Get);
        app.MapGet("api/preferences/updatedates", UpdateDates);
        app.MapGet("api/preferences/addday", AddDayToRepeatable);
        app.MapPost("api/preferences/", Post);
        app.MapPut("api/preferences/", Put);
    }

    [Authorize]
    static async Task<UserPreferencesDto> Get(IGateway unitOfWork)
    {
        var preferences = (await unitOfWork.UserPreferencesRepository.GetAll()).Single();
        return new UserPreferencesDto(TZConvert.WindowsToIana(preferences.TimeZone.Id));
    }


    [Authorize]
    static async Task UpdateDates(IGateway unitOfWork)
    {
        var allOps= await unitOfWork.RepeatableOperationRepository.GetAll();
        var newOps=allOps.Select(i =>new RepeatableOperation(i.Id, i.WalletId, i.OperationType, i.Tags, i.Amount, i.Comment, i.WalletToId, i.NextExecution.Date, i.RepeatableType));
        await unitOfWork.RepeatableOperationRepository.Update(newOps);
    }

    [Authorize]
    static async Task AddDayToRepeatable(IGateway unitOfWork)
    {
        var allOps = await unitOfWork.RepeatableOperationRepository.GetAll();
        var newOps = allOps.Select(i => new RepeatableOperation(i.Id, i.WalletId, i.OperationType, i.Tags, i.Amount, i.Comment, i.WalletToId, i.NextExecution.Date.AddDays(1), i.RepeatableType));
        await unitOfWork.RepeatableOperationRepository.Update(newOps);
    }

    [Authorize]
    static async Task Post(UserPreferencesDto preferences, IGateway unitOfWork)
    {
        await unitOfWork.UserPreferencesRepository.Add(preferences.ToDomain());
    }
    [Authorize]
    static async Task Put(UserPreferencesDto preferences, IGateway unitOfWork)
    {
        await unitOfWork.UserPreferencesRepository.Update(preferences.ToDomain());
    }
}