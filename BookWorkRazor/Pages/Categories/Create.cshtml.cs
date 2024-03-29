using BookWorkRazor.Data;
using BookWorkRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookWorkRazor.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext db;
        
        public Category category { get; set; }
        public CreateModel(ApplicationDbContext _db)
        {
            db = _db;
        }

        public void OnGet()
        {
            
        }
        public IActionResult OnPost() { 
            db.Categories.Add(category);
            db.SaveChanges();
            TempData["success"] = "Category created successfully.";
            return RedirectToPage("Index");
        }
    }
}
