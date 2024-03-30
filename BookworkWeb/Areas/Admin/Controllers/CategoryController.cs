using BookWork.DataAccess.Data;
using BookWork.DataAccess.Repository.IRepository;
using BookWork.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookworkWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitofworkRepository;
        public CategoryController(IUnitOfWork _db)
        {
            unitofworkRepository = _db;
        }
        public IActionResult Index()
        {
            List<Category> categoryList = unitofworkRepository.Category.GetAll().ToList();
            return View(categoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Name and display order cannot be the same");
            }
            if (ModelState.IsValid)
            {
                unitofworkRepository.Category.Add(category);
                unitofworkRepository.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? cat = unitofworkRepository.Category.Get(u => u.Id == id);
            if (cat == null) return NotFound();
            return View(cat);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                unitofworkRepository.Category.Update(category);
                unitofworkRepository.Save();
                TempData["success"] = "Category edited successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? cat = unitofworkRepository.Category.Get(u => u.Id == id);
            if (cat == null) return NotFound();
            return View(cat);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? cat = unitofworkRepository.Category.Get(c => c.Id == id);
            if (cat == null) return NotFound();
            unitofworkRepository.Category.Remove(cat);
            unitofworkRepository.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
