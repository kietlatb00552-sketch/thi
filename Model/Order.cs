using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM_C_6_API.Model
{
    public enum OrderStatus
    {
        Pending,      // Chờ xử lý
        Processing,   // Đang chuẩn bị
        Shipping,     // Đang giao
        Delivered,    // Đã giao
        Confirmed,
        Preparing,   // Đang chuẩn bị
        Cancelled     // Đã hủy
    }

    public enum PaymentMethod
    {
        Cash,         // Tiền mặt
        BankTransfer,
    }
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public bool IsPaid { get; set; } = false;

        [Required]
        [StringLength(200)]
        public string DeliveryAddress { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string ReceiverName { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public DateTime? DeliveredDate { get; set; }
        public string? PaymentUrl { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
