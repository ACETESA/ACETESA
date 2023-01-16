using System.Collections.Generic;
using Acetesa.TomaPedidos.Domain;

namespace Acetesa.TomaPedidos.Core.IBusiness
{
    public interface IListaPrecioService
    {
        ListaPrecioModel GetPreciosByCodArticulo(string ccArtic, string ct_email, string cc_tienda, string cc_lista, int igv_bo);
        ListaPrecioModel GetCcListaByCcArtic(string ccArtic);
    }
}
