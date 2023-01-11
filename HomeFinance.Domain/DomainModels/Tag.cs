﻿using HomeFinance.Domain.Enums;
using HomeFinance.Domain.Models;

namespace HomeFinance.Domain.DomainModels;

public record Tag(string Name, OperationType OperationType, int SortId)
{
}