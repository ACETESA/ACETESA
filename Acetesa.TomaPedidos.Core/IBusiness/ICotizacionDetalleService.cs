using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.Core.IBusiness
{
    public interface ICotizacionDetalleService
    {
        LDPROF_WEB GetByIdCnItem(string cnProforma, string cnItem);
        void Guardar(LDPROF_WEB entity);
    }
}
