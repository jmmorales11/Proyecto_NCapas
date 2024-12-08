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
        // Acción para mostrar la lista de usuarios
        public ActionResult List()
        {
            var proxy = new Proxy();

            try
            {
                var users = proxy.GetUsers();

                if (users == null || !users.Any())
                {
                    ViewBag.Message = "No hay usuarios disponibles.";
                }

                return View(users);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View("Error");
            }
        }

        // Acción para obtener el usuario por ID (mostrar el formulario de edición)
        public ActionResult Edit(int id)
        {
            var proxy = new Proxy();

            try
            {
                var user = proxy.RetrieveUserByID(id);

                if (user == null)
                {
                    return HttpNotFound();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View("Error");
            }
        }

        // Acción para actualizar un usuario
        [HttpPost]
        public ActionResult Edit(User userToUpdate)
        {
            var proxy = new Proxy();

            try
            {
                if (ModelState.IsValid)
                {
                    var result = proxy.UpdateUser(userToUpdate);

                    if (result)
                    {
                        return RedirectToAction("List");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error al actualizar el usuario.");
                    }
                }

                return View(userToUpdate);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View("Error");
            }
        }

        // Acción para mostrar el formulario de registro
        public ActionResult Create()
        {
            return View();
        }

        // Acción para guardar un nuevo usuario
        [HttpPost]
        public ActionResult Create(User newUser)
        {
            var proxy = new Proxy();

            try
            {
                if (ModelState.IsValid)
                {
                    var createdUser = proxy.CreateUser(newUser);

                    if (createdUser != null)
                    {
                        return RedirectToAction("List");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error al crear el usuario.");
                    }
                }

                return View(newUser);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View("Error");
            }
        }

        // Acción para eliminar un usuario por ID
        public ActionResult Delete(int id)
        {
            var proxy = new Proxy();

            try
            {
                var user = proxy.RetrieveUserByID(id);

                if (user == null)
                {
                    return HttpNotFound();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View("Error");
            }
        }

        // Acción para confirmar la eliminación de un usuario
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var proxy = new Proxy();

            try
            {
                var result = proxy.DeleteUser(id);

                if (result)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError("", "Error al eliminar el usuario.");
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
                return View("Error");
            }
        }
    }
}