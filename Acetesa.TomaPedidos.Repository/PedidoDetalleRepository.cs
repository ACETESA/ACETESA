using System.Linq;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;

namespace Acetesa.TomaPedidos.Repository
{
    public class PedidoDetalleRepository : BaseRepository<LDPEDIDO_WEB>, IPedidoDetalleRepository
    {
        private readonly IDbContext _dbContext;

        public PedidoDetalleRepository(IDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public LDPEDIDO_WEB GetByIdCnItem(string cnPedido, string cnItem)
        {
            var query = _dbContext.Query<LDPEDIDO_WEB>()
                .FirstOrDefault(x => x.cn_pedido == cnPedido.Trim() && x.cn_item == cnItem.Trim());
            return query;
        }

        public void Delete(LDPEDIDO_WEB entity)
        {
            _dbContext.Delete(entity);
        }
    }
}
