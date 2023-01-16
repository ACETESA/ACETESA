using System.Web.Mvc;

namespace Acetesa.TomaPedidos.AdminMvc.Controllers
{
    public class TransportistaController : Controller
    {
        // GET: Transportista

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Listado()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Nuevo()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Nuevo(string model)
        {
            return View();
        }
    }
}