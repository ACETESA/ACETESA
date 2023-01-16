using Acetesa.TomaPedidos.Domain;

namespace Acetesa.TomaPedidos.IRepository
{
    public interface IListaPrecioRepository : IBase<ListaPrecioModel>
    {
        ListaPrecioModel GetPreciosByCodArticulo(string ccArtic, string ct_email, string cc_tienda,string cc_lista, int igv_bo);
        ListaPrecioModel GetCcListaByCcArtic(string ccArtic);
    }
}
