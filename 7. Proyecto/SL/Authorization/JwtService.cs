using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SL.Authorization
{
    public static class JwtService
    {
        // Configuration for token generation and validation
        private const string SecretKey = "TuClaveSuperSecretaLargaDe32CaracteresOMas!";
        private const int TokenExpirationMinutes = 30;
        private const string Issuer = "TuAplicacion";
        private const string Audience = "TuAplicacion";

        // Generate JWT token
        public static string GenerateToken(string email, string role)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(SecretKey);
                var credentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                );

                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var token = new JwtSecurityToken(
                    issuer: Issuer,
                    audience: Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(TokenExpirationMinutes),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                // In a real-world scenario, log the exception
                return null;
            }
        }

        // Validate and extract role from JWT token
        public static string GetRoleFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(SecretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = Issuer,
                    ValidateAudience = true,
                    ValidAudience = Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                var roleClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                return roleClaim?.Value;
            }
            catch (Exception ex)
            {
                // In a real-world scenario, log the exception
                return null;
            }
        }

        // Validate token's overall integrity
        public static bool ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(SecretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = Issuer,
                    ValidateAudience = true,
                    ValidAudience = Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                tokenHandler.ValidateToken(token, validationParameters, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Extract email from token
        public static string GetEmailFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(SecretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = Issuer,
                    ValidateAudience = true,
                    ValidAudience = Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                return emailClaim?.Value;
            }
            catch
            {
                return null;
            }
        }

        // Optional: Get token expiration time
        public static DateTime? GetTokenExpirationTime(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                return jwtToken?.ValidTo;
            }
            catch
            {
                return null;
            }
        }
    }
}
