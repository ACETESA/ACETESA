using System.Collections.Generic;
using Acetesa.TomaPedidos.Domain;

namespace Acetesa.TomaPedidos.Core.IBusiness
{
    public interface IEstadoService
    {
        IEnumerable<EstadoModel> GetAll();
    }
}
