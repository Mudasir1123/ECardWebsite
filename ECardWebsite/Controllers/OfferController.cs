using ECardWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECardWebsite.Controllers
{
    public class OfferController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OfferController(ApplicationDbContext context)
        {
            _context = context;
        }

        // INDEX - Show all offers
        public IActionResult Index()
        {
            var offers = _context.Offers.ToList();
            return View(offers);
        }

        // CREATE - Show form to create a new offer
        public IActionResult Create()
        {
            return View();
        }

        // POST CREATE - Save new offer to the database
        [HttpPost]
        public IActionResult Create(Offer offer)
        {
            if (ModelState.IsValid)
            {
                _context.Offers.Add(offer);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(offer);
        }

        // EDIT - Show form to edit an existing offer
        public IActionResult Edit(int offerId)  // Changed from 'id' to 'offerId'
        {
            var offer = _context.Offers.FirstOrDefault(o => o.OfferId == offerId);  // Changed 'Id' to 'OfferId'
            if (offer == null)
            {
                return NotFound();
            }
            return View(offer);
        }

        // POST EDIT - Update the offer in the database
        [HttpPost]
        public IActionResult Edit(Offer offer)
        {
            if (ModelState.IsValid)
            {
                _context.Offers.Update(offer);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(offer);
        }

        // DELETE - Show the confirmation page to delete the offer
        public IActionResult Delete(int offerId)  // Changed from 'id' to 'offerId'
        {
            var offer = _context.Offers.FirstOrDefault(o => o.OfferId == offerId);  // Changed 'Id' to 'OfferId'
            if (offer == null)
            {
                return NotFound();
            }
            return View(offer);
        }

        // POST DELETE - Remove the offer from the database
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int offerId)  // Changed from 'id' to 'offerId'
        {
            var offer = _context.Offers.FirstOrDefault(o => o.OfferId == offerId);  // Changed 'Id' to 'OfferId'
            if (offer != null)
            {
                _context.Offers.Remove(offer);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
