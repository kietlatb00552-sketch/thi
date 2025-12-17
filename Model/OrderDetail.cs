using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Azure.Core.HttpHeader;

namespace ASM_C_6_API.Model
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        [Required]
        public int OrderId { get; set; }

        // Có thể là FoodItem hoặc Combo
        public int? FoodItemId { get; set; }
        public int? ComboId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        // Navigation
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("FoodItemId")]
        public virtual FoodItem FoodItem { get; set; }

        [ForeignKey("ComboId")]
        public virtual Combo Combo { get; set; }
    }
}
