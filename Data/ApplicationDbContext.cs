using Microsoft.EntityFrameworkCore;
using ASM_C_6_API.Model;

namespace ASM_C_6_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<ComboItem> ComboItems { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Additional configuration can be added here if needed
            builder.Entity<Category>()
                .Property(c => c.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Entity<FoodItem>()
                .Property(f => f.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Entity<Combo>()
                .Property(c => c.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Entity<Order>()
                .Property(o => o.OrderDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Entity<FoodItem>()
                .HasOne(f => f.Category)
                .WithMany(c => c.FoodItems)
                .HasForeignKey(f => f.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.FoodItem)
                .WithMany()
                .HasForeignKey(ci => ci.FoodItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.Combo)
                .WithMany()
                .HasForeignKey(ci => ci.ComboId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrderDetail>()
                .Property(od => od.TotalPrice)
                .HasComputedColumnSql("[Quantity] * [UnitPrice]");

            builder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Role>()
                .HasData(new Role { RoleId = 1, Name = RoleType.Admin.ToString() },
                         new Role { RoleId = 2, Name = RoleType.User.ToString() },
                         new Role { RoleId = 3, Name = RoleType.Guest.ToString() });

            builder.Entity<User>()
                .HasData(new User
                {
                    UserId = 1,
                    Username = "admin",
                    PasswordHash = "AQAAAAEAACcQNAAAAEJv1y7sX5G6k8jvY9Vh3F4Z2x5Y6Z7X8Y9Z0A1B2C3D4E5F6G7H8I9J0K1L2M3N4O5P6Q7R8S9T0U1V2W3X4Y5Z6",
                    FirstName = "System",
                    LastName = "Administrator",
                    Email = "Nguyendinhnam24109@gmail.com",
                    PhoneNumber = "0123456789",
                    RoleId = 1,
                    RefreshToken = string.Empty
                }, new User
                {
                    UserId = 2,
                    Username = "user",
                    PasswordHash = "AQAAAAEAACcQNAAAAEJv1y7sX5G6k8jvY9Vh3F4Z2x5Y6Z7X8Y9Z0A1B2C3D4E5F6G7H8I9J0K1L2M3N4O5P6Q7R8S9T0U1V2W3X4Y5Z6",
                    FirstName = "Regular",
                    LastName = "User",
                    Email = "nguyendinhnam288@gmail.com",
                    PhoneNumber = "0987654321",
                    RoleId = 2,
                    RefreshToken = string.Empty
                });

            var now = new DateTime(2025, 11, 24, 0, 0, 0, DateTimeKind.Utc);

            builder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Rice Dishes",
                    Description = "Cơm và các món ăn kèm truyền thống",
                    ImageUrl = "/images/categories/rice.jpg",
                    IsActive = true,
                    CreatedDate = now
                },
                new Category
                {
                    CategoryId = 2,
                    CategoryName = "Noodles",
                    Description = "Phở, mì và các món bún",
                    ImageUrl = "/images/categories/noodles.jpg",
                    IsActive = true,
                    CreatedDate = now
                },
                new Category
                {
                    CategoryId = 3,
                    CategoryName = "Drinks",
                    Description = "Nước giải khát: trà, nước ngọt, nước trái cây",
                    ImageUrl = "/images/categories/drinks.jpg",
                    IsActive = true,
                    CreatedDate = now
                });

            builder.Entity<FoodItem>().HasData(
                new FoodItem
                {
                    FoodItemId = 1,
                    FoodName = "Grilled Chicken with Rice",
                    Description = "Gà nướng sốt đặc biệt phục vụ cùng cơm trắng",
                    Price = 6.99m,
                    ImageUrl = "/images/food/grilled_chicken.jpg",
                    Theme = "Savory",
                    IsAvailable = true,
                    CreatedDate = now,
                    CategoryId = 1
                },
                new FoodItem
                {
                    FoodItemId = 2,
                    FoodName = "Beef Pho",
                    Description = "Phở bò thơm ngon với nước dùng đậm đà",
                    Price = 5.50m,
                    ImageUrl = "/images/food/beef_pho.jpg",
                    Theme = "Traditional",
                    IsAvailable = true,
                    CreatedDate = now,
                    CategoryId = 2
                },
                new FoodItem
                {
                    FoodItemId = 3,
                    FoodName = "Iced Tea",
                    Description = "Trà đá mát lạnh",
                    Price = 1.50m,
                    ImageUrl = "/images/food/iced_tea.jpg",
                    Theme = "Refreshing",
                    IsAvailable = true,
                    CreatedDate = now,
                    CategoryId = 3
                });

            builder.Entity<Combo>().HasData(
                new Combo
                {
                    ComboId = 1,
                    ComboName = "Lunch Combo",
                    Description = "Gồm 1 cơm gà + 1 trà đá",
                    Price = 8.99m,
                    DiscountPrice = 7.99m,
                    ImageUrl = "/images/combos/lunch_combo.jpg",
                    IsAvailable = true,
                    CreatedDate = now
                });

            builder.Entity<ComboItem>().HasData(
                new ComboItem
                {
                    ComboItemId = 1,
                    ComboId = 1,
                    FoodItemId = 1,
                    Quantity = 1
                },
                new ComboItem
                {
                    ComboItemId = 2,
                    ComboId = 1,
                    FoodItemId = 3,
                    Quantity = 1
                });

            // Seed a Cart for existing user (UserId = 2)
            builder.Entity<Cart>().HasData(
                new Cart
                {
                    CartId = 1,
                    UserId = 2,
                    CreatedDate = now,
                    UpdatedDate = now
                });

            // Seed a CartItem referencing a FoodItem in the cart
            builder.Entity<CartItem>().HasData(
                new CartItem
                {
                    CartItemId = 1,
                    CartId = 1,
                    FoodItemId = 1,
                    ComboId = null,
                    Quantity = 1,
                    UnitPrice = 6.99m,
                    AddedDate = now
                });

            // Seed an Order and OrderDetail (order placed by UserId = 2)
            builder.Entity<Order>().HasData(
                new Order
                {
                    OrderId = 1,
                    UserId = 2,
                    OrderDate = now,
                    TotalAmount = 6.99m,
                    Status = (OrderStatus)0,
                    PaymentMethod = (PaymentMethod)0,
                    IsPaid = false,
                    DeliveryAddress = "123 Example Street",
                    PhoneNumber = "0987654321",
                    ReceiverName = "John Doe",
                    Note = "Giao giờ hành chính",
                    PaymentUrl = null,
                    DeliveredDate = null
                });

            builder.Entity<OrderDetail>().HasData(
                new OrderDetail
                {
                    OrderDetailId = 1,
                    OrderId = 1,
                    FoodItemId = 1,
                    ComboId = null,
                    Quantity = 1,
                    UnitPrice = 6.99m,
                    // TotalPrice is computed in DB ([Quantity] * [UnitPrice])
                    TotalPrice = 6.99m
                });
        }
    }
}
