using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.IRepository
{
    public interface IPedidoDetalleRepository : IBase<LDPEDIDO_WEB>
    {
        LDPEDIDO_WEB GetByIdCnItem(string cnPedido, string cnItem);
        void Delete(LDPEDIDO_WEB entity);
    }
}
