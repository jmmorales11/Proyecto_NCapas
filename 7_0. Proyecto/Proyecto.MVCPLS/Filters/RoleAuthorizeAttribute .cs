using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.MVCPLS.Filters
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public string[] AllowedRoles { get; set; }

        public RoleAuthorizeAttribute(params string[] roles)
        {
            AllowedRoles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Verificar si la cookie "UserRole" existe
            var roleCookie = httpContext.Request.Cookies["UserRole"];
            if (roleCookie == null || string.IsNullOrEmpty(roleCookie.Value))
            {
                return false; // No autorizado si no existe el rol
            }

            // Obtener el rol desde la cookie
            var userRole = roleCookie.Value;

            // Verificar si el rol del usuario está permitido
            if (AllowedRoles != null && AllowedRoles.Length > 0)
            {
                return Array.Exists(AllowedRoles, role => role.Equals(userRole, StringComparison.OrdinalIgnoreCase));
            }

            return false; // No autorizado si el rol no coincide
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Redirigir al usuario no autorizado a una página de error o login
            filterContext.Result = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary
                {
                    { "controller", "Account" },
                    { "action", "Login" }
                });
        }
    }
}