using HomeFinance.DataAccess.Core.DBModels;
using HomeFinance.DataAccess.EFBasic.Util;
using HomeFinance.Domain.Enums;
using HomeFinance.Domain.Services;

namespace HomeFinance.DataAccess.EFBasic.Services;

public class RepeatableService : IRepeatableService
{
    readonly HomeFinanceContextBase _homeFinanceContext;


    public RepeatableService(HomeFinanceContextBase homeFinanceContext)
    {
        this._homeFinanceContext = homeFinanceContext;
    }


    public async Task FindAndExecuteRepeatableOperation()
    {
        var workItems = this._homeFinanceContext.RepeatableOperations.Where(i => i.NextExecution < DateTime.UtcNow).ToList();

        if (workItems.Any())
        {
            var users=workItems.GroupBy(i => i.HomeFinanceUserId);

            foreach (var user in users)
            {
                var dateConverter = new DateConverter(TimeZoneInfo
                              .FindSystemTimeZoneById(_homeFinanceContext.UserPreferences.Single(i => i.HomeFinanceUserId == user.Key).TimeZoneId));


                foreach (var workItem in user)
                {
                    this._homeFinanceContext.Operations.Add(new Operation()
                    {
                        Id = new Guid(),
                        WalletId = workItem.WalletId,
                        OperationType = workItem.OperationType,
                        Tags = workItem.Tags,
                        WalletToId = workItem.WalletToId,
                        Amount = workItem.Amount,
                        Comment = workItem.Comment,
                        DateTime = workItem.NextExecution,
                        HomeFinanceUserId = workItem.HomeFinanceUserId
                    });

                    var localDate = dateConverter.ToLocalDateTime(workItem.NextExecution);
                    switch (workItem.RepeatableType)
                    {
                        case RepeatableType.Month:
                            localDate = localDate.AddMonths(1);
                            break;
                        case RepeatableType.Quarter:
                            localDate = localDate.AddMonths(3);
                            break;
                        case RepeatableType.Year:
                            localDate = localDate.AddYears(1);
                            break;
                        default: throw new InvalidOperationException("Unknown RepeatableType");
                    }
                    workItem.NextExecution = dateConverter.ToUtcDateTime( localDate);
                }

            }

            

            await this._homeFinanceContext.SaveChangesAsync();
        }
    }
}