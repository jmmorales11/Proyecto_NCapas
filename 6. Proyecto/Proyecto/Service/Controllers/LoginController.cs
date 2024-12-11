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
                // Autenticar al usuario
                var user = BL.Authenticate(loginRequest.Email, loginRequest.Password);

                // Generar código de verificación
                var random = new Random();
                string verificationCode = random.Next(1000, 9999).ToString();

                // Guardar el código en un diccionario temporal
                VerificationCodes[loginRequest.Email] = verificationCode;

                // Enviar correo con el código
                body1 = $"Se registró un inicio de sesión. Tu código de verificación es: {verificationCode}";
                await _emailService.SendEmailAsync(recipientEmail, subject1, body1);

                return Ok(new
                {
                    Message = "Código de verificación enviado al correo.",
                    RequiresVerification = true
                });
            }
            catch (System.UnauthorizedAccessException ex)
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
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