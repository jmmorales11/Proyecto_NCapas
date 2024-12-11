using Entities;
using ProyectoProxyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.MVCPLS.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // Acción para mostrar el formulario de creación
        public ActionResult Create()
        {
            return View();
        }

        // Acción para guardar el nuevo usuario
        [HttpPost]
        public ActionResult Create(User newUser)
        {
            var proxy = new Proxy();

            if (ModelState.IsValid)
            {
                var createdUser = proxy.CreateUser(newUser);
                if (createdUser != null)
                {
                    TempData["SuccessMessage"] = "Usuario creado exitosamente";
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError("", "Error al crear el usuario.");
                }
            }
            return View(newUser);
        }

        // Acción para obtener el usuario por ID (mostrar el formulario de edición)
        public ActionResult Edit(int id)
        {
            var proxy = new Proxy();

            var user = proxy.RetrieveUserByID(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // Acción para actualizar el usuario
        [HttpPost]
        public ActionResult Edit(User user)
        {
            var proxy = new Proxy();

            if (ModelState.IsValid)
            {
                bool result = proxy.UpdateUser(user);
                if (result)
                {
                    TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
                    return RedirectToAction("List");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al actualizar el usuario";
                }
            }
            return View(user);
        }

        // Acción para eliminar el usuario por ID
        public ActionResult Delete(int id)
        {
            var proxy = new Proxy();

            var user = proxy.RetrieveUserByID(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // Acción para confirmar la eliminación del usuario
        [HttpGet]
        public ActionResult DeleteConfirmed(int id)
        {
            var proxy = new Proxy();

            try
            {
                bool result = proxy.DeleteUser(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Usuario eliminado exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar el usuario.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al intentar eliminar el usuario: {ex.Message}";
            }

            return RedirectToAction("List");
        }

        // Acción para mostrar la lista de usuarios
        [HttpGet]
        public ActionResult List()
        {
            try
            {
                var proxy = new Proxy();
                var users = proxy.GetUsers();

                if (users == null || !users.Any())
                {
                    ViewBag.Message = "No hay usuarios disponibles.";
                }

                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                ViewBag.ErrorMessage = TempData["ErrorMessage"];

                return View(users);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View("Error");
            }
        }
    }
}
