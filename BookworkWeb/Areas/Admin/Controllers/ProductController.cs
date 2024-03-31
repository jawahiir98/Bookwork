using BookWork.DataAccess.Repository.IRepository;
using BookWork.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookworkWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitofworkRepository;
        public ProductController(IUnitOfWork _db)
        {
            unitofworkRepository = _db;
        }
        public IActionResult Index()
        {
            List<Product> productList = unitofworkRepository.Product.GetAll().ToList();
            return View(productList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                unitofworkRepository.Product.Add(product);
                unitofworkRepository.Save();
                TempData["success"] = "Product created successfully";
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
            Product? product = unitofworkRepository.Product.Get(u => u.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                unitofworkRepository.Product.Update(product);
                unitofworkRepository.Save();
                TempData["success"] = "Product edited successfully";
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
            Product? product = unitofworkRepository.Product.Get(u => u.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? cat = unitofworkRepository.Product.Get(c => c.Id == id);
            if (cat == null) return NotFound();
            unitofworkRepository.Product.Remove(cat);
            unitofworkRepository.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
