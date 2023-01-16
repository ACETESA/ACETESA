using System.Collections.Generic;
using Acetesa.TomaPedidos.Domain;

namespace Acetesa.TomaPedidos.Core.IBusiness
{
    public interface IEstadoCuentaService
    {
        EstadoCuentaResumenModel GetResumenByRuc(string ruc);
        IEnumerable<EstadoCuentaDetalleModel> GetDetalleByRuc(string ruc);
    }
}
