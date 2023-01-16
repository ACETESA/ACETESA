using System.Linq;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;

namespace Acetesa.TomaPedidos.Repository
{
    public class CotizacionDetalleRepository : BaseRepository<LDPROF_WEB>, ICotizacionDetalleRepository
    {
        private readonly IDbContext _dbContext;

        public CotizacionDetalleRepository(IDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public LDPROF_WEB GetByIdCnItem(string cnProforma, string cnItem)
        {
            var query = _dbContext.Query<LDPROF_WEB>()
                .FirstOrDefault(x=>x.cn_proforma == cnProforma.Trim() && x.cn_item == cnItem.Trim());
            return query;
        }

        public void Delete(LDPROF_WEB entity)
        {
            _dbContext.Delete(entity);
        }
    }
}
