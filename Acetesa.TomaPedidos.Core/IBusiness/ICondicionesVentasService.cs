using System.Collections.Generic;
using Acetesa.TomaPedidos.Domain;

namespace Acetesa.TomaPedidos.Core.IBusiness
{
    public interface ICondicionesVentasService
    {
        IEnumerable<CondicionVentaModel> GetAll();
        IEnumerable<CondicionVentaModel> GetAll(string ccAnalis);
        List<CondicionVentaModel> RecuperarCondicionVentaPorClienteID(string cc_analis);
        
    }
}
