using ASM_C_6_API.Model;
using ASM_C_6_API.Data;

namespace ASM_C_6_API.Services
{
    public interface IUserServices
    {
        List<User> GetAllUsers();
        User GetUserById(int id);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _context;
        public UserServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
        public User GetUserById(int id)
        {
            var user = _context.Users.Find(id);
            if(user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }
        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void UpdateUser(User user)
        {
            var existingUser = _context.Users.Find(user.UserId);
            if(existingUser == null)
            {
                throw new Exception("User not found");
            }
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            _context.SaveChanges();
        }
        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if(user == null)
            {
                throw new Exception("User not found");
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
