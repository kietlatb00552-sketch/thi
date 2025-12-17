using ASM_C_6_API.Model;
using ASM_C_6_API.Data;

namespace ASM_C_6_API.Services
{
    public interface ICartItemServices
    {
        List<CartItem> GetAllCartItems();
        void AddCartItem(CartItem cartItem, int cartId);
        void UpdateCartItem(CartItem cartItem);
        void RemoveCartItem(CartItem cartItem);
    }
    public class CartItemServices : ICartItemServices
    {
        private readonly ApplicationDbContext _context;
        public CartItemServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<CartItem> GetAllCartItems()
        {
            return _context.CartItems.ToList();
        }
        public void AddCartItem(CartItem cartItem, int cartId)
        {
            cartItem.CartId = cartId;
            _context.CartItems.Add(cartItem);
            _context.SaveChanges();
        }
        public void UpdateCartItem(CartItem cartItem)
        {
            var existingCartItem = _context.CartItems.Find(cartItem.CartItemId);
            if(existingCartItem == null)
            {
                throw new Exception("Cart item not found");
            }
            existingCartItem.Quantity = cartItem.Quantity;
            _context.SaveChanges();
        }
        public void RemoveCartItem(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
        }
    }
}
