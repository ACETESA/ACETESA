using System.Linq;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;

namespace Acetesa.TomaPedidos.Repository
{
    public class MonedaRepository : BaseRepository<MonedaModel>, IMonedaRepository
    {
        private readonly IDbContext _dbContext;

        public MonedaRepository(IDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<MonedaModel> GetAll()
        {
            var idsMonedasValidas = new[] { "01", "02" };
            var query = _dbContext.Query<TMONEDA>()
                .Where(x => idsMonedasValidas.Contains(x.cc_moneda))
                .Select(x => new MonedaModel
            {
                cc_moneda = x.cc_moneda,
                cd_simbolo = x.cd_simbolo
            });
            return query;
        }

        public MonedaModel GetCdMonedaByCcMoneda(string ccMoneda)
        {
            var query = _dbContext.Query<TMONEDA>()
                .Where(x => x.cc_moneda == ccMoneda.Trim())
                .Select(s => new MonedaModel
                {
                    cd_simbolo = s.cd_simbolo
                })
                .FirstOrDefault();
            return query;
        }
    }
}
