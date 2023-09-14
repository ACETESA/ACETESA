using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.Core.IBusiness
{
    public interface IPedidoService
    {
        IEnumerable<PedidoListadoModel> GetPedidosByClienteFecInicioFecFinal(
            string cliente, DateTime fechaInicio, DateTime fechaFinal, string estado);
        IEnumerable<PedidoListadoModel> GetPedidosByClienteFecInicioFecFinal(
            string cliente, DateTime fechaInicio, DateTime fechaFinal, string estado, string vendedor);
        LCPEDIDO_WEB GetById(string cnPedido);
        LCPEDIDOADICIONAL_WEB GetAdicionalById(string cnPedido);
        string GetLastId();
        void Guardar(LCPEDIDO_WEB entity, int igv_bo, string empresa, int zonaLiberada);
        void GuardarAdicional(LCPEDIDO_WEB entity, string email, string Lugar, string Transporte, string Observacion, string contacto, string IdContactoEntregaDirecta, string Tienda, DateTime FechaEntrega, int IncluyeIGV, string cn_ocompra, int zonaLiberada);
        IEnumerable<LDPEDIDO_WEB> PedidoDetalleServices { get; set; }
        void UpdateEstado(string cnProforma, string estado);
        Dictionary<string, string> RegistrarDocumentoOCPorPedido(string idPedido, string usuarioRegistro, byte[] documento);
        Dictionary<string, string> ValidaCreditoSobregiroPorPedido(string ruc, decimal total, string moneda);
        Dictionary<string, string> RegistrarNotaPedidoVenta(LCPEDIDO_WEB Pedido, LCPEDIDOADICIONAL_WEB PedidoAdicional, string DetallePedido, string correoVendedor);
        string RecuperarNumeroPedidoByProformaID(string ProformaID);
        string AsignarNumeroProformaAPedido(string ProformaID, string PedidoID);

    }
}