﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HomeFinance.Domain.Repositories;
using HomeFinance.ViewModels;
using System.Security.Claims;
using HomeFinance.Domain.Dtos;

namespace HomeFinance.Controllers
{
    public static class Exctensions
    {
        public static IEnumerable<OperationDto> TransfersFrom(this IEnumerable<OperationDto> operations, int walletId)
        {
            return operations.Where(i => i.WalletId == walletId && i.OperationType==Domain.Enums.OperationType.Transfer);
        }

        public static IEnumerable<OperationDto> TransfersTo(this IEnumerable<OperationDto> operations, int walletId)
        {
            return operations.Where(i => i.WalletIdTo == walletId);
        }


        public static double GetSumFor(this IEnumerable<OperationDto> operations, int walletId)
        {
            double sum = 0;
            foreach (var operation in operations.Where(i=>i.WalletId==walletId||i.WalletIdTo==walletId))
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
                        sum+=operation.Amount;
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

    public class WalletsController : Controller
    {
        readonly IWalletRepository _walletRepository;
        readonly IOperationRepository _operationRepository;
        readonly ICategoryRepository _categoryRepository;
        public WalletsController(IWalletRepository walletRepository, IOperationRepository operationRepository, ICategoryRepository categoryRepository)
        {
            _walletRepository = walletRepository;
            _operationRepository= operationRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: Wallets
        public async Task<IActionResult> Index()
        {
            if (!(User.Identity?.IsAuthenticated == true))
                return RedirectToAction("Index", "Home");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new Exception();


            var allOperations = (await _operationRepository.GetAll(userId)).ToList();
            var wallets = await _walletRepository.GetAll(userId);

            return View(wallets.Select(i=>new WalletViewModel(i) { Balance=allOperations.GetSumFor(i.Id.Value)}).ToList());
        }

        // GET: Wallets/Details/5
        public async Task<IActionResult> Details(int? id, long? monthB)
        {
            if (!(User.Identity?.IsAuthenticated == true))
                return RedirectToAction("Index", "Home");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new Exception();

            if (id == null)
            {
                return NotFound();
            }
            var wallet = await _walletRepository.GetById(id.Value, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (wallet == null)
            {
                return NotFound();
            }

        

            var month = monthB.HasValue ? DateTime.FromBinary(monthB.Value) : DateTime.Today;

            month = month.Date;
            month = month.AddDays(-month.Day + 1);

            var allOperations = (await _operationRepository.GetForWallet(userId, wallet.Id.Value)).ToList();
           

            

            var oldOperations = allOperations.Where(i => i.DateTime < month).ToList();
            var relevantOperations = allOperations.Where(i => month <= i.DateTime && i.DateTime < month.AddMonths(1)).ToList();
            
         
            var monthBegin = oldOperations.GetSumFor(wallet.Id.Value);
            var monthDiff = relevantOperations.GetSumFor(wallet.Id.Value);
            var monthEnd = monthBegin + monthDiff;

            var wallets = (await _walletRepository.GetAll(userId)).ToDictionary(i => i.Id.Value);
            var categories = (await _categoryRepository.GetAll(userId)).ToDictionary(i => i.Id.Value);

            var operationVMs = relevantOperations.Where(i=>i.OperationType!=Domain.Enums.OperationType.Transfer).Select(i => new OperationViewModel()
            {
                Id = i.Id.Value,
                IsTransfer = false,
                DateTime = i.DateTime,
                Wallet = wallets[i.WalletId].Name,
                Category = categories[i.CategoryId.Value].Name,
                Income = i.OperationType == Domain.Enums.OperationType.Income ? i.Amount : null,
                Outgo = i.OperationType== Domain.Enums.OperationType.Expense ? i.Amount : null,
                Comment = i.Comment ?? categories[i.CategoryId.Value].Name,
            }).ToList();

            operationVMs.AddRange(relevantOperations.TransfersTo(wallet.Id.Value).Select(i => new OperationViewModel()
            {
                Id = i.Id.Value,
                IsTransfer = true,
                DateTime = i.DateTime,
                Wallet = $"{wallets[i.WalletId].Name} -> {wallets[i.WalletIdTo.Value].Name}",
                Income = i.Amount,
                Category = "Transfer",
                Comment = i.Comment ?? "Transfer",
            }));
            operationVMs.AddRange(relevantOperations.TransfersFrom(wallet.Id.Value).Select(i => new OperationViewModel()
            {
                Id = i.Id.Value,
                IsTransfer = true,
                DateTime = i.DateTime,
                Wallet = $"{wallets[i.WalletId].Name} -> {wallets[i.WalletIdTo.Value].Name}",
                Outgo = i.Amount,
                Category = "Transfer",
                Comment = i.Comment ?? "Transfer",
            }));


            var balance = monthBegin;
            foreach (var operation in operationVMs.OrderBy(i => i.DateTime))
            {
                balance+= operation.Income ?? -operation.Outgo.Value;
                operation.Balance = balance;
            }

            var groupedOperations = operationVMs.GroupBy(i => i.DateTime.Date);
            var dayVMs = groupedOperations.Select(i => new DayViewModel(i.Key, i)).OrderByDescending(i => i.Day).ToList();


            var vm = new MonthViewModel
            {
                Month = month,
                Days = dayVMs,
                MonthBegin = monthBegin,
                MonthDiff = monthDiff,
                MonthEnd = monthEnd
            };
            return View(new WalletOperationsViewModel(wallet) { MonthViewModel = vm });
        }

        // GET: Wallets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Wallets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,GroupName,Comment")] WalletViewModel wallet)
        {
            if (ModelState.IsValid)
            {
                await _walletRepository.Add(wallet.ToDto(), User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return RedirectToAction(nameof(Index));
            }
            return View(wallet);
        }

        // GET: Wallets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wallet = await _walletRepository.GetById(id.Value, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (wallet == null)
            {
                return NotFound();
            }
            return View(new WalletViewModel(wallet));
        }

        // POST: Wallets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,GroupName,Comment")] WalletViewModel wallet)
        {
            if (id != wallet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _walletRepository.Update(wallet.ToDto(), User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return RedirectToAction(nameof(Index));
            }
            return View(wallet);
        }

        // GET: Wallets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wallet = await _walletRepository.GetById(id.Value, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(new WalletViewModel(wallet));
        }

        // POST: Wallets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _walletRepository.Remove(id,User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return RedirectToAction(nameof(Index));
        }

        //private bool WalletExists(int id)
        //{
        //    return _context.Wallets.Any(e => e.Id == id);
        //}
    }
}
