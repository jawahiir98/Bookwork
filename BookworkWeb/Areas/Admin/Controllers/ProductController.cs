using BookWork.DataAccess.Data;
using BookWork.DataAccess.Repository.IRepository;
using BookWork.Models;
using BookWork.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BookworkWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitofworkRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork _db, IWebHostEnvironment webHostEnvironment)
        {
            unitofworkRepository = _db;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            List<Product> products = unitofworkRepository.Product.GetAll().ToList();
            return View(products);
        }
        public IActionResult Upsert(int? id) // If id is found, it is an update function. Else just insert function. 
        {
            ProductVM pvm = new ProductVM()
            {
                CategoryList = unitofworkRepository.Category.GetAll().Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                ),
                Product = new Product()
            };
            if (id == null || id == 0)
            { // insert 
                return View(pvm);
            }
            else
            { // update 
                pvm.Product = unitofworkRepository.Product.Get(u => u.Id == id);
                return View(pvm);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productvm,IFormFile? file)
        {
            
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productvm.Product.ImageUrl = @"\images\product\" + fileName;
                }
                unitofworkRepository.Product.Add(productvm.Product);
                unitofworkRepository.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            productvm.CategoryList = unitofworkRepository.Category.GetAll().Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                );
            return View(productvm);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? cat = unitofworkRepository.Product.Get(u => u.Id == id);
            if (cat == null) return NotFound();
            return View(cat);
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
