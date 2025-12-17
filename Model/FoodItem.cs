using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM_C_6_API.Model
{
    public class FoodItem
    {
        [Key]
        public int FoodItemId { get; set; }

        [Required]
        [StringLength(200)]
        public string FoodName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [StringLength(200)]
        public string? ImageUrl { get; set; }

        [StringLength(100)]
        public string? Theme { get; set; } // Chủ đề (VD: "Spicy", "Vegetarian")

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedDate { get; set; }

        // Foreign Key
        [Required]
        public int CategoryId { get; set; }

        // Navigation
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ComboItem> ComboItems { get; set; }
    }
}
