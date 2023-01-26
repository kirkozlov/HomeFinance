using HomeFinanace.DataAccess.Core.DBModels;
using HomeFinance.DataAccess.EFBasic;
using HomeFinance.Domain.Enums;
using HomeFinance.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace HomeFinanceApi.BackgroundWorker
{
    public class RepeatableExecution : BackgroundService
    {
        private readonly HomeFinanceContextBase _dbContext;

        public RepeatableExecution(HomeFinanceContextBase dbContext)
        {
            _dbContext = dbContext;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                var workItems = _dbContext.RepeatableOperations.Where(i => i.NextExecution < DateTime.Now).ToList();
                foreach (var workItem in workItems)
                {
                    this._dbContext.Operations.Add(new Operation()
                    {
                        Id = new Guid(),
                        WalletId = workItem.WalletId,
                        OperationType = workItem.OperationType,
                        Tags = workItem.Tags,
                        WalletToId = workItem.WalletToId,
                        Amount = workItem.Amount,
                        Comment = workItem.Comment,
                        DateTime = workItem.NextExecution
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

                this._dbContext.SaveChanges();

                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }
    }
}
