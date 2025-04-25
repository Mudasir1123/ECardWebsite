using System.ComponentModel.DataAnnotations;

namespace ECardWebsite.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<ECardTemplate> Templates { get; set; }
    }

}
