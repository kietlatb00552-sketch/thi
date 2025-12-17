using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASM_C_6_API.Model
{
    public class ComboItem
    {
        [Key]
        public int ComboItemId { get; set; }

        [Required]
        public int ComboId { get; set; }

        [Required]
        public int FoodItemId { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Navigation
        // Navigation
        [ForeignKey("ComboId")]
        [JsonIgnore] // tránh serialize parent Combo trong mỗi ComboItem — loại vòng lặp
        public virtual Combo Combo { get; set; }

        [ForeignKey("FoodItemId")]
        public virtual FoodItem FoodItem { get; set; }
    }
}
