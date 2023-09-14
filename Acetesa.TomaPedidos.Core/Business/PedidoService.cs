using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;
using Acetesa.TomaPedidos.Transversal.Extensions;
using System.Xml.Linq;

namespace Acetesa.TomaPedidos.Core.Business
{
    public class PedidoService : IPedidoService
    {
        private readonly IDbContext _dbContext;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IPedidoDetalleRepository _pedidoDetalleRepository;
        private readonly IProductoRepository _productoRepository;

        public PedidoService(
            IDbContext dbContext,
            IPedidoRepository pedidoRepository,
            IPedidoDetalleRepository pedidoDetalleRepository,
            IProductoRepository productoRepository)
        {
            _dbContext = dbContext;
            _pedidoRepository = pedidoRepository;
            _pedidoDetalleRepository = pedidoDetalleRepository;
            _productoRepository = productoRepository;
        }

        public IEnumerable<PedidoListadoModel> GetPedidosByClienteFecInicioFecFinal(
            string cliente, DateTime fechaInicio, DateTime fechaFinal,string estado)
        {
            var query = _pedidoRepository.GetPedidosByClienteFecInicioFecFinal(cliente, fechaInicio, fechaFinal, estado);
            return query;
        }

        public IEnumerable<PedidoListadoModel> GetPedidosByClienteFecInicioFecFinal(
            string cliente, DateTime fechaInicio, DateTime fechaFinal, string estado, string vendedor)
        {
            var query = _pedidoRepository.GetPedidosByClienteFecInicioFecFinal(cliente, fechaInicio, fechaFinal, estado, vendedor);
            return query;
        }

        public LCPEDIDO_WEB GetById(string cnPedido)
        {
            if (string.IsNullOrEmpty(cnPedido) || string.IsNullOrWhiteSpace(cnPedido))
            {
                throw new ArgumentNullException("cnPedido");
            }
            var query = _pedidoRepository.GetById(cnPedido.Trim());
            return query;
        }

        public LCPEDIDOADICIONAL_WEB GetAdicionalById(string cnPedido)
        {
            if (string.IsNullOrEmpty(cnPedido) || string.IsNullOrWhiteSpace(cnPedido))
            {
                throw new ArgumentNullException("cnPedido");
            }
            var result = _pedidoRepository.GetAdicionalById(cnPedido);
            return result;
        }

        public string GetLastId()
        {
            var query = _pedidoRepository.GetLastId();
            if (query == null) return (1).ToString().PadLeft(8, '0');
            var sCnPedido = query.cn_pedido;
            var nCnPedido = Convert.ToInt32(sCnPedido);
            var sCnPedidoNew = (nCnPedido + 1).ToString().PadLeft(8, '0');
            return sCnPedidoNew;
        }

        public void Guardar(LCPEDIDO_WEB entity, int igv_bo, string empresa, int zonaLiberada)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                var cnPedido = entity.cn_pedido;
                DateTime? dFechaProceso = null;
                string cbEstado = null;
                if (string.IsNullOrEmpty(cnPedido) || string.IsNullOrWhiteSpace(cnPedido))
                {
                    cnPedido = GetLastId();
                }
                else
                {
                    var entityExiste = _pedidoRepository.GetById(cnPedido);
                    dFechaProceso = entityExiste.df_proceso;
                    cbEstado = entityExiste.cb_estado;
                }
                var dFechaActual = DateTimeExtension.GetDateTimeNow();
                entity.cn_pedido = cnPedido;
                entity.df_proceso = dFechaProceso ?? dFechaActual;

                //Solo si no se ha seteado el estado del EntityModel
                if (string.IsNullOrEmpty(entity.cb_estado)) {
                    entity.cb_estado = cbEstado ?? "1";
                }

                var SubTotVta = PedidoDetalleServices.Sum(x => x.fm_total);
                decimal IgvVta;
                decimal TotVta;
                if (empresa == "ACETESA")
                {
                    /*Nuevo: Calcula IGV segun seleccion usuario*/
                    var igv = 0.18;
                    if (igv_bo == 0)
                    {
                        IgvVta = SubTotVta * (decimal)(igv);
                        TotVta = SubTotVta + IgvVta;
                    }
                    else
                    {
                        TotVta = SubTotVta;
                        SubTotVta = TotVta / (decimal)(igv + 1);
                        IgvVta = SubTotVta * (decimal)(igv);
                    }
                    /*Fin: Calcula IGV segun seleccion usuario*/
                }
                else
                {
                    var igv = 0.18;
                    if (zonaLiberada == 0)
                    {
                        IgvVta = SubTotVta * (decimal)(igv);
                        TotVta = SubTotVta + IgvVta;
                    }
                    else
                    {
                        IgvVta = 0;
                        TotVta = SubTotVta;
                    }
                }



                entity.fm_valvta = SubTotVta;
                entity.fm_igv = IgvVta;
                entity.fm_totvta = TotVta;

                entity.fm_valvta = Math.Round(entity.fm_valvta, 2);
                entity.fm_igv = Math.Round(entity.fm_igv, 2);
                entity.fm_totvta = Math.Round(entity.fm_totvta, 2);

                if (!string.IsNullOrEmpty(cnPedido) && !string.IsNullOrWhiteSpace(cnPedido))
                {
                    DeleteMasterAndDetail(entity);
                }

                _pedidoRepository.Add(entity);

                foreach (var item in PedidoDetalleServices)
                {
                    item.cn_pedido = cnPedido;
                    item.fm_total = Math.Round(item.fm_total, 2);
                    var articuloModel = _productoRepository.GetById(item.cc_artic);
                    item.fq_peso = (decimal)(articuloModel.fq_peso_teorico * (double)item.fq_cantidad ?? 0);
                    item.fq_peso = Math.Round(item.fq_peso, 2);


                    _pedidoDetalleRepository.Add(item);
                }

                _dbContext.Commit();

                scope.Complete();
            }
        }
        public void DeleteMasterAndDetail(LCPEDIDO_WEB master)
        {
            var entityMaster = _pedidoRepository.GetById(master.cn_pedido);
            if (entityMaster == null) return;
            foreach (var entityDetail in entityMaster.LDPEDIDO_WEB.ToList())
            {
                _pedidoDetalleRepository.Delete(entityDetail);
            }
            _pedidoRepository.Delete(entityMaster);
            _dbContext.Commit();
        }
        public IEnumerable<LDPEDIDO_WEB> PedidoDetalleServices { get; set; }
        public void UpdateEstado(string cnProforma, string estado)
        {
            var entity = _pedidoRepository.GetById(cnProforma);
            if (entity == null) return;
            entity.cb_estado = estado;
            _pedidoRepository.Update(entity);
            _dbContext.Commit();
        }

        public void GuardarAdicional(LCPEDIDO_WEB entityMaster, string email, string Lugar, string Transporte, string Observacion, string contacto, string IdContactoEntregaDirecta, string Tienda, DateTime FechaEntrega, int IncluyeIGV, string cn_ocompra, int zonaLiberada)
        {
            if (cn_ocompra == null)
            {
                cn_ocompra = "";
            }
            _pedidoRepository.GuardarAdicional(entityMaster, email, Lugar, Transporte, Observacion, contacto, IdContactoEntregaDirecta, Tienda, FechaEntrega, IncluyeIGV, cn_ocompra,zonaLiberada);
        }
        public Dictionary<string, string> RegistrarDocumentoOCPorPedido(string idPedido, string usuarioRegistro, byte[] documento)
        {
            return _pedidoRepository.RegistrarDocumentoOCPorPedido(idPedido,usuarioRegistro,documento);
        }
        public Dictionary<string, string> ValidaCreditoSobregiroPorPedido(string ruc, decimal total, string moneda)
        {
            return _pedidoRepository.ValidaCreditoSobregiroPorPedido(ruc,total,moneda);
        }
        public Dictionary<string, string> RegistrarNotaPedidoVenta(LCPEDIDO_WEB Pedido, LCPEDIDOADICIONAL_WEB PedidoAdicional, string DetallePedido, string correoVendedor)        {
            return _pedidoRepository.RegistrarNotaPedidoVenta(Pedido,PedidoAdicional,DetallePedido,correoVendedor);
        }

        public string RecuperarNumeroPedidoByProformaID(string ProformaID)
        {
            return _pedidoRepository.RecuperarNumeroPedidoByProformaID(ProformaID);
        }
        public string AsignarNumeroProformaAPedido(string ProformaID, string PedidoID)
        {
            return _pedidoRepository.AsignarNumeroProformaAPedido(ProformaID, PedidoID);
        }

    }
}
