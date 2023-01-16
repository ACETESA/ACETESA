using System.Collections.Generic;
using Acetesa.TomaPedidos.Domain;

namespace Acetesa.TomaPedidos.IRepository
{
    public interface ICondicionesVentasRepository : IBase<CondicionVentaModel>
    {
        IEnumerable<CondicionVentaModel> GetAll();
        IEnumerable<CondicionVentaModel> GetAll(string ccAnalis);
    }
}
