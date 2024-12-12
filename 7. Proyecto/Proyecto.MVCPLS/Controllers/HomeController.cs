using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoProxyService;
using Entities;
using SL.Logger;

namespace Proyecto.MVCPLS.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            LogHelper.LogInformation("Iniciando vista de Home.");
            return View();
        }
    }
}