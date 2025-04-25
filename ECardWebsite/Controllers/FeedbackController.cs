using ECardWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECardWebsite.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        // INDEX - Show all feedbacks
        public IActionResult Index()
        {
            var feedbacks = _context.Feedbacks.Include(f => f.User).ToList();
            return View(feedbacks);
        }

        // CREATE - Show form to create new feedback
        public IActionResult Create()
        {
            return View();
        }

        // POST CREATE - Save new feedback to the database
        [HttpPost]
        public IActionResult Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Feedbacks.Add(feedback);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(feedback);
        }

        // EDIT - Show form to edit an existing feedback
        public IActionResult Edit(int feedbackId)  // Changed from 'id' to 'feedbackId'
        {
            var feedback = _context.Feedbacks.FirstOrDefault(f => f.FeedbackId == feedbackId);  // Changed 'Id' to 'FeedbackId'
            if (feedback == null)
            {
                return NotFound();
            }
            return View(feedback);
        }

        // POST EDIT - Update the feedback in the database
        [HttpPost]
        public IActionResult Edit(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Feedbacks.Update(feedback);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(feedback);
        }

        // DELETE - Show the confirmation page to delete the feedback
        public IActionResult Delete(int feedbackId)  // Changed from 'id' to 'feedbackId'
        {
            var feedback = _context.Feedbacks.FirstOrDefault(f => f.FeedbackId == feedbackId);  // Changed 'Id' to 'FeedbackId'
            if (feedback == null)
            {
                return NotFound();
            }
            return View(feedback);
        }

        // POST DELETE - Remove the feedback from the database
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int feedbackId)  // Changed from 'id' to 'feedbackId'
        {
            var feedback = _context.Feedbacks.FirstOrDefault(f => f.FeedbackId == feedbackId);  // Changed 'Id' to 'FeedbackId'
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
