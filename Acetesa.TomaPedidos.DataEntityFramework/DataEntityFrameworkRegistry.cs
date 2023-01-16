using System.Data.Entity;
using Acetesa.TomaPedidos.DataEntityFramework.Models;
using Acetesa.TomaPedidos.Transversal;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Web;
using StructureMap.Web.Pipeline;

namespace Acetesa.TomaPedidos.DataEntityFramework
{
    public class DataEntityFrameworkRegistry : Registry
    {
        public DataEntityFrameworkRegistry()
        {
            Scan(
               scan => scan.TheCallingAssembly());

            For<DbContext>().HybridHttpOrThreadLocalScoped().Use<ZicoErpContext>(); 
            For<IDbContext>().HybridHttpOrThreadLocalScoped().Use<DbContextAdapter>();
        }
    }
}
