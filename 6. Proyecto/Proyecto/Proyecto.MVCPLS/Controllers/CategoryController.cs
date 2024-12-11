using Entities;
using ProyectoProxyService;
using SL.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.MVCPLS.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            LogHelper.LogInformation("Iniciando vista de índice.");
            return View();
        }

        // POST: Home/Index
        [HttpPost]
        public ActionResult Index(int id)
        {
            try
            {
                LogHelper.LogInformation($"Consultando productos para la categoría con ID: {id}.");
                var proxy = new Proxy();
                var products = proxy.FilterProductsByCategoryID(id);

                if (products == null || !products.Any())
                {
                    LogHelper.LogWarning("No se encontraron productos.");
                }
                else
                {
                    LogHelper.LogInformation($"Se encontraron {products.Count()} productos.");
                }

                return View("ProductList", products);
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Error al consultar productos.", ex);
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View("Error");
            }
        }

        // GET: Category/List
        [HttpGet]
        public ActionResult List()
        {
            try
            {
                LogHelper.LogInformation("Consultando lista de categorías.");
                var proxy = new Proxy();
                var categories = proxy.GetCategory();

                if (categories == null || !categories.Any())
                {
                    LogHelper.LogWarning("No hay categorías disponibles.");
                    ViewBag.Message = "No hay categorías disponibles.";
                }
                else
                {
                    LogHelper.LogInformation($"Se encontraron {categories.Count()} categorías.");
                }

                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                ViewBag.ErrorMessage = TempData["ErrorMessage"];

                return View(categories);
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Error al consultar categorías.", ex);
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View("Error");
            }
        }

        // Acción para obtener la categoría por ID (mostrar el formulario de edición)
        public ActionResult Edit(int id)
        {
            LogHelper.LogInformation($"Consultando categoría con ID: {id} para edición.");
            var proxy = new Proxy();
            var category = proxy.RetrieveCategoryByID(id);

            if (category == null)
            {
                LogHelper.LogWarning($"No se encontró la categoría con ID: {id}.");
                return HttpNotFound();
            }

            return View(category);
        }

        // Acción para actualizar la categoría
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            var proxy = new Proxy();

            if (ModelState.IsValid)
            {
                LogHelper.LogInformation($"Actualizando categoría con ID: {category.CategoryID}.");
                bool result = proxy.UpdateCategory(category);
                if (result)
                {
                    LogHelper.LogInformation("Categoría actualizada exitosamente.");
                    TempData["SuccessMessage"] = "Categoría actualizada exitosamente.";
                    return RedirectToAction("List");
                }
                else
                {
                    LogHelper.LogWarning("Error al actualizar la categoría.");
                    ModelState.AddModelError("", "Error al actualizar la categoría.");
                }
            }
            return View(category);
        }

        // Acción para eliminar la categoría por ID
        public ActionResult Delete(int id)
        {
            LogHelper.LogInformation($"Consultando categoría con ID: {id} para eliminación.");
            var proxy = new Proxy();
            var category = proxy.RetrieveCategoryByID(id);

            if (category == null)
            {
                LogHelper.LogWarning($"No se encontró la categoría con ID: {id}.");
                return HttpNotFound();
            }

            return View(category);
        }

        // Acción para confirmar la eliminación
        [HttpGet]
        public ActionResult DeleteConfirmed(int id)
        {
            LogHelper.LogInformation($"Eliminando categoría con ID: {id}.");
            var proxy = new Proxy();
            bool result = proxy.DeleteCategory(id);

            if (result)
            {
                LogHelper.LogInformation("Categoría eliminada exitosamente.");
                TempData["SuccessMessage"] = "Categoría eliminada exitosamente";
            }
            else
            {
                LogHelper.LogWarning("Error al eliminar la categoría.");
                TempData["ErrorMessage"] = "No se pudo eliminar la categoría.";
            }

            return RedirectToAction("List");
        }

        // Acción para mostrar el formulario de creación
        public ActionResult Create()
        {
            LogHelper.LogInformation("Mostrando formulario de creación de categoría.");
            return View();
        }

        // Acción para guardar la nueva categoría
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category newCategory)
        {
            var proxy = new Proxy();

            if (ModelState.IsValid)
            {
                LogHelper.LogInformation("Creando nueva categoría.");
                var createdCategory = proxy.CreateCategory(newCategory);
                if (createdCategory != null)
                {
                    LogHelper.LogInformation("Categoría creada exitosamente.");
                    TempData["SuccessMessage"] = "Categoría guardada exitosamente.";
                    return RedirectToAction("List");
                }
                else
                {
                    LogHelper.LogWarning("Error al crear la categoría.");
                    ModelState.AddModelError("", "Error al crear la categoría.");
                }
            }

            return View(newCategory);
        }
    }
}
