using System;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Proyecto.MVCPLS.Models; // Para LoginViewModel
using SL.Logger; // Para registro de logs
using ProyectoProxyService; // Para la clase Proxy

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

        // Método para destruir las cookies
        private void ClearAuthCookies()
        {
            if (Request.Cookies["AuthToken"] != null)
            {
                var authCookie = new HttpCookie("AuthToken")
                {
                    Expires = DateTime.Now.AddDays(-1) // Expirar la cookie
                };
                Response.Cookies.Add(authCookie);
                LogHelper.LogInformation("Cookie 'AuthToken' eliminada.");
            }

            if (Request.Cookies["UserRole"] != null)
            {
                var roleCookie = new HttpCookie("UserRole")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(roleCookie);
                LogHelper.LogInformation("Cookie 'UserRole' eliminada.");
            }
        }

        // Acción para mostrar el formulario de inicio de sesión
        public ActionResult Login()
        {
            // Destruir cookies de autenticación
            if (Request.Cookies["AuthToken"] != null)
            {
                var authCookie = new HttpCookie("AuthToken")
                {
                    Expires = DateTime.Now.AddDays(-1)
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

            LogHelper.LogInformation("El usuario accedió al login y se eliminaron las cookies.");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken] // Protección contra CSRF
        public ActionResult Login(LoginViewModel model)
        {
            // Asegúrate de eliminar las cookies antes de procesar la solicitud
            ClearAuthCookies();

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

                var response = _proxy.Authenticate(model.Email, model.Password);

                if (response != null && !string.IsNullOrEmpty(response.Token))
                {
                    var authCookie = new HttpCookie("AuthToken", response.Token)
                    {
                        HttpOnly = true,
                        Secure = Request.IsSecureConnection,
                        Expires = DateTime.Now.AddHours(1)
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
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.ErrorMessage = response?.Message ?? "Credenciales inválidas. Por favor, intente nuevamente.";
                LogHelper.LogWarning("Credenciales inválidas o token no recibido.");
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Error durante el inicio de sesión", ex);
                ViewBag.ErrorMessage = $"Ocurrió un error: {ex.Message}";
            }

            return View(model);
        }
    }
}
