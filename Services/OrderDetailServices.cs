using ASM_C_6_API.Model;
using ASM_C_6_API.Data;

namespace ASM_C_6_API.Services
{
    public interface IOrderDetailServices
    {
        List<OrderDetail> GetAllOrderDetails();
        void AddOrderDetail(OrderDetail orderDetail, int orderId);
        void RemoveOrderDetail(OrderDetail orderDetail);
    }
    public class OrderDetailServices : IOrderDetailServices
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<OrderDetail> GetAllOrderDetails()
        {
            return _context.OrderDetails.ToList();
        }
        public void AddOrderDetail(OrderDetail orderDetail, int orderId)
        {
            orderDetail.OrderId = orderId;
            _context.OrderDetails.Add(orderDetail);
            _context.SaveChanges();
        }
        public void RemoveOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Remove(orderDetail);
            _context.SaveChanges();
        }
    }
}
