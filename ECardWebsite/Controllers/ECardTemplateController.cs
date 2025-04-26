using ECardWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index()
        {
            var templates = await _context.ECardTemplates
                .Include(t => t.Category)
                .ToListAsync();
            return View(templates);
        }

        // CREATE - Show form to create a new eCard template
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.Categories = await _context.Categories.ToListAsync() ?? new List<Category>();
                return View();
            }
            catch (Exception ex)
            {
                // Log the error (in production, use a logging framework like Serilog)
                ModelState.AddModelError("", "Unable to load categories. Please try again later.");
                ViewBag.Categories = new List<Category>();
                return View();
            }
        }

        // POST CREATE - Save new eCard template to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ECardTemplate template)
        {
            if (ModelState.IsValid)
            {
                if (template.CategoryId == 0)
                {
                    ModelState.AddModelError("CategoryId", "Please select a valid category.");
                    ViewBag.Categories = await _context.Categories.ToListAsync() ?? new List<Category>();
                    return View(template);
                }

                if (await _context.ECardTemplates.AnyAsync(t => t.Title == template.Title))
                {
                    ModelState.AddModelError("Title", "An eCard template with this title already exists.");
                    ViewBag.Categories = await _context.Categories.ToListAsync() ?? new List<Category>();
                    return View(template);
                }

                if (!await _context.Categories.AnyAsync(c => c.CategoryId == template.CategoryId))
                {
                    ModelState.AddModelError("CategoryId", "Selected category does not exist.");
                    ViewBag.Categories = await _context.Categories.ToListAsync() ?? new List<Category>();
                    return View(template);
                }

                template.CreatedAt = DateTime.Now;
                _context.ECardTemplates.Add(template);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home"); // Redirect to /Home/Index
            }

            ViewBag.Categories = await _context.Categories.ToListAsync() ?? new List<Category>();
            return View(template);
        }

        // EDIT - Show form to edit an existing eCard template
        public async Task<IActionResult> Edit(int templateId)
        {
            var template = await _context.ECardTemplates
                .FirstOrDefaultAsync(t => t.TemplateId == templateId);
            if (template == null)
            {
                return NotFound();
            }
            ViewBag.Categories = await _context.Categories.ToListAsync() ?? new List<Category>();
            return View(template);
        }

        // POST EDIT - Update the eCard template in the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ECardTemplate template)
        {
            if (ModelState.IsValid)
            {
                if (template.CategoryId == 0)
                {
                    ModelState.AddModelError("CategoryId", "Please select a valid category.");
                    ViewBag.Categories = await _context.Categories.ToListAsync() ?? new List<Category>();
                    return View(template);
                }

                if (await _context.ECardTemplates.AnyAsync(t => t.Title == template.Title && t.TemplateId != template.TemplateId))
                {
                    ModelState.AddModelError("Title", "An eCard template with this title already exists.");
                    ViewBag.Categories = await _context.Categories.ToListAsync() ?? new List<Category>();
                    return View(template);
                }

                if (!await _context.Categories.AnyAsync(c => c.CategoryId == template.CategoryId))
                {
                    ModelState.AddModelError("CategoryId", "Selected category does not exist.");
                    ViewBag.Categories = await _context.Categories.ToListAsync() ?? new List<Category>();
                    return View(template);
                }

                var existingTemplate = await _context.ECardTemplates
                    .FirstOrDefaultAsync(t => t.TemplateId == template.TemplateId);
                if (existingTemplate == null)
                {
                    return NotFound();
                }

                existingTemplate.Title = template.Title;
                existingTemplate.Description = template.Description;
                existingTemplate.ImageUrl = template.ImageUrl;
                existingTemplate.CategoryId = template.CategoryId;
                existingTemplate.CreatedAt = template.CreatedAt;

                _context.ECardTemplates.Update(existingTemplate);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home"); // Redirect to /Home/Index
            }

            ViewBag.Categories = await _context.Categories.ToListAsync() ?? new List<Category>();
            return View(template);
        }

        // DELETE - Show the confirmation page to delete the eCard template
        public async Task<IActionResult> Delete(int templateId)
        {
            var template = await _context.ECardTemplates
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.TemplateId == templateId);
            if (template == null)
            {
                return NotFound();
            }
            return View(template);
        }

        // POST DELETE - Remove the eCard template from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int templateId)
        {
            var template = await _context.ECardTemplates
                .FirstOrDefaultAsync(t => t.TemplateId == templateId);
            if (template != null)
            {
                _context.ECardTemplates.Remove(template);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home"); // Redirect to /Home/Index
        }
    }
}