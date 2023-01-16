using System.Linq;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;

namespace Acetesa.TomaPedidos.Repository
{
    public class EstadoRepository : BaseRepository<TESTADO>, IEstadoRepository
    {
        private readonly IDbContext _dbContext;

        public EstadoRepository(IDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TESTADO> GetAll()
        {
            var query = _dbContext.Query<TESTADO>().OrderBy(o => o.cn_orden);
            return query;
        }
    }
}
