using Entities;
using ProyectoProxyService;
using SL.Authentication;
using SL.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.MVCPLS.Controllers
{
    public class UserController : Controller
    {
        private readonly IEmailService _emailService;
        public UserController()
        {
            _emailService = new EmailService(); // Inicialización manual del servicio de correos
        }
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
        /////////////////////////////VERIFFICAR EL CODIGO
        ///        [HttpPost]
        public async Task<ActionResult> SendVerificationCode(string email)
        {
            if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
            {
                return Json(new { success = false, message = "Correo electrónico inválido." });
            }

            // Generar código de seguridad
            var random = new Random();
            string securityCode = random.Next(1000, 9999).ToString();
            string subject = "Código de verificación";
            string body = $"Tu código de verificación es: {securityCode}";

            try
            {
                // Enviar el correo
                await _emailService.SendEmailAsync(email, subject, body);

                // Guardar el código en la sesión
                Session["SecurityCode"] = securityCode;
                Session["EmailToVerify"] = email;

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al enviar el correo: {ex.Message}" });
            }
        }

        // Acción para verificar el código de seguridad
        public ActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VerifySecurityCode(string enteredCode)
        {
            var storedCode = Session["SecurityCode"] as string;
            var emailToVerify = Session["EmailToVerify"] as string;

            if (storedCode == enteredCode)
            {
                // Código correcto, marcar el correo como verificado
                Session["IsEmailVerified"] = true;

                return Json(new { success = true, email = emailToVerify });
            }

            return Json(new { success = false, message = "El código ingresado es incorrecto." });
        }

        // Acción para crear el usuario
        [HttpPost]
        public ActionResult CreateUser(User newUser)
        {
            if (Session["IsEmailVerified"] == null || !(bool)Session["IsEmailVerified"] || newUser.Email != Session["EmailToVerify"].ToString())
            {
                return Json(new { success = false, message = "El correo no ha sido verificado." });
            }

            var proxy = new Proxy();

            if (ModelState.IsValid)
            {
                var createdUser = proxy.CreateUser(newUser);
                if (createdUser != null)
                {
                    // Limpiar sesión
                    Session.Remove("IsEmailVerified");
                    Session.Remove("EmailToVerify");
                    TempData["SuccessMessage"] = "Usuario creado exitosamente";
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false, message = "Error al crear el usuario." });
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}