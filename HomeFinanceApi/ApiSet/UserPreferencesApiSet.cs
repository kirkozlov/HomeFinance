using HomeFinance.Domain.Utils;
using Microsoft.AspNetCore.Authorization;
using HomeFinance.Domain.DomainModels;
using TimeZoneConverter;
using HomeFinanceApi.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeFinanceApi.Controllers;

public static class UserPreferencesApiSet
{

    public static void Map(WebApplication app)
    {
        app.MapGet("api/preferences/", Get);
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