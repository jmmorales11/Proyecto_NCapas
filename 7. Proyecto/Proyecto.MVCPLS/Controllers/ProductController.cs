using Entities;
using Proyecto.MVCPLS.Filters;
using ProyectoProxyService;
using SL.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.MVCPLS.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        [RoleAuthorize("Viewer", "Editor", "Admin")]

        public ActionResult Index()
        {
            LogHelper.LogInformation("Iniciando vista de índice de productos.");
            return View();
        }

        // Acción para mostrar el formulario de creación
        [RoleAuthorize("Editor", "Admin")]
        public ActionResult Create()
        {
            LogHelper.LogInformation("Mostrando formulario de creación de producto.");
            return View();
        }

        // Acción para guardar el nuevo producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize("Editor", "Admin")]
        public ActionResult Create(Product newProduct)
        {
            var proxy = new Proxy();

            if (ModelState.IsValid)
            {
                LogHelper.LogInformation("Creando nuevo producto.");
                var createdProduct = proxy.CreateProduct(newProduct);
                if (createdProduct != null)
                {
                    LogHelper.LogInformation("Producto creado exitosamente.");
                    TempData["SuccessMessage"] = "Producto creado exitosamente";
                    return RedirectToAction("List");
                }
                else
                {
                    LogHelper.LogWarning("Error al crear el producto.");
                    ModelState.AddModelError("", "Error al crear el producto.");
                }
            }
            return View(newProduct);
        }

        // Acción para obtener el producto por ID (mostrar el formulario de edición)
        [RoleAuthorize("Editor", "Admin")]
        public ActionResult Edit(int id)
        {
            LogHelper.LogInformation($"Consultando producto con ID: {id} para edición.");
            var proxy = new Proxy();
            var product = proxy.RetrieveProductByID(id);
            if (product == null)
            {
                LogHelper.LogWarning($"No se encontró el producto con ID: {id}.");
                return HttpNotFound();
            }
            return View(product);
        }

        // Acción para actualizar el producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize("Editor", "Admin")]
        public ActionResult Edit(Product product)
        {
            var proxy = new Proxy();

            if (ModelState.IsValid)
            {
                LogHelper.LogInformation($"Actualizando producto con ID: {product.ProductID}.");
                bool result = proxy.UpdateProduct(product);
                if (result)
                {
                    LogHelper.LogInformation("Producto actualizado exitosamente.");
                    TempData["SuccessMessage"] = "Producto actualizado exitosamente.";
                    return RedirectToAction("List");
                }
                else
                {
                    LogHelper.LogWarning("Error al actualizar el producto.");
                    TempData["ErrorMessage"] = "Error al actualizar el producto.";
                }
            }
            return View(product);
        }

        // Acción para eliminar el producto por ID
        [RoleAuthorize("Admin")]
        public ActionResult Delete(int id)
        {
            LogHelper.LogInformation($"Consultando producto con ID: {id} para eliminación.");
            var proxy = new Proxy();
            var product = proxy.RetrieveProductByID(id);
            if (product == null)
            {
                LogHelper.LogWarning($"No se encontró el producto con ID: {id}.");
                return HttpNotFound();
            }
            return View(product);
        }

        // Acción para confirmar la eliminación
        [HttpGet]
        public ActionResult DeleteConfirmed(int id)
        {
            LogHelper.LogInformation($"Eliminando producto con ID: {id}.");
            var proxy = new Proxy();

            try
            {
                bool result = proxy.DeleteProduct(id);
                if (result)
                {
                    LogHelper.LogInformation("Producto eliminado exitosamente.");
                    TempData["SuccessMessage"] = "Producto eliminado exitosamente.";
                }
                else
                {
                    LogHelper.LogWarning("No se pudo eliminar el producto. Es posible que tenga existencias.");
                    TempData["ErrorMessage"] = "No se pudo eliminar el producto. Es posible que tenga existencias.";
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Error al intentar eliminar el producto.", ex);
                TempData["ErrorMessage"] = $"Error al intentar eliminar el producto: {ex.Message}";
            }

            return RedirectToAction("List");
        }

        [HttpGet]
        [RoleAuthorize("Viewer", "Editor", "Admin")]
        public ActionResult List()
        {
            try
            {
                LogHelper.LogInformation("Consultando lista de productos.");
                var proxy = new Proxy();
                var products = proxy.GetProducts();

                if (products == null || !products.Any())
                {
                    LogHelper.LogWarning("No hay productos disponibles.");
                    ViewBag.Message = "No hay productos disponibles.";
                }
                else
                {
                    LogHelper.LogInformation($"Se encontraron {products.Count()} productos.");
                }

                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                ViewBag.ErrorMessage = TempData["ErrorMessage"];

                return View(products);
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Error al consultar productos.", ex);
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View("Error");
            }
        }
    }
}
