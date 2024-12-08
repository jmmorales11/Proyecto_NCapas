using Microsoft.Owin;
using Owin;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.Jwt;
using System.Text;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Service.Startup))]

namespace Service
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var key = Encoding.UTF8.GetBytes("TuClaveSuperSecretaLargaDe32CaracteresOMas!");

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "TuAplicacion",
                    ValidAudience = "TuAplicacion",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                },
                Provider = new OAuthBearerAuthenticationProvider
                {
                    OnRequestToken = context =>
                    {
                        // Permitir la ruta de login sin autenticación
                        if (context.Request.Path.StartsWithSegments(new PathString("/login")) ||
                            context.Request.Path.StartsWithSegments(new PathString("/user/create-user")))
                        {
                            context.Token = null;
                        }
                        return Task.FromResult<object>(null);
                    }
                }
            });
        }
    }
}