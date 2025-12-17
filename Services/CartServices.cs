using ASM_C_6_API.Model;
using ASM_C_6_API.Data;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace ASM_C_6_API.Services
{
    public interface ICartServices
    {
        List<Cart> GetAllCarts();
        Cart GetCartById(int id);
        Cart GetCartsByUserId(int userId);
        void AddCart(Cart cart);
        void UpdateCart(Cart cart);
        void DeleteCart(int id);
    }
    public class CartServices : ICartServices
    {
        private readonly ApplicationDbContext _context;
        public CartServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Cart> GetAllCarts()
        {
            return _context.Carts.ToList();
        }

        public Cart GetCartById(int id)
        {
            var cart = _context.Carts.Find(id);
            if(cart == null)
            {
                throw new Exception("Cart not found");
            }
            return cart;
        }

        public Cart GetCartsByUserId(int userId)
        {
            var userCart = _context.Carts
                            .Include(c => c.CartItems)
                                .ThenInclude(ci => ci.FoodItem)
                            .Include(c => c.CartItems)
                                .ThenInclude(ci => ci.Combo)
                            .FirstOrDefault(c => c.UserId == userId);
            if (userCart == null)
            {
                return null; 
            }
            return userCart;
        }

        public void AddCart(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        public void UpdateCart(Cart cart)
        {
            var existingCart = _context.Carts.Find(cart.CartId);
            if(existingCart == null)
            {
                throw new Exception("Cart not found");
            }
            existingCart.UserId = cart.UserId;
            _context.SaveChanges();
        }

        public void DeleteCart(int id)
        {
            var cart = _context.Carts.Find(id);
            if(cart == null)
            {
                throw new Exception("Cart not found");
            }
            _context.Carts.Remove(cart);
            _context.SaveChanges();
        }
    }
}
