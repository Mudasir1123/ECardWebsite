using System.ComponentModel.DataAnnotations;

namespace ECardWebsite.Models
{
    public class Offer
    {
        public int OfferId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal DiscountPercentage { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Optional: Offers linked to subscriptions
        public ICollection<Subscription> Subscriptions { get; set; }
    }

}
