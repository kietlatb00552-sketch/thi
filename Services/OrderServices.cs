using ASM_C_6_API.Model;
using ASM_C_6_API.Data;

namespace ASM_C_6_API.Services
{
    public interface IOrderServices
    {
        List<Order> GetAllOrders();
        List<Order> GetOrdersByUserId(int userId);
        List<OrderDetail> GetOrderDetailsByOrderId(int orderId);
        Order GetOrderById(int id);
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(int id);
    }
    public class OrderServices : IOrderServices
    {
        private readonly ApplicationDbContext _context;
        public OrderServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Order> GetAllOrders()
        {
            return _context.Orders.ToList();
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            return _context.Orders.Where(o => o.UserId == userId).ToList();
        }

        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            return _context.OrderDetails.Where(od => od.OrderId == orderId).ToList();
        }

        public Order GetOrderById(int id)
        {
            var order = _context.Orders.Find(id);
            if(order == null)
            {
                throw new Exception("Order not found");
            }
            return order;
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            var existingOrder = _context.Orders.Find(order.OrderId);
            if(existingOrder == null)
            {
                throw new Exception("Order not found");
            }
            existingOrder.UserId = order.UserId;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.TotalAmount = order.TotalAmount;
            existingOrder.Status = order.Status;
            _context.SaveChanges();
        }

        public void DeleteOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if(order == null)
            {
                throw new Exception("Order not found");
            }
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
    }
}
