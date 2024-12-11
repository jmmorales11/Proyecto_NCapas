using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Service
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de Web API

            // Rutas de Web API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            
        }

        public class RoleAuthorizeAttribute : AuthorizationFilterAttribute
        {
            private readonly string[] _roles;

            public RoleAuthorizeAttribute(params string[] roles)
            {
                _roles = roles;
            }

            public override void OnAuthorization(HttpActionContext actionContext)
            {
                var principal = actionContext.RequestContext.Principal;
                if (principal == null || !_roles.Any(role => principal.IsInRole(role)))
                {
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}
