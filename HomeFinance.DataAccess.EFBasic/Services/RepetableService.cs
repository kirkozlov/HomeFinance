using HomeFinanace.DataAccess.Core.DBModels;
using HomeFinance.Domain.Enums;
using HomeFinance.Domain.Services;

namespace HomeFinance.DataAccess.EFBasic.Services;

public class RepetableService : IRepetableService
{
    readonly HomeFinanceContextBase _homeFinanceContext;


    public RepetableService(HomeFinanceContextBase homeFinanceContext)
    {
        this._homeFinanceContext = homeFinanceContext;
    }


    public async Task FindAndExcecuteRepeatableOperation()
    {
        var workItems = this._homeFinanceContext.RepeatableOperations.Where(i => i.NextExecution < DateTime.Now).ToList();

        if (workItems.Any())
        {

            foreach (var workItem in workItems)
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

                switch (workItem.RepeatableType)
                {
                    case RepeatableType.Month:
                        workItem.NextExecution = workItem.NextExecution.AddMonths(1);
                        break;
                    case RepeatableType.Quarter:
                        workItem.NextExecution = workItem.NextExecution.AddMonths(3);
                        break;
                    case RepeatableType.Year:
                        workItem.NextExecution = workItem.NextExecution.AddYears(1);
                        break;
                    default: throw new InvalidOperationException("Unknown RepeatableType");
                }
            }

            await this._homeFinanceContext.SaveChangesAsync();
        }
    }
}