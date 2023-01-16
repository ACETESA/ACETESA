using Acetesa.TomaPedidos.IRepository;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Acetesa.TomaPedidos.Repository
{
    public class RepositoryRegistry : Registry
    {
        public RepositoryRegistry()
        {
            Scan(
               scan =>
               {
                   scan.TheCallingAssembly();
                   scan.AssemblyContainingType<IProductoRepository>();
               });
            //For<IProductoRepository>().Use<ProductoRepository>();


        }
    }
}
