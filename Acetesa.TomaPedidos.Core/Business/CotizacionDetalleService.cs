using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;

namespace Acetesa.TomaPedidos.Core.Business
{
    public class CotizacionDetalleService : ICotizacionDetalleService
    {
        private readonly ICotizacionDetalleRepository _cotizacionDetalleRepository;

        public CotizacionDetalleService(ICotizacionDetalleRepository cotizacionDetalleRepository)
        {
            _cotizacionDetalleRepository = cotizacionDetalleRepository;
        }

        public LDPROF_WEB GetByIdCnItem(string cnProforma, string cnItem)
        {
            var query = _cotizacionDetalleRepository.GetByIdCnItem(cnProforma, cnItem);
            return query;
        }

        public void Guardar(LDPROF_WEB entity)
        {
            _cotizacionDetalleRepository.Add(entity);
        }
    }
}
