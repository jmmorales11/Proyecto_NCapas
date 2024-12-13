using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Categories
    {
        public Category Create(Category newCategory)
        {
            Category Result = null;
            using (var r = RepositoryFactory.CreateRepository())
            {
                // Verificar si la categoría ya existe
                Category res = r.Retrieve<Category>(c => c.CategoryName == newCategory.CategoryName);
                if (res == null)
                {
                    // Crear la categoría
                    Result = r.Create(newCategory);
                }
                else
                {
                    // Se podría lanzar una excepción indicando que la categoría ya existe
                }
            }
            return Result;
        }

        public Category RetrieveByID(int ID)
        {
            Category Result = null;
            using (var r = RepositoryFactory.CreateRepository())
            {
                Result = r.Retrieve<Category>(c => c.CategoryID == ID);
            }
            return Result;
        }

        public bool Update(Category categoryToUpdate)
        {
            bool Result = false;
            using (var r = RepositoryFactory.CreateRepository())
            {
                // Validar que el nombre de la categoría no exista ya
                Category temp = r.Retrieve<Category>(c =>
                    c.CategoryName == categoryToUpdate.CategoryName &&
                    c.CategoryID != categoryToUpdate.CategoryID);
                if (temp == null)
                {
                    Result = r.Update(categoryToUpdate);
                }
                else
                {
                    // Implementar lógica para notificar que no se puede actualizar
                }
            }
            return Result;
        }

        public bool Delete(int ID)
        {
            bool Result = false;
            var category = RetrieveByID(ID);
            if (category != null)
            {
                using (var r = RepositoryFactory.CreateRepository())
                {
                    Result = r.Delete(category);
                }
            }
            else
            {
                // Implementar lógica para indicar que la categoría no existe
            }
            return Result;
        }

        public List<Category> GetAllCategories()
        {
            List<Category> categories = null;
            using (var r = RepositoryFactory.CreateRepository())
            {
                // Recuperar todos los usuarios
                categories = r.GetAll<Category>();
            }
            return categories;
        }

        public List<Product> GetAllProduct()
        {
            List<Product> products = null;
            using (var r = RepositoryFactory.CreateRepository())
            {
                // Recuperar todos los usuarios
                products = r.GetAll<Product>();
            }
            return products;
        }
    }
}
