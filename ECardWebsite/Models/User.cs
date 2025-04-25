using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECardWebsite.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [NotMapped] // This ensures we don't try to map this to the database
        public string Password { get; set; }

        public string HashedPassword { get; set; }

        public string Role { get; set; } = "User"; // "Admin" or "User"

        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}