using System;
using System.ComponentModel.DataAnnotations;

namespace ECardWebsite.Models
{
    public class ECardTemplate
    {
        [Key]
        public int TemplateId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Foreign Key to Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}