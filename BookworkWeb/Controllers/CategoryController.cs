using BookworkWeb.Data;
using BookworkWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookworkWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext db;
        public CategoryController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            List<Category> categoryList = db.Categories.ToList();   
            return View(categoryList);
        }
    }
}
