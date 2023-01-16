using System.Web.Mvc;
using Acetesa.TomaPedidos.AdminMvc.Infrastructure;

namespace Acetesa.TomaPedidos.AdminMvc.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //return View();
            return RedirectToAction<VentaController>(x => x.Venta());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            //return View();
            return RedirectToAction<VentaController>(x => x.Venta());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            //return View();
            return RedirectToAction<VentaController>(x => x.Venta());
        }
    }
}