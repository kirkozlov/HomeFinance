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
        public WalletsController(IWalletRepository walletRepository )
        {
            _walletRepository = walletRepository;
        }

        // GET: Wallets
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View((await _walletRepository.GetForUser(User.FindFirst(ClaimTypes.NameIdentifier).Value)).Select(i=>new WalletViewModel(i)).ToList());
        }

        // GET: Wallets/Details/5
        public async Task<IActionResult> Details(int? id)
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
