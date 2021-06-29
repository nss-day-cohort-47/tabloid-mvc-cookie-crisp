 using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category GetCategoryById(int id);
        void DeleteCategory(int categoryId);

        void UpdateCategory(Category Category);

        void AddCategory(Category category);
    }
}