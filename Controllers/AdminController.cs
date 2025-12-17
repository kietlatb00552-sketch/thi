using ASM_C_6_API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASM_C_6_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("interview")]
        [Authorize]
        public async Task<IActionResult> GetAdminDashboard()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role.Name == "User")
                .CountAsync();

            var foods = await _context.FoodItems.CountAsync();
            var orders = await _context.Orders.CountAsync();
            var revenue = await _context.Orders.SumAsync(o => o.TotalAmount);

            var statistics = new
            {
                TotalUsers = users,
                TotalFoods = foods,
                TotalOrders = orders,
                TotalRevenue = revenue
            };
            return Ok(statistics);
        }
    }
}
