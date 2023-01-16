using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class PedidoClienteViewModel
    {
        public LCPEDIDO_WEB Pedido { get; set; }
        public ClienteModel Cliente { get; set; }
        public LCPEDIDOADICIONAL_WEB Adicional { get; set; }
    }
}