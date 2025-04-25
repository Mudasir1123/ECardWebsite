using System.ComponentModel.DataAnnotations;

namespace ECardWebsite.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }

}
