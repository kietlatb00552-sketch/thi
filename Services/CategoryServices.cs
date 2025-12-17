using ASM_C_6_API.Data;
using ASM_C_6_API.Model;

namespace ASM_C_6_API.Services
{
    public interface ICategoryServices
    {
        List<Category> GetAllCategories();
        Category GetCategoryById(int id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);
    }
    public class CategoryServices : ICategoryServices
    {
        private readonly ApplicationDbContext _context;
        public CategoryServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategoryById(int id)
        {
            var category = _context.Categories.Find(id);
            if(category == null)
            {
                throw new Exception("Category not found");
            }
            return category;
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            var existingCategory = _context.Categories.Find(category.CategoryId);
            if(existingCategory == null)
            {
                throw new Exception("Category not found");
            }
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.Description = category.Description;
            _context.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if(category == null)
            {
                throw new Exception("Category not found");
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}
