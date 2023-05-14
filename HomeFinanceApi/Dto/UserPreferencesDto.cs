using HomeFinance.Domain.DomainModels;
using TimeZoneConverter;

namespace HomeFinanceApi.Dto
{
    public record UserPreferencesDto(string TimeZone)
    {
        public UserPreferences ToDomain()
        {
           return new UserPreferences( TZConvert.GetTimeZoneInfo(TimeZone));
        }
    }
}
