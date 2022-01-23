#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HomeFinance.Domain.Repositories;
using HomeFinance.Models;
using System.Security.Claims;

namespace HomeFinance.Controllers
{
    public class WalletsController : Controller
    {
        readonly IWalletRepository _walletRepository;
        readonly IOperationRepository _operationRepository;
        readonly ICategoryRepository _categoryRepository;
        public WalletsController(IWalletRepository walletRepository, IOperationRepository operationRepository, ICategoryRepository categoryRepository )
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


            return View(wallets.Select(i=>new WalletViewModel(i) { Balance=allOperations.Where(j=>j.WalletId==i.Id).Sum(i=>(i.Outgo?-1:1)*i.Amount)}).ToList());
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

            var monthBegin = oldOperations.Sum(i => (i.Outgo ? -1 : 1) * i.Amount);
            var monthDiff = relevantOperations.Sum(i => (i.Outgo ? -1 : 1) * i.Amount);
            var monthEnd = monthBegin + monthDiff;

            var wallets = new List<WalletViewModel> { new WalletViewModel(wallet) };
            var categories = (await _categoryRepository.GetAll(userId)).Select(i => new CategoryViewModel(i)).ToList();

            var operationVMs=new List<OperationViewModel>();
            var balance = monthBegin;
            foreach (var operation in relevantOperations.OrderBy(i => i.DateTime))
            {
                balance+= (operation.Outgo ? -1 : 1)*operation.Amount;
                operationVMs.Add(new OperationViewModel(operation) {Balance=balance });
            }
            operationVMs.Reverse();

            var vm = new WalletOperationsViewModel(wallet)
            {
                OperationsOverviewViewModel = new OperationsOverviewViewModel
                {
                    Month = month,
                    RelevantOperations = operationVMs,
                    MonthBegin = monthBegin,
                    MonthDiff = monthDiff,
                    MonthEnd = monthEnd,
                    AllWallets = wallets,
                    AllCategories = categories
                }
            };
            return View(vm);
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
