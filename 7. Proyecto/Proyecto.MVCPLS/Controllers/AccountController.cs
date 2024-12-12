using Entities;
using ProyectoProxyService;
using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using SL.Logger; // Asegúrate de incluir el namespace del logger
using System.Web;
using Proyecto.MVCPLS.Models; // Para LoginViewModel

namespace Proyecto.MVCPLS.Controllers
{
    public class AccountController : Controller
    {
        private readonly Proxy _proxy;

        public AccountController()
        {
            // Inicializa el proxy para interactuar con la API
            _proxy = new Proxy();
        }

        // Acción para mostrar el formulario de inicio de sesión
        public ActionResult Login()
        {
            LogHelper.LogInformation("Se mostró el formulario de inicio de sesión.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  // Protección contra CSRF
        public ActionResult Login(LoginViewModel model)
        {
            // Validar las entradas del usuario
            if (!ModelState.IsValid)
            {
                LogHelper.LogWarning("Intento de inicio de sesión con datos inválidos.");
                ViewBag.ErrorMessage = "Por favor ingrese los datos correctamente.";
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                LogHelper.LogWarning("Intento de inicio de sesión con campos vacíos.");
                ViewBag.ErrorMessage = "Por favor ingrese un correo electrónico y una contraseña válidos.";
                return View(model);
            }

            var emailAddress = new EmailAddressAttribute();
            if (!emailAddress.IsValid(model.Email))
            {
                LogHelper.LogWarning($"Correo electrónico inválido: {model.Email}");
                ViewBag.ErrorMessage = "El correo electrónico no es válido.";
                return View(model);
            }

            try
            {
                LogHelper.LogInformation($"Inicio de sesión iniciado para el correo: {model.Email}");

                // Llama a la función síncrona Authenticate del proxy
                var response = _proxy.Authenticate(model.Email, model.Password);

                if (response != null && !string.IsNullOrEmpty(response.Token))
                {
                    // Crear una cookie para almacenar el token
                    var authCookie = new HttpCookie("AuthToken", response.Token)
                    {
                        HttpOnly = true, // Seguridad contra accesos desde scripts
                        Secure = Request.IsSecureConnection, // Solo para conexiones HTTPS
                        Expires = DateTime.Now.AddHours(1) // Establecer tiempo de expiración
                    };

                    var roleCookie = new HttpCookie("UserRole", response.Role)
                    {
                        HttpOnly = true,
                        Secure = Request.IsSecureConnection,
                        Expires = DateTime.Now.AddHours(1)
                    };

                    Response.Cookies.Add(authCookie);
                    Response.Cookies.Add(roleCookie);

                    LogHelper.LogInformation("Login exitoso. Token y rol guardados en cookies.");

                    // Redirigir al Dashboard Principal
                    return RedirectToAction("Index", "Home");
                }

                // Si la autenticación falla, muestra un mensaje de error
                ViewBag.ErrorMessage = response?.Message ?? "Credenciales inválidas. Por favor, intente nuevamente.";
                LogHelper.LogWarning("Credenciales inválidas o token no recibido.");
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción
                LogHelper.LogError("Error durante el inicio de sesión", ex);
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
            }

            // Si llegamos aquí, la autenticación falló
            return View(model);
        }

        // Acción para cerrar sesión
        public ActionResult Logout()
        {
            // Limpiar la sesión y las cookies
            Session.Abandon();

            if (Request.Cookies["AuthToken"] != null)
            {
                var authCookie = new HttpCookie("AuthToken")
                {
                    Expires = DateTime.Now.AddDays(-1) // Expirar la cookie
                };
                Response.Cookies.Add(authCookie);
            }

            if (Request.Cookies["UserRole"] != null)
            {
                var roleCookie = new HttpCookie("UserRole")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(roleCookie);
            }

            LogHelper.LogInformation("El usuario cerró sesión.");
            return RedirectToAction("Login", "Account");
        }
    }
}
