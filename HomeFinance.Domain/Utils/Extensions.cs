﻿using HomeFinance.Domain.DomainModels;

namespace HomeFinance.Domain.Utils;

public static class Exctensions
{
    public static IEnumerable<Operation> TransfersFrom(this IEnumerable<Operation> operations, Guid walletId)
    {
        return operations.Where(i => i.WalletId == walletId && i.OperationType == Domain.Enums.OperationType.Transfer);
    }

    public static IEnumerable<Operation> TransfersTo(this IEnumerable<Operation> operations, Guid walletId)
    {
        return operations.Where(i => i.WalletToId == walletId);
    }


    public static double GetSumFor(this IEnumerable<Operation> operations, Guid walletId)
    {
        double sum = 0;
        foreach (var operation in operations.Where(i => i.WalletId == walletId || i.WalletToId == walletId))
        {
            switch (operation.OperationType)
            {
                case Domain.Enums.OperationType.Transfer:
                    if (operation.WalletId == walletId)
                    {
                        sum -= operation.Amount;
                    }
                    else
                    {
                        sum += operation.Amount;
                    }
                    break;
                case Domain.Enums.OperationType.Income:
                    sum += operation.Amount;
                    break;
                case Domain.Enums.OperationType.Expense:
                    sum -= operation.Amount;
                    break;
                default: throw new Exception();
            }
        }
        return sum;
    }
}