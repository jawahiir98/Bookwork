using BookWorkRazor.Data;
using BookWorkRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookWorkRazor.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public Category category { get; set; }
        public DeleteModel(ApplicationDbContext _db)
        {
            db = _db;
        }
        public void OnGet(int? id)
        {
            if (id != null && id != 0)
            {
                category = db.Categories.Find(id);
            }
        }

        public IActionResult OnPost()
        {
            Category? obj = db.Categories.Find(category.Id);
            if (obj == null)
            {
                return NotFound();
            }
            db.Categories.Remove(obj);
            db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToPage("Index");
        }

    }
}
