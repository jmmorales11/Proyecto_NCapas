using Entities;
using ProyectoProxyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service;

namespace Proyecto.MVCPLS.Controllers
{
    public class CategoryController : Controller
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

                return View("ProductList", products);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View("Error");
            }

        }


        [HttpGet]
        // GET: Category/List
        public ActionResult List()
        {
            try
            {
                var proxy = new Proxy();

                var categories = proxy.GetCategory();

                if (categories == null || !categories.Any())
                {
                    ViewBag.Message = "No hay categorías disponibles.";
                }

                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                ViewBag.ErrorMessage = TempData["ErrorMessage"];

                return View(categories);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View("Error");
            }
        }


        // Acción para obtener la categoría por ID (mostrar el formulario de edición)
        public ActionResult Edit(int id)
        {
            var proxy = new Proxy();

            var category = proxy.RetrieveCategoryByID(id); 
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category); 
        }

        // Acción para actualizar la categoría
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            var proxy = new Proxy();

            if (ModelState.IsValid)
            {
                bool result = proxy.UpdateCategory(category); 
                if (result)
                {
                    TempData["SuccessMessage"] = "Categoría actualizada exitosamente.";
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError("", "Error al actualizar la categoría.");
                }
            }
            return View(category); 
        }


        // Acción para eliminar la categoría por ID
        public ActionResult Delete(int id)
        {
            var proxy = new Proxy();

            var category = proxy.RetrieveCategoryByID(id); 
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category); 
        }

        // Acción para confirmar la eliminación
        [HttpGet]
        public ActionResult DeleteConfirmed(int id)
        {
            var proxy = new Proxy();

            bool result = proxy.DeleteCategory(id); 
            if (result)
            {
                TempData["SuccessMessage"] = "Categoría eliminada exitosamente";
                return RedirectToAction("List"); 
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo eliminar la categoría.";
                return RedirectToAction("List"); 
            }
        }

        // Acción para mostrar el formulario de creación
        public ActionResult Create()
        {
            return View(); 
        }

        // Acción para guardar la nueva categoría
        [HttpPost]
        public ActionResult Create(Category newCategory)
        {
            var proxy = new Proxy();

            if (ModelState.IsValid)
            {
                var createdCategory = proxy.CreateCategory(newCategory); 
                if (createdCategory != null)
                {
                    TempData["SuccessMessage"] = "Categoría guardada exitosamente.";
                    return RedirectToAction("List"); 
                }
                else
                {
                    ModelState.AddModelError("", "Error al crear la categoría.");
                }
            }
            return View(newCategory); 
        }


    }
}