using HomeFinance.Domain.Repositories;
using HomeFinance.Domain.Utils;
using HomeFinance.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeFinance.Controllers
{
    public class OperationsController : Controller
    {

        readonly IGateway _unitOfWork;

        public OperationsController(IGateway unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            var allOperations = (await _unitOfWork.OperationRepository.GetAll(userId)).ToList();

            var oldOperations = allOperations.Where(i => i.DateTime < month).ToList();
            var relevantOperations = allOperations.Where(i => month <= i.DateTime && i.DateTime < month.AddMonths(1)).ToList();

          
            var monthBegin = oldOperations.Where(i=>i.OperationType!=Domain.Enums.OperationType.Transfer).Sum(i => (i.OperationType==Domain.Enums.OperationType.Income ? -1 : 1) * i.Amount);
            var monthDiff = relevantOperations.Where(i => i.OperationType != Domain.Enums.OperationType.Transfer).Sum(i => (i.OperationType == Domain.Enums.OperationType.Expense ? -1 : 1) * i.Amount);
            var monthEnd = monthBegin + monthDiff;

            var wallets = (await _unitOfWork.WalletRepository.GetAll(userId)).ToDictionary(i=>i.Id.Value);
            var categories = (await _unitOfWork.CategoryRepository.GetAll(userId)).ToDictionary(i => i.Id.Value);


           var operationVMs= relevantOperations.Select(i => new OperationViewModel()
            {
                Id = i.Id.Value,
                IsTransfer = i.OperationType == Domain.Enums.OperationType.Transfer,
                DateTime = i.DateTime,
                Wallet = i.OperationType == Domain.Enums.OperationType.Transfer? $"{wallets[i.WalletId].Name} -> {wallets[i.WalletIdTo.Value].Name}" : wallets[i.WalletId].Name,
                Category = i.OperationType == Domain.Enums.OperationType.Transfer? "Transfer": categories[i.CategoryId.Value].Name,
                Income = i.OperationType == Domain.Enums.OperationType.Income ? i.Amount : null,
                Outgo = i.OperationType == Domain.Enums.OperationType.Expense ? i.Amount : null,
                Transfer = i.OperationType == Domain.Enums.OperationType.Transfer ? i.Amount : null,
                Comment = i.Comment ?? (i.OperationType == Domain.Enums.OperationType.Transfer ? "Transfer" : categories[i.CategoryId.Value].Name),
            }).ToList();


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
            var allCategories = (await _unitOfWork.CategoryRepository.GetAll(userId)).Select(i => new CategoryViewModel(i)).ToList();
            var vm = new AddEditOperationViewModel()
            {
                PossibleWallets = (await _unitOfWork.WalletRepository.GetAll(userId)).Select(i => new WalletViewModel(i)).ToList(),
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
                await _unitOfWork.OperationRepository.Add(operation.ToDto(), User.FindFirst(ClaimTypes.NameIdentifier).Value);
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
                await _unitOfWork.OperationRepository.Add(transfer.ToDto(), User.FindFirst(ClaimTypes.NameIdentifier).Value);
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
                var operation = await _unitOfWork.OperationRepository.GetById(id, userId);
                if (operation == null)
                    return NotFound();

                var vm = await CreateVM(operation.DateTime, userId);
                vm.Operation = new AddEditIncomeOutgoOperationViewModel( operation);
                vm.Operation.NavigateToWallet = fromWallet;
                return View(vm);
            }
            else
            {
                var transfer = await _unitOfWork.OperationRepository.GetById(id, userId);
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
                await _unitOfWork.OperationRepository.Update(operation.ToDto(), userId);
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
                await _unitOfWork.OperationRepository.Update(transfer.ToDto(), User.FindFirst(ClaimTypes.NameIdentifier).Value);
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
            await _unitOfWork.OperationRepository.Remove(id, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (navigateToWallet.HasValue)
                return RedirectToAction(nameof(WalletsController.Details), "Wallets", new { id = navigateToWallet.Value, monthB = monthB });

            return RedirectToAction(nameof(Index),new {  monthB = monthB });
        }

        // POST: Operations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTransfer(int id, int? navigateToWallet = null, long? monthB = null)
        {
            await _unitOfWork.OperationRepository.Remove(id, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (navigateToWallet.HasValue)
                return RedirectToAction(nameof(WalletsController.Details), "Wallets", new { id = navigateToWallet.Value, monthB = monthB });
            return RedirectToAction(nameof(Index), new { monthB = monthB });
        }
    }
}
