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
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            LogHelper.LogInformation("Iniciando vista de índice de usuarios.");
            return View();
        }

        // Acción para mostrar el formulario de creación
        public ActionResult Create()
        {
            LogHelper.LogInformation("Mostrando formulario de creación de usuario.");
            return View();
        }

        // Acción para guardar el nuevo usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User newUser)
        {
            var proxy = new Proxy();

            if (ModelState.IsValid)
            {
                LogHelper.LogInformation("Creando nuevo usuario.");
                var createdUser = proxy.CreateUser(newUser);
                if (createdUser != null)
                {
                    LogHelper.LogInformation("Usuario creado exitosamente.");
                    TempData["SuccessMessage"] = "Usuario creado exitosamente";
                    return RedirectToAction("List");
                }
                else
                {
                    LogHelper.LogWarning("Error al crear el usuario.");
                    ModelState.AddModelError("", "Error al crear el usuario.");
                }
            }
            return View(newUser);
        }

        // Acción para obtener el usuario por ID (mostrar el formulario de edición)
        public ActionResult Edit(int id)
        {
            LogHelper.LogInformation($"Consultando usuario con ID: {id} para edición.");
            var proxy = new Proxy();

            var user = proxy.RetrieveUserByID(id);
            if (user == null)
            {
                LogHelper.LogWarning($"No se encontró el usuario con ID: {id}.");
                return HttpNotFound();
            }
            return View(user);
        }

        // Acción para actualizar el usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            var proxy = new Proxy();

            if (ModelState.IsValid)
            {
                LogHelper.LogInformation($"Actualizando usuario con ID: {user.UserID}.");
                bool result = proxy.UpdateUser(user);
                if (result)
                {
                    LogHelper.LogInformation("Usuario actualizado exitosamente.");
                    TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
                    return RedirectToAction("List");
                }
                else
                {
                    LogHelper.LogWarning("Error al actualizar el usuario.");
                    TempData["ErrorMessage"] = "Error al actualizar el usuario";
                }
            }
            return View(user);
        }

        // Acción para eliminar el usuario por ID
        public ActionResult Delete(int id)
        {
            LogHelper.LogInformation($"Consultando usuario con ID: {id} para eliminación.");
            var proxy = new Proxy();

            var user = proxy.RetrieveUserByID(id);
            if (user == null)
            {
                LogHelper.LogWarning($"No se encontró el usuario con ID: {id}.");
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
                LogHelper.LogInformation($"Eliminando usuario con ID: {id}.");
                bool result = proxy.DeleteUser(id);
                if (result)
                {
                    LogHelper.LogInformation("Usuario eliminado exitosamente.");
                    TempData["SuccessMessage"] = "Usuario eliminado exitosamente.";
                }
                else
                {
                    LogHelper.LogWarning("No se pudo eliminar el usuario.");
                    TempData["ErrorMessage"] = "No se pudo eliminar el usuario.";
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Error al intentar eliminar el usuario.", ex);
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
                LogHelper.LogInformation("Consultando lista de usuarios.");
                var proxy = new Proxy();
                var users = proxy.GetUsers();

                if (users == null || !users.Any())
                {
                    LogHelper.LogWarning("No hay usuarios disponibles.");
                    ViewBag.Message = "No hay usuarios disponibles.";
                }
                else
                {
                    LogHelper.LogInformation($"Se encontraron {users.Count()} usuarios.");
                }

                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                ViewBag.ErrorMessage = TempData["ErrorMessage"];

                return View(users);
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Error al consultar usuarios.", ex);
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View("Error");
            }
        }
    }
}
