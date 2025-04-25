using ECardWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECardWebsite.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // INDEX - Show all subscriptions
        public IActionResult Index()
        {
            var subscriptions = _context.Subscriptions.Include(s => s.User).Include(s => s.Offer).ToList();
            return View(subscriptions);
        }

        // CREATE - Show form to create a new subscription
        public IActionResult Create()
        {
            ViewBag.Users = _context.Users.ToList();
            ViewBag.Offers = _context.Offers.ToList();
            return View();
        }

        // POST CREATE - Save new subscription to the database
        [HttpPost]
        public IActionResult Create(Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                _context.Subscriptions.Add(subscription);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(subscription);
        }

        // EDIT - Show form to edit an existing subscription
        public IActionResult Edit(int subscriptionId)  // Changed from 'id' to 'subscriptionId'
        {
            var subscription = _context.Subscriptions.FirstOrDefault(s => s.SubscriptionId == subscriptionId);  // Changed 'Id' to 'SubscriptionId'
            if (subscription == null)
            {
                return NotFound();
            }
            ViewBag.Users = _context.Users.ToList();
            ViewBag.Offers = _context.Offers.ToList();
            return View(subscription);
        }

        // POST EDIT - Update the subscription in the database
        [HttpPost]
        public IActionResult Edit(Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                _context.Subscriptions.Update(subscription);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(subscription);
        }

        // DELETE - Show the confirmation page to delete the subscription
        public IActionResult Delete(int subscriptionId)  // Changed from 'id' to 'subscriptionId'
        {
            var subscription = _context.Subscriptions.FirstOrDefault(s => s.SubscriptionId == subscriptionId);  // Changed 'Id' to 'SubscriptionId'
            if (subscription == null)
            {
                return NotFound();
            }
            return View(subscription);
        }

        // POST DELETE - Remove the subscription from the database
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int subscriptionId)  // Changed from 'id' to 'subscriptionId'
        {
            var subscription = _context.Subscriptions.FirstOrDefault(s => s.SubscriptionId == subscriptionId);  // Changed 'Id' to 'SubscriptionId'
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
