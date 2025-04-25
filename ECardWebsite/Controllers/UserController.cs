using ECardWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace ECardWebsite.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly ILogger<UserController> _logger;

        public UserController(
            ApplicationDbContext context,
            ILogger<UserController> logger)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
            _logger = logger;
        }

        // INDEX - Show list of users
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // CREATE - Show the form to create a new user
        public IActionResult Create()
        {
            return View();
        }

        // POST CREATE - Save new user to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            try
            {
                _logger.LogInformation("Attempting to create new user");

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        _logger.LogWarning("Validation error: {ErrorMessage}", error.ErrorMessage);
                    }
                    return View(user);
                }

                // Check if email already exists
                if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email is already registered");
                    return View(user);
                }

                // Hash the password
                user.HashedPassword = _passwordHasher.HashPassword(user, user.Password);

                // Add to database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User created successfully: {Email}", user.Email);
                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while creating user");
                ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                return View(user);
            }
        }

        // EDIT - Show the form to edit an existing user
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST EDIT - Update the user in the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users.FindAsync(id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    // Check if email is being changed to an existing email
                    if (existingUser.Email != user.Email &&
                        await _context.Users.AnyAsync(u => u.Email == user.Email))
                    {
                        ModelState.AddModelError("Email", "Email is already in use");
                        return View(user);
                    }

                    // Update properties
                    existingUser.Name = user.Name;
                    existingUser.Email = user.Email;
                    existingUser.Role = user.Role;

                    // Only update password if it was provided
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        existingUser.HashedPassword = _passwordHasher.HashPassword(existingUser, user.Password);
                    }

                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "User updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Error updating user");
                    ModelState.AddModelError("", "An error occurred while updating. Please try again.");
                }
            }
            return View(user);
        }

        // DELETE - Show the confirmation page to delete the user
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST DELETE - Remove the user from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "User deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error deleting user");
                TempData["ErrorMessage"] = "An error occurred while deleting. Please try again.";
                return View("Delete", user);
            }
        }
    }
}