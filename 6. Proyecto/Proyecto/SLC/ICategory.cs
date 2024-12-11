using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLC
{
    public interface ICategory
    {
        Category CreateCategory(Category newCategory);
        Category RetrieveCategoryByID(int ID);
        bool UpdateCategory(Category CategoryToUpdate);
        bool DeleteCategory(int ID);
        List<Category> GetCategory();
    }
}
