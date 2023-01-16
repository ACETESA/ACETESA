using System.Collections.Generic;
using System.Linq;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.IRepository;

namespace Acetesa.TomaPedidos.Core.Business
{
    public class MonedaService : IMonedaService
    {
        private readonly IMonedaRepository _monedaRepository;

        public MonedaService(IMonedaRepository monedaRepository)
        {
            _monedaRepository = monedaRepository;
        }

        public IEnumerable<MonedaModel> GetAll()
        {
            var query = _monedaRepository.GetAll().ToList();
            return query;
        }

        public MonedaModel GetCdMonedaByCcMoneda(string ccMoneda)
        {
            var query = _monedaRepository.GetCdMonedaByCcMoneda(ccMoneda);
            return query;
        }
    }
}
