using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM_C_6_API.Model
{
    public class Combo
    {
        [Key]
        public int ComboId { get; set; }

        [Required]
        [StringLength(200)]
        public string ComboName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountPrice { get; set; }

        [StringLength(200)]
        public string ImageUrl { get; set; }

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedDate { get; set; }

        // Navigation
        public virtual ICollection<ComboItem>? ComboItems { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
