using BLL;
using BLL.Security; // Para PasswordHasher
using BLL.Exceptions;
using Service.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Common.Interfaces;

namespace Service.Controllers
{
    public class LoginController : ApiController
    {
        private readonly IEmailService _emailService;
        public LoginController()
        {
            _emailService = new EmailService(); // Inicialización manual
        }

        [HttpPost]
        [Route("login")]
        public async Task<IHttpActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var BL = new Users();
            string recipientEmail = loginRequest.Email;
            string subject = "Inicio de sesión fallido";
            string subject1 = "Inicio de sesión";
            string body = $"Se registró un inicio de sesión fallido.";


            try
            {
                // Autenticar al usuario
                var user = BL.Authenticate(loginRequest.Email, loginRequest.Password);
                var random = new Random();
                string verificationCode = random.Next(1000, 9999).ToString();
                string body1 = $"Se registró un inicio de sesión. Tu código de verificación es: {verificationCode}";
                await _emailService.SendEmailAsync(recipientEmail, subject1, body1);
                // Generar el token JWT
   
                return Ok(new
                {

                    UserID = user.UserID,
                    Role = user.Role,
                    Message = "Login exitoso"
                });

            }
            catch (BLL.Exceptions.UnauthorizedAccessException ex) // Contraseña incorrecta
            {
                try
                {
                    await _emailService.SendEmailAsync(recipientEmail, subject, body);
                }
                catch (Exception emailEx)
                {
                    // Log de error al enviar el correo, para evitar que falle el flujo
                    Console.WriteLine($"Error al enviar el correo: {emailEx.Message}");
                }

                return Content(HttpStatusCode.Unauthorized, new { Message = ex.Message });
            }
            catch (Exception ex) // Otros errores
            {
                try
                {
                    await _emailService.SendEmailAsync(recipientEmail, subject, body);
                }
                catch (Exception emailEx)
                {
                    Console.WriteLine($"Error al enviar el correo: {emailEx.Message}");
                }

                return InternalServerError(ex);
            }
        }
    }
}
