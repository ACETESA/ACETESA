using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acetesa.TomaPedidos.AdminMvc.Controllers
{
    public class LiquidacionGastosController : Controller
    {
        // GET: LiquidacionGastos
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Listado()
        {
            return View();
        }

    }
}