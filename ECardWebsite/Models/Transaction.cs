
namespace ECardWebsite.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int TemplateId { get; set; }
        public ECardTemplate Template { get; set; }

        public string RecipientEmail { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;
    }

}
