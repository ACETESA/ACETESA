using System.Collections.Generic;
using Acetesa.TomaPedidos.Domain;

namespace Acetesa.TomaPedidos.IRepository
{
    public interface IEstadoCuentaRepository : IBase<EstadoCuentaModel>
    {
        EstadoCuentaResumenModel GetResumenByRuc(string ruc);
        IEnumerable<EstadoCuentaDetalleModel> GetDetalleByRuc(string ruc);
    }
}
