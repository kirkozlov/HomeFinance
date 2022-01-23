﻿using HomeFinance.Domain.Dtos;

namespace HomeFinance.Models
{
    public class OperationViewModel
    {
        public int Id { get; set; }

        public string Wallet { get; set; }

        public string Category { get; set; }

        public DateTime DateTime { get; set; }

        public double? Income { get; set; }
        public double? Transfer { get; set; }
        public double? Outgo { get; set; }

        public double? Balance { get; set; }

        public string Comment { get; set; }
    }

    public class DayViewModel
    {
        public DateTime Day { get; set; }
        public double Income { get; set; }
        public double Outgo { get; set; }
        public IEnumerable<OperationViewModel> Operations { get; set; }


        public DayViewModel(DateTime day,IEnumerable<OperationViewModel> operations)
        {
            Day= day;

            if (operations.Any(i => i.DateTime.Date != day))
                throw new Exception();

            Income = operations.Sum(i => i.Income ?? 0);
            Outgo = operations.Sum(i => i.Outgo ?? 0);
            Operations=operations.OrderByDescending(i => i.DateTime).ToList();
        }
    }

    public class MonthViewModel
    {
        public DateTime Month { get; set; }
        public double MonthBegin { get; set; }
        public double MonthDiff { get; set; }
        public double MonthEnd { get; set; }
        public IEnumerable<DayViewModel> Days { get; set; }
    }


    public class AddEditOperationViewModel
    {
        public int? Id { get; set; }

        public int WalletId { get; set; }

        public int CategoryId { get; set; }

        public DateTime DateTime { get; set; }

        public bool Outgo { get; set; }

        public double Amount { get; set; }

        public string? Comment { get; set; }


        public IEnumerable<WalletViewModel>? PossibleWallets { get; set;  }
        
        public IEnumerable<CategoryViewModel>? PossibleCategories { get; set; }

        public AddEditOperationViewModel(OperationDto operation)
        {
            Id=operation.Id;
            WalletId=operation.WalletId;
            CategoryId=operation.CategoryId;
            DateTime = operation.DateTime;
            Outgo=operation.Outgo;
            Amount = operation.Amount;
            Comment = operation.Comment;
        }

        public AddEditOperationViewModel()
        {
        }

        public OperationDto ToDto()
        {
            return new OperationDto(Id, WalletId, CategoryId, DateTime, Outgo, Amount, Comment);
        }
    }

  
}
