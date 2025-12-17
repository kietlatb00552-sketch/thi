using ASM_C_6_API.Data;
using ASM_C_6_API.DTOs;
using ASM_C_6_API.Model;
using ASM_C_6_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASM_C_6_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // GET: api/<AuthController>
        private readonly ApplicationDbContext _context;
        private readonly IJwtServices _jwtServices;

        public AuthController(ApplicationDbContext context, IJwtServices jwtServices)
        {
            _context = context;
            _jwtServices = jwtServices;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(RegisterDTO registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            {
                return BadRequest(new { message = "Username đã tồn tại" });
            }

            // 2. Kiểm tra email đã tồn tại chưa
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return BadRequest(new { message = "Email đã được sử dụng" });
            }

            // 3. Hash password bằng BCrypt
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // 4. Tạo user mới
            var user = new User
            {
                Username = registerDto.Username,
                PasswordHash = hashedPassword,
                Email = registerDto.Email,
                RoleId = 1, // Mặc định là User
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            User userRegister = _context.Users.Include(u => u.Role).FirstOrDefault(user1 => user1.UserId == user.UserId);
            // 5. Tạo JWT token
            var token = _jwtServices.GenerateToken(user);
            var role = _context.Roles.FirstOrDefault(i => i.RoleId == user.RoleId);

            user.RefreshToken = token;
            await _context.SaveChangesAsync();
            // 6. Trả về response
            return Ok(new AuthResponseDTO
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Role = role?.Name,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username || u.Email == loginDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Username hoặc password không đúng" });
            }
            // Tạo JWT token
            var token = _jwtServices.GenerateToken(user);
            var role = _context.Roles.FirstOrDefault(i => i.RoleId == user.RoleId);

            user.RefreshToken = token;
            await _context.SaveChangesAsync();
            // Trả về response
            return Ok(new AuthResponseDTO
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Role = role.Name,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            });
        }

        [HttpGet("GetUserInfo")]
        [Authorize]
        public async Task<ActionResult<User>> GetUserInfo([FromQuery] string token)
        {
            var user = await CheckTokenAsync(token);
            if (user == null)
            {
                return Unauthorized();
            }

            // Remove sensitive data before returning
            user.PasswordHash = string.Empty;
            return Ok(user);
        }

        /// <summary>
        /// Parse JWT without validation to extract NameIdentifier claim and return the corresponding user.
        /// Note: this reads token claims only; if you need signature/lifetime validation, use a proper ValidateToken method.
        /// </summary>
        private async Task<User?> CheckTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            try
            {
                var handler = new JwtSecurityTokenHandler();

                // Read token without validating signature — we only need claims to locate the user.
                var jwt = handler.ReadJwtToken(token);

                // ClaimTypes.NameIdentifier was used when generating the token
                var idClaim = jwt.Claims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.NameIdentifier || c.Type == JwtRegisteredClaimNames.Sub);

                if (idClaim == null)
                    return null;

                if (!int.TryParse(idClaim.Value, out var userId))
                    return null;

                // Load user including Role navigation
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                return user;
            }
            catch
            {
                return null;
            }
        }
    }
}
