namespace ASM_C_6_API.Model
{
    public enum RoleType
    {
        Admin,
        User,
        Guest
    }
    public class Role
    {
        public int RoleId { get; set; }
        public string? Name { get; set; }

        // Navigation
        public virtual ICollection<User>? Users { get; set; }
    }
}
