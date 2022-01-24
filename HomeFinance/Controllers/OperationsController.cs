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
        readonly ITransferRepository _transferRepository;

        public OperationsController(IWalletRepository walletRepository, ICategoryRepository categoryRepository, IOperationRepository operationRepository, ITransferRepository transferRepository)
        {
            _walletRepository = walletRepository;
            _categoryRepository = categoryRepository;
            _operationRepository = operationRepository;
            _transferRepository = transferRepository;
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
            var allTransfers= (await _transferRepository.GetAll(userId)).ToList();

            var oldOperations = allOperations.Where(i => i.DateTime < month).ToList();
            var relevantOperations = allOperations.Where(i => month <= i.DateTime && i.DateTime < month.AddMonths(1)).ToList();

            var oldTransfers = allTransfers.Where(i => i.DateTime < month).ToList();
            var relevantTransfers = allTransfers.Where(i => month <= i.DateTime && i.DateTime < month.AddMonths(1)).ToList();

            var monthBegin = oldOperations.Sum(i => (i.Outgo ? -1 : 1) * i.Amount);
            var monthDiff = relevantOperations.Sum(i => (i.Outgo ? -1 : 1) * i.Amount);
            var monthEnd = monthBegin + monthDiff;

            var wallets = (await _walletRepository.GetAll(userId)).ToDictionary(i=>i.Id.Value);
            var categories = (await _categoryRepository.GetAll(userId)).ToDictionary(i => i.Id.Value);


           var operationVMs= relevantOperations.Select(i => new OperationViewModel()
            {
                Id = i.Id.Value,
               IsTransfer = false,
               DateTime = i.DateTime,
                Wallet = wallets[i.WalletId].Name,
                Category = categories[i.CategoryId].Name,
                Income = i.Outgo ? null : i.Amount,
                Outgo = i.Outgo ? i.Amount : null,
                Comment = i.Comment ?? categories[i.CategoryId].Name,
            }).ToList();


            operationVMs.AddRange(relevantTransfers.Select(i => new OperationViewModel()
            {
                Id = i.Id.Value,
                IsTransfer = true,
                DateTime = i.DateTime,
                Wallet = $"{wallets[i.WalletIdFrom].Name} -> {wallets[i.WalletIdTo].Name}",
                Transfer = i.Amount,
                Category = "Transfer",
                Comment = i.Comment ?? "Transfer",
            })) ;

            var groupedOperations=operationVMs.GroupBy(i => i.DateTime.Date);

            var dayVMs = groupedOperations.Select(i => new DayViewModel(i.Key,i)).OrderByDescending(i=>i.Day).ToList();


            var vm = new MonthViewModel
            {
                Month = month,
                Days = dayVMs,
                MonthBegin = monthBegin,
                MonthDiff = monthDiff,
                MonthEnd = monthEnd               
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

            var vm =await CreateVM(datetime, userId);
            if (walletId.HasValue)
            {
                vm.Operation.WalletId = walletId.Value;
                vm.Transfer.WalletIdFrom = walletId.Value;
                vm.Operation.NavigateToWallet = true;
                vm.Transfer.NavigateToWallet = true;
            }
            return View(vm);
        }

        async Task<AddEditOperationViewModel> CreateVM(DateTime datetime, string userId)
        {
            var allCategories = (await _categoryRepository.GetAll(userId)).Select(i => new CategoryViewModel(i)).ToList();
            var vm = new AddEditOperationViewModel()
            {
                PossibleWallets = (await _walletRepository.GetAll(userId)).Select(i => new WalletViewModel(i)).ToList(),
                IncomeCategories = allCategories.Where(i => !i.Outgo).ToList(),
                OutgoCategories = allCategories.Where(i => i.Outgo).ToList(),
                Operation = new AddEditIncomeOutgoOperationViewModel() {  DateTime = datetime },
                Transfer = new AddEditTransferViewModel() { DateTime = datetime }
            };
            return vm;
        }

        // POST: OperationsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WalletId,CategoryId,DateTime,Outgo,Amount,Comment,NavigateToWallet")] AddEditIncomeOutgoOperationViewModel operation)
        {
            if (ModelState.IsValid)
            {
                await _operationRepository.Add(operation.ToDto(), User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (operation.NavigateToWallet)
                    return RedirectToAction(nameof(WalletsController.Details), "Wallets", new { id = operation.WalletId, monthB = operation.DateTime.Date.ToBinary() });
                return RedirectToAction(nameof(Index),new { monthB= operation.DateTime.Date.ToBinary()});
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new Exception();


            var vm = await CreateVM(operation.DateTime, userId);
            vm.Operation=operation;
            vm.Transfer.NavigateToWallet = operation.NavigateToWallet;
           

            return View(vm);
        }

        // POST: OperationsController/CreateTransfer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTransfer([Bind("WalletIdFrom,WalletIdTo,DateTime,Amount,Comment,NavigateToWallet")] AddEditTransferViewModel transfer)
        {
            if (ModelState.IsValid)
            {
                await _transferRepository.Add(transfer.ToDto(), User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (transfer.NavigateToWallet)
                    return RedirectToAction(nameof(WalletsController.Details), "Wallets", new { id = transfer.WalletIdFrom, monthB = transfer.DateTime.Date.ToBinary() });
                return RedirectToAction(nameof(Index), new { monthB = transfer.DateTime.Date.ToBinary() });
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new Exception();


            var vm = await CreateVM(transfer.DateTime, userId);
            vm.Transfer = transfer;
            vm.Operation.NavigateToWallet = transfer.NavigateToWallet;

            return View(nameof(Create),vm);
        }

        // GET: Operations/Edit/5
        public async Task<IActionResult> Edit(int id, bool isTransfer=false,  bool fromWallet=false)
        {

            if (!(User.Identity?.IsAuthenticated == true))
                return RedirectToAction("Index", "Home");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new Exception();

            if (!isTransfer)
            {
                var operation = await _operationRepository.GetById(id, userId);
                if (operation == null)
                    return NotFound();

                var vm = await CreateVM(operation.DateTime, userId);
                vm.Operation = new AddEditIncomeOutgoOperationViewModel( operation);
                vm.Operation.NavigateToWallet = fromWallet;
                return View(vm);
            }
            else
            {
                var transfer = await _transferRepository.GetById(id, userId);
                if (transfer == null)
                    return NotFound();

                var vm = await CreateVM(transfer.DateTime, userId);
                vm.Transfer = new AddEditTransferViewModel( transfer);
                vm.Transfer.NavigateToWallet = fromWallet;
                return View(vm);
            }
            
        }

        // POST: Operations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,WalletId,CategoryId,DateTime,Outgo,Amount,Comment,NavigateToWallet")] AddEditIncomeOutgoOperationViewModel operation)
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
                if (operation.NavigateToWallet)
                    return RedirectToAction(nameof(WalletsController.Details), "Wallets", new { id = operation.WalletId, monthB = operation.DateTime.Date.ToBinary() });
                return RedirectToAction(nameof(Index));
            }

            var vm = await CreateVM(operation.DateTime, userId);
            vm.Operation = operation;
            vm.Transfer.NavigateToWallet = operation.NavigateToWallet;
            return View(operation);
        }

        // POST: OperationsController/EditTransfer/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTransfer(int id, [Bind("Id,WalletIdFrom,WalletIdTo,DateTime,Amount,Comment,NavigateToWallet")] AddEditTransferViewModel transfer)
        {
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new Exception();

            if (ModelState.IsValid)
            {
                await _transferRepository.Update(transfer.ToDto(), User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (transfer.NavigateToWallet)
                    return RedirectToAction(nameof(WalletsController.Details), "Wallets", new { id = transfer.WalletIdFrom, monthB = transfer.DateTime.Date.ToBinary() });
                return RedirectToAction(nameof(Index), new { monthB = transfer.DateTime.Date.ToBinary() });
            }
            


            var vm = await CreateVM(transfer.DateTime, userId);
            vm.Transfer = transfer;
            vm.Transfer.NavigateToWallet = transfer.NavigateToWallet;

            return View(nameof(Edit),vm);
        }

        // POST: Operations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int? navigateToWallet=null, long? monthB=null)
        {
            await _operationRepository.Remove(id, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (navigateToWallet.HasValue)
                return RedirectToAction(nameof(WalletsController.Details), "Wallets", new { id = navigateToWallet.Value, monthB = monthB });

            return RedirectToAction(nameof(Index),new {  monthB = monthB });
        }

        // POST: Operations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTransfer(int id, int? navigateToWallet = null, long? monthB = null)
        {
            await _transferRepository.Remove(id, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (navigateToWallet.HasValue)
                return RedirectToAction(nameof(WalletsController.Details), "Wallets", new { id = navigateToWallet.Value, monthB = monthB });
            return RedirectToAction(nameof(Index), new { monthB = monthB });
        }
    }
}
