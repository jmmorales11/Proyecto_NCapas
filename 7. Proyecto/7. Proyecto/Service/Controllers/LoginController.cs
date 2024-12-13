using BLL;
using SL.Authorization;
using SL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using SL.Authentication;
using Service.Models;


namespace Service.Controllers
{
    [RoutePrefix("user")]

    public class LoginController : ApiController
    {
        private static Dictionary<string, string> VerificationCodes = new Dictionary<string, string>();
        private readonly IEmailService _emailService;

        public LoginController()
        {
            _emailService = new EmailService(); // Inicialización manual
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var BL = new Users();
            string recipientEmail = loginRequest.Email;
            string subject1 = "Inicio de sesión";
            string body1;
            try
            {
                // Intentar autenticar al usuario
                var user = BL.Authenticate(loginRequest.Email, loginRequest.Password);

                // Debugging: Add null checks and logging
                if (user == null)
                {
                    return Content(HttpStatusCode.Unauthorized, new { Message = "Usuario no encontrado" });
                }

                // Ensure you have email and role
                string userEmail = user.Email ?? loginRequest.Email;
                string userRole = user.Role ?? "User"; // Fallback to default role if not set

                // Generar token JWT inmediatamente después de la autenticación
                var token = JwtService.GenerateToken(userEmail, userRole);

                // Debugging: Check if token is null
                if (string.IsNullOrEmpty(token))
                {
                    return Content(HttpStatusCode.InternalServerError, new { Message = "Error al generar token" });
                }

                // Enviar correo de notificación de inicio de sesión
                body1 = $"Se ha iniciado sesión en tu cuenta.";
                await _emailService.SendEmailAsync(recipientEmail, subject1, body1);

                // Devolver token y información del usuario
                return Ok(new
                {
                    Token = token,
                    Email = userEmail,
                    Role = userRole,
                    Message = "Inicio de sesión exitoso"
                });
            }
            catch (System.UnauthorizedAccessException ex)
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the full exception details
                // Consider using a proper logging framework
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("verify-code")]
        [AllowAnonymous]
        public IHttpActionResult VerifyCode([FromBody] VerifyCodeRequest verifyRequest)
        {
            if (VerificationCodes.TryGetValue(verifyRequest.Email, out string storedCode) && storedCode == verifyRequest.Code)
            {
                // Eliminar el código una vez validado
                VerificationCodes.Remove(verifyRequest.Email);

                // Para el ejemplo, supongamos que el rol fue pasado desde el cliente o almacenado temporalmente
                string userRole = "User"; // Reemplaza con el rol correcto si ya lo tienes en algún lugar

                // Generar token JWT
                var token = JwtService.GenerateToken(verifyRequest.Email, userRole);

                return Ok(new
                {
                    Token = token,
                    Email = verifyRequest.Email,
                    Role = userRole,
                    Message = "Login exitoso"
                });
            }
            else
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = "Código de verificación inválido o expirado." });
            }
        }

        public class VerifyCodeRequest
        {
            public string Email { get; set; }
            public string Code { get; set; }
        }
    }
}