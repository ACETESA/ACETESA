﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.Repository;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;
using Acetesa.TomaPedidos.Transversal.Extensions;
using System.Web;


namespace Acetesa.TomaPedidos.Core.Business
{
    public class CotizacionService : ICotizacionService
    {
        private readonly ICotizacionRepository _cotizacionRepository;
        private readonly ICotizacionDetalleRepository _cotizacionDetalleRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IDbContext _dbContext;

        public CotizacionService(
            ICotizacionRepository cotizacionRepository,
            ICotizacionDetalleRepository cotizacionDetalleRepository,
            IProductoRepository productoRepository,
            IDbContext dbContext)
        {
            if (cotizacionRepository == null)
            {
                throw new ArgumentNullException("cotizacionRepository");
            }
            _cotizacionRepository = cotizacionRepository;
            _cotizacionDetalleRepository = cotizacionDetalleRepository;
            _productoRepository = productoRepository;
            _dbContext = dbContext;
        }

        public IEnumerable<CotizacionListadoModel> GetCotizacionesByClienteFecInicioFecFinal(string cliente, DateTime fechaInicio, DateTime fechaFinal, string estado)
        {
            var query = _cotizacionRepository.GetCotizacionesByClienteFecInicioFecFinal(cliente, fechaInicio, fechaFinal, estado);
            return query;
        }

        public IEnumerable<CotizacionListadoModel> GetCotizacionesByClienteFecInicioFecFinal(string cliente, DateTime fechaInicio, DateTime fechaFinal, string vendedor, string estado)
        {
            var query = _cotizacionRepository.GetCotizacionesByClienteFecInicioFecFinal(cliente, fechaInicio, fechaFinal, vendedor, estado);
            return query;
        }

        public LCPROF_WEB GetById(string cnProforma)
        {
            if (string.IsNullOrEmpty(cnProforma) || string.IsNullOrWhiteSpace(cnProforma))
            {
                throw new ArgumentNullException("cnProforma");
            }
            var query = _cotizacionRepository.GetById(cnProforma.Trim());
            return query;
        }

        public LCPROF_WEB GetLastCnProforma()
        {
            //var query = _cotizacionRepository.GetLastCotizacion();
            //if (query == null) return (1).ToString().PadLeft(8, '0');
            //var sCnProforma = query.cn_proforma;
            //var nCnProforma = Convert.ToInt32(sCnProforma);
            //var sCnProformaNew = (nCnProforma + 1).ToString().PadLeft(8, '0');
            //return sCnProformaNew;
            var query = _cotizacionRepository.GetLastCotizacion();
            return query;
        }

        public void Guardar(LCPROF_WEB entity, int igv_bo, string empresa, int zonaLiberada)
        {
            try
            {
                string sPedidos = "";
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    var cnProforma = entity.cn_proforma;
                    DateTime? dFechaProceso = null;
                    string cbEstado = null;
                    if (string.IsNullOrEmpty(cnProforma) || string.IsNullOrWhiteSpace(cnProforma))
                    {
                        LCPROF_WEB proforma = GetLastCnProforma();
                        cnProforma = proforma.cn_proforma;
                    }
                    else
                    {
                        var entityExiste = _cotizacionRepository.GetById(cnProforma);
                        dFechaProceso = entityExiste.df_proceso;
                        cbEstado = entityExiste.cb_estado;
                    }
                    var dFechaActual = DateTimeExtension.GetDateTimeNow();
                    entity.cn_proforma = cnProforma;
                    entity.df_proceso = dFechaProceso ?? dFechaActual;
                    entity.cb_estado = cbEstado ?? "1";


                    var SubTotVta = CotizacionDetalleServices.Sum(x => x.fm_total);
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

                    if (!string.IsNullOrEmpty(cnProforma) && !string.IsNullOrWhiteSpace(cnProforma))
                    {
                        sPedidos = PedidoRepository.getCotizaciones(cnProforma);
                        DeleteMasterAndDetail(entity);
                    }

                    _cotizacionRepository.Add(entity);

                    foreach (var item in CotizacionDetalleServices)
                    {
                        item.cn_proforma = cnProforma;
                        item.fm_total = Math.Round(item.fm_total, 2);
                        var articuloModel = _productoRepository.GetById(item.cc_artic);
                        item.fq_peso = (decimal)(articuloModel.fq_peso_teorico * (double)item.fq_cantidad ?? 0);
                        item.fq_peso = Math.Round(item.fq_peso, 2);
                        _cotizacionDetalleRepository.Add(item);
                    }

                    _dbContext.Commit();

                    scope.Complete();
                }
                if (sPedidos != "")
                {
                    foreach (var pedido in sPedidos.Split(','))
                    {
                        if (!string.IsNullOrEmpty(pedido))
                        {
                            PedidoRepository.setCotizaciones(entity.cn_proforma, pedido);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var a = e;
                Console.WriteLine(e);
            }
 
        }

        public void DeleteMasterAndDetail(LCPROF_WEB master)
        {
            var entityMaster = _cotizacionRepository.GetById(master.cn_proforma);
            if (entityMaster == null) return;
            foreach (var entityDetail in entityMaster.LDPROF_WEB.ToList())
            {
                _cotizacionDetalleRepository.Delete(entityDetail);
            }
            _cotizacionRepository.Delete(entityMaster);
            _dbContext.Commit();
        }

        public IEnumerable<LDPROF_WEB> CotizacionDetalleServices { get; set; }

        public void UpdateEstado(string cnProforma, string estado)
        {
            var entity = _cotizacionRepository.GetById(cnProforma.Trim());
            if (entity == null) return;
            entity.cb_estado = estado;
            _cotizacionRepository.Update(entity);
            _dbContext.Commit();
        }

        public void GuardarAdicional(LCPROF_WEB entityMaster, string email, string Tienda, int IncluyeIGV, string cn_suc, string cn_contacto, bool imprimirPrecioTN, string observacion, int zonaLiberada)
        {
            _cotizacionRepository.GuardarAdicional(entityMaster, email, Tienda, IncluyeIGV, cn_suc, cn_contacto, imprimirPrecioTN, observacion,zonaLiberada);
        }
        public LCPROFADICIONAL_WEB GetAdicionalById(string cnCotizacion)
        {
            if (string.IsNullOrEmpty(cnCotizacion) || string.IsNullOrWhiteSpace(cnCotizacion))
            {
                throw new ArgumentNullException("cnCotizacion");
            }
            var result = _cotizacionRepository.GetAdicionalById(cnCotizacion);
            return result;
        }
        public string AnularRestablecerProforma(string cn_proforma) {
            return _cotizacionRepository.AnularRestablecerProforma(cn_proforma);
        }

        public List<Entity.Consultas.DetalleCotizacion> DatosDetalleProformaParaPedido(string cn_proforma)
        {
            return _cotizacionRepository.DatosDetalleProformaParaPedido(cn_proforma);
        }
        public List<CotizacionMotivoRechazo> ListaMotivosRechazoCotizacion()
        {
            return _cotizacionRepository.ListaMotivosRechazoCotizacion();
        }
        public Dictionary<string, string> RegistrarRechazoCotizacion(string cn_proforma, int idMotivo, string mensajeRechazo) {
            return _cotizacionRepository.RegistrarRechazoCotizacion(cn_proforma, idMotivo, mensajeRechazo);
        }

        public Dictionary<string, string> RegistrarCierreCotizacionParcial(string cn_proforma, int idMotivo, string mensajeRechazo)
        {
            return _cotizacionRepository.RegistrarCierreCotizacionParcial(cn_proforma, idMotivo, mensajeRechazo);
        }

    }

}
