using System.ComponentModel.DataAnnotations;

namespace ASM_C_6_API.Model
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(200)]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }

        // Navigation
        public virtual ICollection<FoodItem> FoodItems { get; set; }
    }
}
