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
        public async Task<IActionResult> Index(DateTime? month)
        {
            if (!(User.Identity?.IsAuthenticated == true))
                return RedirectToAction("Index", "Home");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new Exception();

            if (month == null)
            {
                month = DateTime.Today;
            }
            month = month.Value.Date;
            month = month.Value.AddDays(-month.Value.Day + 1);



            var allOperations = (await _operationRepository.GetAll(userId)).ToList();

            var oldOperations = allOperations.Where(i => i.DateTime < month).ToList();
            var relevantOperations = allOperations.Where(i => month <= i.DateTime && i.DateTime <  month.Value.AddMonths(1)).ToList();

            var monthBegin = oldOperations.Sum(i => (i.Outgo ? -1 : 1) * i.Amount);
            var monthDiff = relevantOperations.Sum(i => (i.Outgo ? -1 : 1) * i.Amount);
            var monthEnd=monthBegin+ monthDiff;

            var wallets = (await _walletRepository.GetAll(userId)).Select(i=>new WalletViewModel(i)).ToList();
            var categories= (await _categoryRepository.GetAll(userId)).Select(i=>new CategoryViewModel(i)).ToList();

            var vm = new OperationsOverviewViewModel
            {
                Month = month.Value,
                RelevantOperations = relevantOperations.OrderByDescending(i=>i.DateTime).Select(i => new OperationViewModel(i)).ToList(),
                MonthBegin = monthBegin,
                MonthDiff = monthDiff,
                MonthEnd = monthEnd,
                AllWallets=wallets,
                AllCategories=categories
            };
            return View(vm);
        }

        // GET: OperationsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OperationsController/Create
        public async Task<IActionResult> Create()
        {
            if (!(User.Identity?.IsAuthenticated==true))
                return RedirectToAction("Index", "Home");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new Exception();
            var vm = new AddEditOperationViewModel()
            {
                PossibleWallets = (await _walletRepository.GetAll(userId)).Select(i => new WalletViewModel(i)).ToList(),
                PossibleCategories = (await _categoryRepository.GetAll(userId)).Select(i => new CategoryViewModel(i)).ToList(),
                DateTime=DateTime.Now
            };
            return View(vm);
        }

        // POST: OperationsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,WalletId,CategoryId,DateTime,Outgo,Comment")] AddEditOperationViewModel operation)
        {
            operation.Amount = double.Parse(Request.Form["Amount"], System.Globalization.CultureInfo.InvariantCulture);
            if (ModelState.IsValid)
            {
                await  _operationRepository.Add(operation.ToDto(), User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return RedirectToAction(nameof(Index));
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new Exception();
            operation.PossibleWallets = (await _walletRepository.GetAll(userId)).Select(i => new WalletViewModel(i)).ToList();
            operation.PossibleCategories = (await _categoryRepository.GetAll(userId)).Select(i => new CategoryViewModel(i)).ToList();
            return View(operation);
        }

        // GET: OperationsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OperationsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: OperationsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OperationsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
