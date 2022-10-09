﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using HomeFinance.Domain.Enums;
using HomeFinance.Domain.Models;

namespace HomeFinance.Domain.DomainModels;

public abstract record OperationBase(Guid? Id, Guid WalletId, OperationType OperationType, IEnumerable<string> Tags, double Amount, string Comment, Guid? WalletIdTo)
{
}

public record Operation(Guid? Id, Guid WalletId, OperationType OperationType, IEnumerable<string> Tags, double Amount, string Comment, Guid? WalletIdTo, DateTime DateTime) 
    : OperationBase(Id, WalletId, OperationType, Tags, Amount, Comment, WalletIdTo)
{
}

public record RepeatableOperation(Guid? Id, Guid WalletId, OperationType OperationType, IEnumerable<string> Tags, double Amount, string Comment, Guid? WalletIdTo, DateTime NextExecution, RepeatableType RepeatableType) 
    : OperationBase(Id, WalletId, OperationType, Tags, Amount, Comment, WalletIdTo)
{
}