using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.IRepository
{
    public interface IPedidoRepository : IBase<LCPEDIDO_WEB>
    {
        IEnumerable<PedidoListadoModel> GetPedidosByClienteFecInicioFecFinal(string cliente,
            DateTime fechaInicio, DateTime fechaFinal, string estado);

        IEnumerable<PedidoListadoModel> GetPedidosByClienteFecInicioFecFinal(string cliente,
            DateTime fechaInicio, DateTime fechaFinal, string estado, string vendedor);

        LCPEDIDO_WEB GetLastId();
        LCPEDIDOADICIONAL_WEB GetAdicionalById(string cnPedido);
        void Delete(LCPEDIDO_WEB entity);
        void GuardarAdicional(LCPEDIDO_WEB entityMaster, string email, string Lugar, string Transporte, string Observacion, string contacto, string IdContactoEntregaDirecta, string Tienda, DateTime FechaEntrega, int IncluyeIGV, string cn_ocompra, int zonaLiberada, string ObservacionGuia);
        Dictionary<string, string> RegistrarDocumentoOCPorPedido(string idPedido, string usuarioRegistro, byte[] documento);
        Dictionary<string, string> ValidaCreditoSobregiroPorPedido(string ruc, decimal total, string moneda);
        Dictionary<string, string> RegistrarNotaPedidoVenta(LCPEDIDO_WEB Pedido, LCPEDIDOADICIONAL_WEB PedidoAdicional, string DetallePedido, string correoVendedor);
        string RecuperarNumeroPedidoByProformaID(string ProformaID);
        string AsignarNumeroProformaAPedido(string ProformaID, string PedidoID);
        LCPEDIDO_WEB GetLastPedido();
        LCPEDIDO_WEB RecuperarDatosPedidoByID(string PedidoID);
        void EliminarPedidoByID(string PedidoID);
        void GuardarCabeceraPedido(LCPEDIDO_WEB Pedido);
        void GuardarDetallePedido(LDPEDIDO_WEB DetallePedido);
        void RegistrarDocumentoPedido(string PedidoID, byte[] Documento);


    }
}
