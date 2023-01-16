using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Acetesa.TomaPedidos.AdminMvc.Startup))]
namespace Acetesa.TomaPedidos.AdminMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}