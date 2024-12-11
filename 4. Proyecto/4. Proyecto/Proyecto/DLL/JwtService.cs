using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Security
{
    public static class JwtService
    {
        private const string SecretKey = "TuClaveSuperSecretaLargaDe32CaracteresOMas!"; // Cambia esta clave por algo seguro y almacénala de forma segura
        private const int TokenExpirationMinutes = 30; // Duración del token en minutos

        public static string GenerateToken(string email, string role)
        {
            var key = Encoding.UTF8.GetBytes(SecretKey);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "TuAplicacion",
                audience: "TuAplicacion",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(TokenExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
