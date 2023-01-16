using System;
using System.Collections.Generic;
using System.Linq;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.IRepository;

namespace Acetesa.TomaPedidos.Core.Business
{
    public class EstadoCuentaService : IEstadoCuentaService
    {
        private readonly IEstadoCuentaRepository _estadoCuentaRepository;

        public EstadoCuentaService(IEstadoCuentaRepository estadoCuentaRepository)
        {
            if (estadoCuentaRepository == null)
            {
                throw new ArgumentNullException("estadoCuentaRepository");
            }
            _estadoCuentaRepository = estadoCuentaRepository;
        }

        public EstadoCuentaResumenModel GetResumenByRuc(string ruc)
        {
            if (string.IsNullOrEmpty(ruc) || string.IsNullOrWhiteSpace(ruc))
            {
                throw new ArgumentNullException("ruc");
            }
            var query = _estadoCuentaRepository.GetResumenByRuc(ruc);
            return query;
        }

        public IEnumerable<EstadoCuentaDetalleModel> GetDetalleByRuc(string ruc)
        {
            if (string.IsNullOrEmpty(ruc) || string.IsNullOrWhiteSpace(ruc))
            {
                throw new ArgumentNullException("ruc");
            }
            var query = _estadoCuentaRepository
                .GetDetalleByRuc(ruc)
                .ToList();

            if (query.Count <= 0) return null;

            const string sTotalSoles = "Total Soles";
            const string sTotalDolar = "Total Dólares";
            const string monedaSoles = "S/.";
            var querySoles = query
                .Where(w => w.Mon_Docum == monedaSoles)
                .OrderBy(o=>o.Fecha_Vencimiento)
                .ToList();

            IEnumerable<EstadoCuentaDetalleModel> queryTotalSoles = null;
            if (querySoles.Count > 0)
            {
                queryTotalSoles = new List<EstadoCuentaDetalleModel>{
                    new EstadoCuentaDetalleModel
                    {
                        Mon_Docum = sTotalSoles,
                        Tot_Docum = querySoles.Sum(c=>c.Tot_Docum),
                        Acta_Docum = querySoles.Sum(c=>c.Acta_Docum),
                        Sal_Docum = querySoles.Sum(c=>c.Sal_Docum)
                    }
                };
            }
            var totalSoles = queryTotalSoles != null ? querySoles.Union(queryTotalSoles) : querySoles;

            const string monedaDolares = "US$";
            var queryDolares = query
                .Where(w => w.Mon_Docum == monedaDolares)
                .OrderBy(o => o.Fecha_Vencimiento)
                .ToList();

            IEnumerable<EstadoCuentaDetalleModel> queryTotalDolares = null;
            if (queryDolares.Count > 0)
            {
                queryTotalDolares = new List<EstadoCuentaDetalleModel>
                {
                    new EstadoCuentaDetalleModel
                    {
                        Mon_Docum = sTotalDolar,
                        Tot_Docum = queryDolares.Sum(c=>c.Tot_Docum),
                        Acta_Docum = queryDolares.Sum(c=>c.Acta_Docum),
                        Sal_Docum = queryDolares.Sum(c=>c.Sal_Docum)
                    }
                };
            }

            var totalDolares = queryTotalDolares != null ? queryDolares.Union(queryTotalDolares) : queryDolares;

            var total = totalSoles.Union(totalDolares);

            return total;
        }
    }
}
