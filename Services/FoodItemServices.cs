using ASM_C_6_API.Model;
using ASM_C_6_API.Data;

namespace ASM_C_6_API.Services
{
    public interface IFoodItemServices
    {
        List<FoodItem> GetAllFoodItems();
        FoodItem GetFoodItemById(int id);
        void AddFoodItem(FoodItem foodItem);
        void UpdateFoodItem(FoodItem foodItem);
        void DeleteFoodItem(int id);
    }
    public class FoodItemServices : IFoodItemServices
    {
        private readonly ApplicationDbContext _context;
        public FoodItemServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<FoodItem> GetAllFoodItems()
        {
            return _context.FoodItems.ToList();
        }

        public FoodItem GetFoodItemById(int id)
        {
            var foodItem = _context.FoodItems.Find(id);
            if(foodItem == null)
            {
                throw new Exception("Food item not found");
            }
            return foodItem;
        }

        public void AddFoodItem(FoodItem foodItem)
        {
            _context.FoodItems.Add(foodItem);
            _context.SaveChanges();
        }

        public void UpdateFoodItem(FoodItem foodItem)
        {
            var existingFoodItem = _context.FoodItems.Find(foodItem.FoodItemId);
            if(existingFoodItem == null)
            {
                throw new Exception("Food item not found");
            }
            existingFoodItem.FoodName = foodItem.FoodName;
            existingFoodItem.Description = foodItem.Description;
            existingFoodItem.Price = foodItem.Price;
            existingFoodItem.CategoryId = foodItem.CategoryId;
            _context.SaveChanges();
        }

        public void DeleteFoodItem(int id)
        {
            var foodItem = _context.FoodItems.Find(id);
            if(foodItem == null)
            {
                throw new Exception("Food item not found");
            }
            _context.FoodItems.Remove(foodItem);
            _context.SaveChanges();
        }
    }
}
