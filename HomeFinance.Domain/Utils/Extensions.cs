﻿using HomeFinance.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeFinance.Domain.Utils
{

        public static class Exctensions
        {
            public static IEnumerable<OperationDto> TransfersFrom(this IEnumerable<OperationDto> operations, int walletId)
            {
                return operations.Where(i => i.WalletId == walletId && i.OperationType == Domain.Enums.OperationType.Transfer);
            }

            public static IEnumerable<OperationDto> TransfersTo(this IEnumerable<OperationDto> operations, int walletId)
            {
                return operations.Where(i => i.WalletIdTo == walletId);
            }


            public static double GetSumFor(this IEnumerable<OperationDto> operations, int walletId)
            {
                double sum = 0;
                foreach (var operation in operations.Where(i => i.WalletId == walletId || i.WalletIdTo == walletId))
                {
                    switch (operation.OperationType)
                    {
                        case Domain.Enums.OperationType.Transfer:
                            if (operation.WalletId == walletId)
                            {
                                sum += operation.Amount;
                            }
                            else
                            {
                                sum -= operation.Amount;
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
}