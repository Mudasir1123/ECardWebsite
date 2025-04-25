using ECardWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECardWebsite.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // INDEX - Show all transactions
        public IActionResult Index()
        {
            var transactions = _context.Transactions.Include(t => t.User).Include(t => t.Template).ToList();
            return View(transactions);
        }

        // CREATE - Show form to create a new transaction
        public IActionResult Create()
        {
            ViewBag.Users = _context.Users.ToList();
            ViewBag.Templates = _context.ECardTemplates.ToList();
            return View();
        }

        // POST CREATE - Save new transaction to the database
        [HttpPost]
        public IActionResult Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Transactions.Add(transaction);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        // EDIT - Show form to edit an existing transaction
        public IActionResult Edit(int transactionId)  // Changed from 'id' to 'transactionId'
        {
            var transaction = _context.Transactions.FirstOrDefault(t => t.TransactionId == transactionId);  // Changed 'Id' to 'TransactionId'
            if (transaction == null)
            {
                return NotFound();
            }
            ViewBag.Users = _context.Users.ToList();
            ViewBag.Templates = _context.ECardTemplates.ToList();
            return View(transaction);
        }

        // POST EDIT - Update the transaction in the database
        [HttpPost]
        public IActionResult Edit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Transactions.Update(transaction);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        // DELETE - Show the confirmation page to delete the transaction
        public IActionResult Delete(int transactionId)  // Changed from 'id' to 'transactionId'
        {
            var transaction = _context.Transactions.FirstOrDefault(t => t.TransactionId == transactionId);  // Changed 'Id' to 'TransactionId'
            if (transaction == null)
            {
                return NotFound();
            }
            return View(transaction);
        }

        // POST DELETE - Remove the transaction from the database
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int transactionId)  // Changed from 'id' to 'transactionId'
        {
            var transaction = _context.Transactions.FirstOrDefault(t => t.TransactionId == transactionId);  // Changed 'Id' to 'TransactionId'
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
