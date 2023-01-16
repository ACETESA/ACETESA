using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Acetesa.TomaPedidos.AdminMvc.Controllers;
using log4net;
using StructureMap.Web.Pipeline;

namespace Acetesa.TomaPedidos.AdminMvc
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
        }

        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            MvcHandler.DisableMvcResponseHeader = true;
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }
    }
}