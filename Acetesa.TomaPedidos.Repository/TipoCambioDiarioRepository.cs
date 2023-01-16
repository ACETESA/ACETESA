using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;

namespace Acetesa.TomaPedidos.Repository
{
    public class TipoCambioDiarioRepository : BaseRepository<tipo_cambio_diario>, ITipoCambioDiarioRepository
    {
        private readonly IDbContext _dbContext;

        public TipoCambioDiarioRepository(IDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public tipo_cambio_diario GetByFechaTipoCambio(DateTime fechaTipoCambio)
        {
            var fecha = fechaTipoCambio.Date;
            var query = _dbContext.Query<tipo_cambio_diario>()
                .Where(x => x.d_f_tipo_cambio.Year == fecha.Year
                && x.d_f_tipo_cambio.Month == fecha.Month
                && x.d_f_tipo_cambio.Day == fecha.Day)
                .OrderByDescending(o=>o.d_f_tipo_cambio)
                .FirstOrDefault();
            return query;
        }
    }
}
