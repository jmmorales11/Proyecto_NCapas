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
        [Route("create-category")]
        [Authorize(Roles = "Admin,Editor")]
        [AllowAnonymous]
        public Category CreateCategory(Category newCategory)
        {
            var BL = new Categories();
            var NewCategories = BL.Create(newCategory);
            return NewCategories;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("delete-category/{id}")]
        public bool DeleteCategory(int id)
        {
            var BL = new Categories();
            var NewCategories = BL.Delete(id);
            return NewCategories;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Viewer")]
        [Route("filter-category")]
        [AllowAnonymous]
        public List<Category> GetCategory()
        {
            var BL = new Categories();
            var result = BL.GetAllCategories();
            return result;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Viewer")]
        [Route("retrieve-category/{id}")]
        public Category RetrieveCategoryByID(int id)
        {
            var BL = new Categories();
            var result = BL.RetrieveByID(id);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        [Route("update-category")]
        public bool UpdateCategory(Category CategoryToUpdate)
        {
            var BL = new Categories();
            var result = BL.Update(CategoryToUpdate);
            return result;
        }
    }
}
