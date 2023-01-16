using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.IRepository;

namespace Acetesa.TomaPedidos.Core.Business
{
    public class ListaPrecioService : IListaPrecioService
    {
        private readonly IListaPrecioRepository _listaPrecioRepository;

        public ListaPrecioService(IListaPrecioRepository listaPrecioRepository)
        {
            _listaPrecioRepository = listaPrecioRepository;
        }

        public ListaPrecioModel GetPreciosByCodArticulo(string ccArtic, string ct_email, string cc_tienda, string cc_lista, int igv_bo)
        {
            var query = _listaPrecioRepository.GetPreciosByCodArticulo(ccArtic, ct_email, cc_tienda, cc_lista, igv_bo);
            return query;
        }

        public ListaPrecioModel GetCcListaByCcArtic(string ccArtic)
        {
            var query = _listaPrecioRepository.GetCcListaByCcArtic(ccArtic);
            return query;
        }
    }
}
