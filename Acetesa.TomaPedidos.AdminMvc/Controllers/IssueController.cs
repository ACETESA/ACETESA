using System.Web.Mvc;

namespace Acetesa.TomaPedidos.AdminMvc.Controllers
{
    [Authorize]
    public class IssueController : Controller
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Http404()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Http500()
        {
            return View();
        }
    }
}