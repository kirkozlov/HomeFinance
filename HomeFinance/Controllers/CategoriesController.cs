#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HomeFinance.Domain.Repositories;
using HomeFinance.ViewModels;
using System.Security.Claims;

namespace HomeFinance.Controllers
{
    public class CategoriesController : Controller
    {
        readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        #region Helpers
        
        #endregion


        // GET: Categories
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View((await _categoryRepository.GetAll(User.FindFirst(ClaimTypes.NameIdentifier).Value)).Select(i => new CategoryViewModel(i)).ToList());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetById(id.Value, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(new CategoryViewModel(category));
        }

        // GET: Categories/Create
        public async Task< IActionResult> Create()
        {
            var allCategories = (await _categoryRepository.GetAll(User.FindFirst(ClaimTypes.NameIdentifier).Value)).Select(i => new AddEditCategoryViewModel(i)).ToList();
            return View(new AddEditCategoryViewModel() { PossibleParents = allCategories, Outgo=true });
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Outgo,ParentId,Comment")] AddEditCategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.Add(category.ToDto(), User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return RedirectToAction(nameof(Index));
            }
            var allCategories = (await _categoryRepository.GetAll(User.FindFirst(ClaimTypes.NameIdentifier).Value)).Select(i => new AddEditCategoryViewModel(i)).ToList();
            category.PossibleParents = allCategories;
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetById(id.Value, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (category == null)
            {
                return NotFound();
            }
            var allCategories = (await _categoryRepository.GetAll(User.FindFirst(ClaimTypes.NameIdentifier).Value)).Select(i => new AddEditCategoryViewModel(i)).ToList();
            return View(new AddEditCategoryViewModel(category) { PossibleParents=allCategories});
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Outgo,ParentId,Comment")] AddEditCategoryViewModel category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _categoryRepository.Update(category.ToDto(), User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return RedirectToAction(nameof(Index));
            }
            var allCategories = (await _categoryRepository.GetAll(User.FindFirst(ClaimTypes.NameIdentifier).Value)).Select(i => new AddEditCategoryViewModel(i)).ToList();
            category.PossibleParents = allCategories;
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetById(id.Value, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(new AddEditCategoryViewModel( category));
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryRepository.Remove(id, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return RedirectToAction(nameof(Index));
        }

        //private bool CategoryExists(int id)
        //{
        //    return _context.Categories.Any(e => e.Id == id);
        //}
    }
}
