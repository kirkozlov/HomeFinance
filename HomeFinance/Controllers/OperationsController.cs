using HomeFinance.Domain.Repositories;
using HomeFinance.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeFinance.Controllers
{
    public class OperationsController : Controller
    {

        readonly IWalletRepository _walletRepository;
        readonly ICategoryRepository _categoryRepository;
        readonly IOperationRepository _operationRepository;

        public OperationsController(IWalletRepository walletRepository, ICategoryRepository categoryRepository, IOperationRepository operationRepository)
        {
            _walletRepository = walletRepository;
            _categoryRepository = categoryRepository;
            _operationRepository = operationRepository;
        }



        // GET: OperationsController
        public async Task<IActionResult> Index(long? monthB)
        {
            if (!(User.Identity?.IsAuthenticated == true))
                return RedirectToAction("Index", "Home");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new Exception();

            var  month= monthB.HasValue? DateTime.FromBinary(monthB.Value):DateTime.Today;

           
            month = month.Date;
            month = month.AddDays(-month.Day + 1);

            var allOperations = (await _operationRepository.GetAll(userId)).ToList();

            var oldOperations = allOperations.Where(i => i.DateTime < month).ToList();
            var relevantOperations = allOperations.Where(i => month <= i.DateTime && i.DateTime < month.AddMonths(1)).ToList();

            var monthBegin = oldOperations.Sum(i => (i.Outgo ? -1 : 1) * i.Amount);
            var monthDiff = relevantOperations.Sum(i => (i.Outgo ? -1 : 1) * i.Amount);
            var monthEnd = monthBegin + monthDiff;

            var wallets = (await _walletRepository.GetAll(userId)).Select(i => new WalletViewModel(i)).ToList();
            var categories = (await _categoryRepository.GetAll(userId)).Select(i => new CategoryViewModel(i)).ToList();

            var vm = new OperationsOverviewViewModel
            {
                Month = month,
                RelevantOperations = relevantOperations.OrderByDescending(i => i.DateTime).Select(i => new OperationViewModel(i)).ToList(),
                MonthBegin = monthBegin,
                MonthDiff = monthDiff,
                MonthEnd = monthEnd,
                AllWallets = wallets,
                AllCategories = categories
            };
            return View(vm);
        }

        // GET: OperationsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OperationsController/Create
        public async Task<IActionResult> Create(long? day, int? walletId)
        {
            if (!(User.Identity?.IsAuthenticated == true))
                return RedirectToAction("Index", "Home");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new Exception();


            var datetime = DateTime.Now;
            if (day.HasValue)
            {
                datetime = DateTime.FromBinary(day.Value) + datetime.TimeOfDay;
            }

            var vm = new AddEditOperationViewModel()
            {
                PossibleWallets = (await _walletRepository.GetAll(userId)).Select(i => new WalletViewModel(i)).ToList(),
                PossibleCategories = (await _categoryRepository.GetAll(userId)).Select(i => new CategoryViewModel(i)).ToList(),
                DateTime = datetime
            };
            if(walletId.HasValue)
                vm.WalletId = walletId.Value;
            return View(vm);
        }

        // POST: OperationsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,WalletId,CategoryId,DateTime,Outgo,Amount,Comment")] AddEditOperationViewModel operation)
        {
            if (ModelState.IsValid)
            {
                await _operationRepository.Add(operation.ToDto(), User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return RedirectToAction(nameof(Index),new { monthB= operation.DateTime.Date.ToBinary()});
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new Exception();
            operation.PossibleWallets = (await _walletRepository.GetAll(userId)).Select(i => new WalletViewModel(i)).ToList();
            operation.PossibleCategories = (await _categoryRepository.GetAll(userId)).Select(i => new CategoryViewModel(i)).ToList();
            return View(operation);
        }

        // GET: Operations/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (!(User.Identity?.IsAuthenticated == true))
                return RedirectToAction("Index", "Home");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new Exception();

            var operation = await _operationRepository.GetById(id, userId);
            if (operation == null)
                return NotFound();

            var vm = new AddEditOperationViewModel(operation)
            {
                PossibleWallets = (await _walletRepository.GetAll(userId)).Select(i => new WalletViewModel(i)).ToList(),
                PossibleCategories = (await _categoryRepository.GetAll(userId)).Select(i => new CategoryViewModel(i)).ToList(),
            };
            return View(vm);
        }

        // POST: Operations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,WalletId,CategoryId,DateTime,Outgo,Amount,Comment")] AddEditOperationViewModel operation)
        {
            if (id != operation.Id)
            {
                return NotFound();
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new Exception();

            if (ModelState.IsValid)
            {
                await _operationRepository.Update(operation.ToDto(), userId);
                return RedirectToAction(nameof(Index));
            }
            operation.PossibleWallets = (await _walletRepository.GetAll(userId)).Select(i => new WalletViewModel(i)).ToList();
            operation.PossibleCategories = (await _categoryRepository.GetAll(userId)).Select(i => new CategoryViewModel(i)).ToList();
            return View(operation);
        }

        // POST: Operations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _operationRepository.Remove(id, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return RedirectToAction(nameof(Index));
        }
    }
}
