using ASM_C_6_API.Model;
using ASM_C_6_API.Data;

namespace ASM_C_6_API.Services
{
    public interface IComboDetailServices
    {
        List<ComboItem> GetAllComboDetails();
        void AddComboDetail(ComboItem comboItem, int comboId);
        void RemoveComboDetail(ComboItem comboItem);
    }
    public class ComboDetailServices : IComboDetailServices
    {
        private readonly ApplicationDbContext _context;
        public ComboDetailServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<ComboItem> GetAllComboDetails()
        {
            return _context.ComboItems.ToList();
        }
        public void AddComboDetail(ComboItem comboItem, int comboId)
        {
            comboItem.ComboId = comboId;
            _context.ComboItems.Add(comboItem);
            _context.SaveChanges();
        }
        public void RemoveComboDetail(ComboItem comboItem)
        {
            _context.ComboItems.Remove(comboItem);
            _context.SaveChanges();
        }
    }
}
