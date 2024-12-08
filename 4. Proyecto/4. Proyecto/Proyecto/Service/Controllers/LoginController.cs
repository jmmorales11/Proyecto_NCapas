using BLL;
using BLL.Security; // Para PasswordHasher
using BLL.Exceptions;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Service.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var BL = new Users();

            try
            {
                // Autenticar al usuario
                var user = BL.Authenticate(loginRequest.Email, loginRequest.Password);

                // Generar el token JWT
                var token = JwtService.GenerateToken(user.Email, user.Role);

                return Ok(new
                {
                    Token = token,
                    UserID = user.UserID,
                    Role = user.Role,
                    Message = "Login exitoso"
                });
            }
            catch (BLL.Exceptions.UnauthorizedAccessException ex) // Excepción específica de la BLL
            {
                return Content(HttpStatusCode.Unauthorized, new { Message = ex.Message });
            }
            catch (Exception ex) // Capturar cualquier otro error
            {
                return InternalServerError(ex);
            }
        }


    }
}
