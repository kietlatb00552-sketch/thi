using ASM_C_6_API.Model;
using ASM_C_6_API.Data;
using Microsoft.EntityFrameworkCore;

namespace ASM_C_6_API.Services
{
    public interface IComboServices
    {
        List<Combo> GetAllCombos();
        List<ComboItem> GetComboItemsByComboId(int comboId);
        Combo GetComboById(int id);
        void AddCombo(Combo combo);
        void UpdateCombo(Combo combo);
        void DeleteCombo(int id);
    }
    public class ComboServices : IComboServices
    {
        private readonly ApplicationDbContext _context;
        public ComboServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Combo> GetAllCombos()
        {
            return _context.Combos.Include(c => c.ComboItems!).ThenInclude(ci => ci.FoodItem)
                           .ToList();
        }

        public List<ComboItem> GetComboItemsByComboId(int comboId)
        {
            return _context.ComboItems.Where(ci => ci.ComboId == comboId).ToList();
        }

        public Combo GetComboById(int id)
        {
            var combo = _context.Combos.Find(id);
            if(combo == null)
            {
                throw new Exception("Combo not found");
            }
            return combo;
        }

        public void AddCombo(Combo combo)
        {
            _context.Combos.Add(combo);
            _context.SaveChanges();
        }

        public void UpdateCombo(Combo combo)
        {
            var existingCombo = _context.Combos.Find(combo.ComboId);
            if(existingCombo == null)
            {
                throw new Exception("Combo not found");
            }
            existingCombo.ComboName = combo.ComboName;
            existingCombo.Description = combo.Description;
            existingCombo.Price = combo.Price;
            _context.SaveChanges();
        }

        public void DeleteCombo(int id)
        {
            var combo = _context.Combos.Find(id);
            if(combo == null)
            {
                throw new Exception("Combo not found");
            }
            _context.Combos.Remove(combo);
            _context.SaveChanges();
        }
    }
}
