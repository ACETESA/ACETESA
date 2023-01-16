using System.Web;
using System.Web.Mvc;

namespace Acetesa.TomaPedidos.AdminMvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
