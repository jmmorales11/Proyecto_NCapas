using BLL;
using Common.Interfaces;
using Entities;
using SLC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static Service.WebApiConfig;

namespace Service.Controllers
{
    [RoutePrefix("category")]

    public class CategoryController : ApiController, ICategory
    {
        private readonly IEmailService _emailService;
        public CategoryController()
        {
            _emailService = new EmailService(); // Inicialización manual
        }

        [HttpPost]
        [RoleAuthorize("Admin", "Editor")]
        [Route("create-category")]
<<<<<<< HEAD
        [AllowAnonymous]
=======
>>>>>>> 33144d7683630ae9432d0686e5dcc123a8bd1d29
        public Category CreateCategory(Category newCategory)
        {
            var BL = new Categories();
            var NewCategories = BL.Create(newCategory);
            return NewCategories;
        }

        [HttpGet]
<<<<<<< HEAD
=======
        [RoleAuthorize("Admin")]
>>>>>>> 33144d7683630ae9432d0686e5dcc123a8bd1d29
        [Route("delete-category/{id}")]
        public bool DeleteCategory(int id)
        {
            var BL = new Categories();
            var NewCategories = BL.Delete(id);
            return NewCategories;
        }

        [HttpGet]
<<<<<<< HEAD
=======
        [RoleAuthorize("Admin", "Viewer")]
>>>>>>> 33144d7683630ae9432d0686e5dcc123a8bd1d29
        [Route("filter-category")]
        [AllowAnonymous]
        public List<Category> GetCategory()
        {
            var BL = new Categories();
            var result = BL.GetAllCategories();
            return result;
        }

        [HttpGet]
<<<<<<< HEAD
=======
        [RoleAuthorize("Admin", "Viewer")]
>>>>>>> 33144d7683630ae9432d0686e5dcc123a8bd1d29
        [Route("retrieve-category/{id}")]
        public Category RetrieveCategoryByID(int id)
        {
            var BL = new Categories();
            var result = BL.RetrieveByID(id);
            return result;
        }

        [HttpPost]
<<<<<<< HEAD

=======
        [RoleAuthorize("Admin", "Editor")]
>>>>>>> 33144d7683630ae9432d0686e5dcc123a8bd1d29
        [Route("update-category")]
        public bool UpdateCategory(Category CategoryToUpdate)
        {
            var BL = new Categories();
            var result = BL.Update(CategoryToUpdate);
            return result;
        }
    }
}
