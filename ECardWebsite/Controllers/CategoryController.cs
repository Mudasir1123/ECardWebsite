using ECardWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECardWebsite.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // INDEX - Show all categories
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // CREATE - Show form to create a new category
        public IActionResult Create()
        {
            return View();
        }

        // POST CREATE - Save new category to the database
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // EDIT - Show form to edit an existing category
        public IActionResult Edit(int categoryId)  // Changed id to categoryId
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);  // Changed to CategoryId
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST EDIT - Update the category in the database
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // DELETE - Show the confirmation page to delete the category
        public IActionResult Delete(int categoryId)  // Changed id to categoryId
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);  // Changed to CategoryId
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST DELETE - Remove the category from the database
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int categoryId)  // Changed id to categoryId
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);  // Changed to CategoryId
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
