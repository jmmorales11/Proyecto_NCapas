using Entities;
using ProyectoProxyService;
using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using SL.Logger;  // Asegúrate de incluir el namespace de Serilog

namespace Proyecto.MVCPLS.Controllers
{
    public class AccountController : Controller
    {
        // Acción para mostrar el formulario de inicio de sesión
        public ActionResult Login()
        {
            LogHelper.LogInformation("Se mostró el formulario de inicio de sesión.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  // Protección contra CSRF
        public ActionResult Login(string email, string password)
        {
            // Validar las entradas de usuario para prevenir XSS y otros problemas
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                LogHelper.LogWarning("Intento de inicio de sesión con campos vacíos.");
                ViewBag.ErrorMessage = "Por favor ingrese un correo electrónico y una contraseña válidos.";
                return View();
            }

            // Validación básica de correo electrónico (evita inyección de comandos y XSS)
            var emailAddress = new EmailAddressAttribute();
            if (!emailAddress.IsValid(email))
            {
                LogHelper.LogWarning($"Correo electrónico inválido: {email}");
                ViewBag.ErrorMessage = "El correo electrónico no es válido.";
                return View();
            }

            // Usar el proxy para autenticar al usuario, evitando inyecciones SQL a través de parametrización interna
            var proxy = new Proxy();
            try
            {
                LogHelper.LogInformation($"Inicio de sesión iniciado para el correo: {email}");

                // Suponiendo que Authenticate ya maneja los parámetros de forma segura en el servicio
                var response = proxy.Authenticate(email, password);

                if (!string.IsNullOrEmpty(response.Token))
                {
                    // Guardar el token y otros datos en la sesión
                    Session["JWTToken"] = response.Token;
                    Session["UserID"] = response.UserID;
                    Session["Role"] = response.Role;

                    LogHelper.LogInformation("Login exitoso. Token recibido y almacenado en la sesión.");

                    ViewBag.SuccessMessage = "Inicio de sesión exitoso.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Manejo de error si el token es nulo o vacío
                    LogHelper.LogWarning("Credenciales inválidas o token no recibido.");
                    ViewBag.ErrorMessage = response.Message ?? "Credenciales inválidas. Por favor, intente nuevamente.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Manejo seguro de excepciones
                LogHelper.LogError("Error durante el inicio de sesión", ex);
                ViewBag.ErrorMessage = "Error al procesar la solicitud. Por favor, intente nuevamente.";
                return View();
            }
        }
    }
}
