using Entities;
using ProyectoProxyService;
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
        public ActionResult Index()
        {
            return View();
        }

        // Acción para mostrar el formulario de creación
        public ActionResult Create()
        {
            return View();
        }

        // Acción para guardar la nuevo producto
        [HttpPost]
        public ActionResult Create(Product newProduct)
        {
            var proxy = new Proxy();

            if (ModelState.IsValid)
            {
                var createdProduct = proxy.CreateProduct(newProduct);
                if (createdProduct != null)
                {
                    TempData["SuccessMessage"] = "Producto creado exitosamente";
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError("", "Error al crear el producto.");
                }
            }
            return View(newProduct);
        }


        // Acción para obtener el producto por ID (mostrar el formulario de edición)
        public ActionResult Edit(int id)
        {
            var proxy = new Proxy();

            var product = proxy.RetrieveProductByID(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // Acción para actualizar la categoría
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            var proxy = new Proxy();

            if (ModelState.IsValid)
            {
                bool result = proxy.UpdateProduct(product);
                if (result)
                {
                    TempData["SuccessMessage"] = "Producto actualizado exitosamente.";
                    return RedirectToAction("List");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al actualizar la categoria";
                }
            }
            return View(product);
        }

        // Acción para eliminar el producto por ID
        public ActionResult Delete(int id)
        {
            var proxy = new Proxy();

            var product = proxy.RetrieveProductByID(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // Acción para confirmar la eliminación
        [HttpGet]
        public ActionResult DeleteConfirmed(int id)
        {
            var proxy = new Proxy();

            try
            {
                bool result = proxy.DeleteProduct(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Producto eliminado exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar el producto. Es posible que tenga existencias.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al intentar eliminar el producto: {ex.Message}";
            }

            return RedirectToAction("List");
        }


        [HttpGet]
        public ActionResult List()
        {
            try
            {
                var proxy = new Proxy();
                var products = proxy.GetProducts();

                if (products == null || !products.Any())
                {
                    ViewBag.Message = "No hay productos disponibles.";
                }

                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                ViewBag.ErrorMessage = TempData["ErrorMessage"];

                return View(products);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View("Error");
            }
        }



    }
}