using BookWork.DataAccess.Data;
using BookWork.DataAccess.Repository;
using BookWork.DataAccess.Repository.IRepository;
using BookWork.Models;
using BookWork.Models.ViewModels;
using BookWork.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BookworkWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
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
            List<Product> products = unitofworkRepository.Product.GetAll(includeProperties:"Category").ToList();

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
                    if (!string.IsNullOrEmpty(productvm.Product.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath =
                            Path.Combine(wwwRootPath, productvm.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productvm.Product.ImageUrl = @"\images\product\" + fileName;
                }
                else if(file == null)
                {
                    productvm.Product.ImageUrl = "";
                }
                if (productvm.Product.Id == 0)
                {
                    unitofworkRepository.Product.Add(productvm.Product);
                    TempData["success"] = "Product created successfully";
                }
                else
                {
                    unitofworkRepository.Product.Update(productvm.Product);
                    TempData["success"] = "Product edited successfully";
                }
                unitofworkRepository.Save();
               
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
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = unitofworkRepository.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = unitofworkRepository.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath =
                           Path.Combine(_webHostEnvironment.WebRootPath,
                           productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            unitofworkRepository.Product.Remove(productToBeDeleted);
            unitofworkRepository.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion
    }
}
