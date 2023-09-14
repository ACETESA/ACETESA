using System.Collections.Generic;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.IRepository;

namespace Acetesa.TomaPedidos.Core.Business
{
    public class CondicionesVentasService : ICondicionesVentasService
    {
        private readonly ICondicionesVentasRepository _condicionesVentasRepository;

        public CondicionesVentasService(ICondicionesVentasRepository condicionesVentasRepository)
        {
            _condicionesVentasRepository = condicionesVentasRepository;
        }

        public IEnumerable<CondicionVentaModel> GetAll()
        {
            var query = _condicionesVentasRepository.GetAll();
            return query;
        }

        public IEnumerable<CondicionVentaModel> GetAll(string ccAnalis)
        {
            var query = _condicionesVentasRepository.GetAll(ccAnalis);
            return query;
        }
        public List<CondicionVentaModel> RecuperarCondicionVentaPorClienteID(string cc_analis)
        {
            return _condicionesVentasRepository.RecuperarCondicionVentaPorClienteID(cc_analis);
        }
    }
}
