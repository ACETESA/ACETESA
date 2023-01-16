using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.IRepository
{
    public interface ICotizacionDetalleRepository : IBase<LDPROF_WEB>
    {
        LDPROF_WEB GetByIdCnItem(string cnProforma, string cnItem);
        void Delete(LDPROF_WEB entity);
    }
}
