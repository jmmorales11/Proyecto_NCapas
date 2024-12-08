using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoProxyService;
using Entities;

namespace Proyecto.MVCPLS.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            // Devuelve la vista inicial del formulario
            return View();
        }

        // POST: Home/Index
        [HttpPost]
        public ActionResult Index(int id)
        {
            try
            {
                // Crear instancia del proxy
                var proxy = new Proxy();

                var products = proxy.FilterProductsByCategoryID(id);
                if (products == null || !products.Any())
                {
                    Console.WriteLine("No se encontraron productos.");
                }
                else
                {
                    foreach (var product in products)
                    {
                        Console.WriteLine($"Producto: {product.ProductName}");
                    }
                }


                // Devolver la vista 'ProductList' con los productos obtenidos
                return View("ProductList", products);
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que ocurra y mostrar un mensaje
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View("Error");
            }

        }
    }
}