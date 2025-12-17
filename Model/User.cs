using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASM_C_6_API.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public int RoleId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public virtual ICollection<Order>? Orders { get; set; }
        public virtual Cart Cart { get; set; }

        [ForeignKey("RoleId")]
        [JsonIgnore]
        public virtual Role Role { get; set; }
    }
}
