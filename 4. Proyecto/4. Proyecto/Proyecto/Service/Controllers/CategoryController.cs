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
        public Category CreateCategory(Category newCategory)
        {
            var BL = new Categories();
            var NewCategories = BL.Create(newCategory);
            return NewCategories;
        }

        [HttpGet]
        [Route("delete-category/{id}")]
        public bool DeleteCategory(int id)
        {
            var BL = new Categories();
            var NewCategories = BL.Delete(id);
            return NewCategories;
        }

        [HttpGet]
        [Route("filter-category")]
        public List<Category> GetCategory()
        {
            var BL = new Categories();
            var result = BL.GetAllCategories();
            // Enviar un correo después de obtener los usuarios
            string recipientEmail = "jmmorales11@espe.edu.ec"; // Reemplaza con el correo del destinatario
            string subject = "Listado de Usuarios";
            string body = $"Se ha solicitado el listado de usuarios. Total de usuarios: {result.Count}.";

            // Llama al servicio de correo electrónico
            try
            {
                _emailService.SendEmailAsync(recipientEmail, subject, body).Wait();
            }
            catch (Exception ex)
            {
                // Manejo de errores si el envío falla
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
            }
            return result;
        }

        [HttpGet]
        [Route("retrieve-category/{id}")]
        public Category RetrieveCategoryByID(int id)
        {
            var BL = new Categories();
            var result = BL.RetrieveByID(id);
            return result;
        }

        [HttpPost]
        [Route("update-category")]
        public bool UpdateCategory(Category CategoryToUpdate)
        {
            var BL = new Categories();
            var result = BL.Update(CategoryToUpdate);
            return result;
        }
    }
}
