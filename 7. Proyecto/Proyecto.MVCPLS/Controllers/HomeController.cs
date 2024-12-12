using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoProxyService;
using Entities;
using SL.Logger;
using Proyecto.MVCPLS.Filters;

namespace Proyecto.MVCPLS.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [RoleAuthorize("Viewer", "Editor", "Admin")]
        public ActionResult Index()
        {
            LogHelper.LogInformation("Iniciando vista de Home.");
            return View();
        }

        // Método para cerrar sesión
        public ActionResult Logout()
        {
            // Limpiar la sesión y las cookies
            Session.Abandon();

            if (Request.Cookies["AuthToken"] != null)
            {
                var authCookie = new HttpCookie("AuthToken")
                {
                    Expires = DateTime.Now.AddDays(-1) // Expirar la cookie
                };
                Response.Cookies.Add(authCookie);
            }

            if (Request.Cookies["UserRole"] != null)
            {
                var roleCookie = new HttpCookie("UserRole")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(roleCookie);
            }

            LogHelper.LogInformation("El usuario cerró sesión.");
            // Redirige al Login en el AccountController
            return RedirectToAction("Login", "Account");
        }

    }
}