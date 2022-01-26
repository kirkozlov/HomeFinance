﻿using System.ComponentModel.DataAnnotations;

namespace HomeFinance.Domain.Models
{
    public class Operation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HomeFinanceUserId { get; set; }

        public virtual HomeFinanceUser HomeFinanceUser { get; set; }

        [Required]
        public int WalletId { get; set; }

        public virtual Wallet Wallet { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// true if it is outgo, otherwise it is income
        /// </summary>
        [Required]
        public bool Outgo { get; set; }

        [Required]
        public double Amount { get; set; }

        public string? Comment { get; set; } = null;
    }
}