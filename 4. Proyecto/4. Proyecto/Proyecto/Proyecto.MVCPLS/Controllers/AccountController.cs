using ProyectoProxyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.MVCPLS.Controllers
{
    public class AccountController : Controller
    {
        // Acción para mostrar el formulario de inicio de sesión
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var proxy = new Proxy();
            try
            {

                System.Diagnostics.Debug.WriteLine("Inicio de sesión iniciado."); // Log inicia

                System.Diagnostics.Debug.WriteLine($"Email recibido: {email}");

                var response = proxy.Authenticate(email, password);

                if (!string.IsNullOrEmpty(response.Token))
                {
                    // Guardar el token y otros datos en la sesión
                    Session["JWTToken"] = response.Token;
                    Session["UserID"] = response.UserID;
                    Session["Role"] = response.Role;

                    System.Diagnostics.Debug.WriteLine("Login exitoso. Token recibido y almacenado en la sesión.");

                    ViewBag.SuccessMessage = "Inicio de sesión exitoso.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {

                    // Manejo de error si el token es nulo o vacío
                    ViewBag.ErrorMessage = "Credenciales inválidas. Por favor, intente nuevamente.";
                    System.Diagnostics.Debug.WriteLine("Credenciales inválidas. Token no recibido.");

                    ViewBag.ErrorMessage = response.Message ?? "Credenciales inválidas. Por favor, intente nuevamente.";
                    System.Diagnostics.Debug.WriteLine("Credenciales inválidas.");

                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al procesar la solicitud: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Error durante el inicio de sesión: {ex.Message}");
                return View();
            }
        }
    }
}