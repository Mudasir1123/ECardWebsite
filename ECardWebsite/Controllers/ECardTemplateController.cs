using ECardWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECardWebsite.Controllers
{
    public class ECardTemplateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ECardTemplateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // INDEX - Show all eCard templates
        public IActionResult Index()
        {
            var templates = _context.ECardTemplates.Include(t => t.Category).ToList();
            return View(templates);
        }

        // CREATE - Show form to create a new eCard template
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList(); // Get categories for dropdown
            return View();
        }

        // POST CREATE - Save new eCard template to the database
        [HttpPost]
        public IActionResult Create(ECardTemplate template)
        {
            if (ModelState.IsValid)
            {
                _context.ECardTemplates.Add(template);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(template);
        }

        // EDIT - Show form to edit an existing eCard template
        public IActionResult Edit(int templateId)  // Changed from 'id' to 'templateId'
        {
            var template = _context.ECardTemplates.FirstOrDefault(t => t.TemplateId == templateId);  // Changed 'Id' to 'TemplateId'
            if (template == null)
            {
                return NotFound();
            }
            ViewBag.Categories = _context.Categories.ToList(); // Get categories for dropdown
            return View(template);
        }

        // POST EDIT - Update the eCard template in the database
        [HttpPost]
        public IActionResult Edit(ECardTemplate template)
        {
            if (ModelState.IsValid)
            {
                _context.ECardTemplates.Update(template);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(template);
        }

        // DELETE - Show the confirmation page to delete the eCard template
        public IActionResult Delete(int templateId)  // Changed from 'id' to 'templateId'
        {
            var template = _context.ECardTemplates.FirstOrDefault(t => t.TemplateId == templateId);  // Changed 'Id' to 'TemplateId'
            if (template == null)
            {
                return NotFound();
            }
            return View(template);
        }

        // POST DELETE - Remove the eCard template from the database
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int templateId)  // Changed from 'id' to 'templateId'
        {
            var template = _context.ECardTemplates.FirstOrDefault(t => t.TemplateId == templateId);  // Changed 'Id' to 'TemplateId'
            if (template != null)
            {
                _context.ECardTemplates.Remove(template);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
