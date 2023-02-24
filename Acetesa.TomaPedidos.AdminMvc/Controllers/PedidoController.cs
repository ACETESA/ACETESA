using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Acetesa.TomaPedidos.AdminMvc.Infrastructure;
using Acetesa.TomaPedidos.AdminMvc.Models;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.Repository;
using Acetesa.TomaPedidos.Transversal;
using Acetesa.TomaPedidos.Transversal.Enums;
using Acetesa.TomaPedidos.Transversal.Extensions;
using MvcRazorToPdf;
using PagedList;
using System.Net;
using System.Net.Http;
using Acetesa.TomaPedidos.AdminMvc.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.IO;
using System.Web;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace Acetesa.TomaPedidos.AdminMvc.Controllers
{
    [Authorize]
    public class PedidoController : BaseController
    {
        private const int PageSize = 50;

        public IClienteService ClienteService { get; set; }
        public IEstadoService EstadoService { get; set; }
        public IPedidoService PedidoService { get; set; }
        public IVendedorService VendedorService { get; set; }
        public IPedidoDetalleService PedidoDetalleService { get; set; }
        public IMonedaService MonedaService { get; set; }
        public ICondicionesVentasService CondicionesVentasService { get; set; }
        public IArticuloService ArticuloService { get; set; }
        public IListaPrecioService ListaPrecioService { get; set; }
        public ITipoCambioDiarioService TipoCambioDiarioService { get; set; }
        public ISucursalClienteService SucursalClienteService { get; set; }
        public ICotizacionService CotizacionService { get; set; }
        public ITiendaService TiendaService { get; set; }
        public IProductoService ProductoService { get; set; }

        public string EmpresaSegunBD()
        {
            var empresa = "";
            var sqlDB = new System.Data.SqlClient.SqlConnectionStringBuilder(
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            if (sqlDB.InitialCatalog == "ZICO_ERP04")
            {
                empresa = "GALPESA";
            }
            else
            {
                empresa = "ACETESA";
            }
            return empresa;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Listado(int? page)
        {
            ViewBag.BaseUrl = Url.Content("~/");
            ViewBag.GetClientes = GetClientes();
            ViewBag.GetEstados = GetEstados();
            var model = new PedidoFindViewModel { EnviarMailViewModel = new EnviarMailViewModel() };
            var pageNumber = (page ?? 1);
            try
            {
                var fechaInicio = (model.FechaInicio + " 00:00:00").ConvertDateTime();
                var fechaFinal = (model.FechaFinal + " 23:59:59").ConvertDateTime();
                var estadoEnviado = ((int)EstadoPedidoTypes.Emitido).ToString();
                var result = PedidoService.GetPedidosByClienteFecInicioFecFinal(model.Cliente, fechaInicio, fechaFinal, estadoEnviado, User.Identity.Name);
                //model.PagedListListaEntity = result.ToPagedList(pageNumber, PageSize);
                model.CotizacionModels = result;
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return JsonError(ex.Message);
                }
                ModelState.AddModelError(string.Empty, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                return View(model);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialCotizacionList", model);
            }


            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Listado(PedidoFindViewModel model, int? page)
        {
            ViewBag.BaseUrl = Url.Content("~/");
            model.EnviarMailViewModel = new EnviarMailViewModel();
            ViewBag.GetClientes = GetClientes();
            ViewBag.GetEstados = GetEstados();

            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    return JsonValidationError();
                }
                return View();
            }

            var pageNumber = (page ?? 1);
            try
            {
                var fechaInicio = (model.FechaInicio + " 00:00:00").ConvertDateTime();
                var fechaFinal = (model.FechaFinal + " 23:59:59").ConvertDateTime();
                var estado = ((int)model.Estado).ToString();
                var result = PedidoService.GetPedidosByClienteFecInicioFecFinal(model.Cliente, fechaInicio, fechaFinal, estado, User.Identity.Name);
                //model.PagedListListaEntity = result.ToPagedList(pageNumber, PageSize);
                model.CotizacionModels = result;
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return JsonError(ex.Message);
                }
                ModelState.AddModelError(string.Empty, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                return View(model);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialPedidoList", model);
            }
            return View(model);
        }

        private string _cnPedido;
        private string _cnProforma;
        private string _ccAnalis;
        private string _ccMoneda;
        private string _ccVta;
        private string _cnSuc;
        private string _Cn_lug;
        private string _IdContactoEntregaDirecta;
        private string _CC_transp;
        private string _ContactoTransporte;
        private string _Vt_observacion;
        private string _cn_ocompra;
        private int _zonaLiberada_bo;
        private string _cbRecojo;
        private string _Tienda;
        private int _Igv_bo;
        private string _cFechaEmision;
        private string _cdAtencion;
        private decimal _fmTipCam;
        private string _ccTipana;
        private string _cdRazsoc;
        private string _cFechaEntrega;

        private SessionTypes _sessionTypes = SessionTypes.NoAsignado;

        private const string SessionCcTipanaNuevo = "pedido_cc_tipana_nuevo";
        private const string SessionCcTipanaEditar = "pedido_cc_tipana_editar";
        private const string SessionCcAnalisNuevo = "pedido_cc_analis_nuevo";
        private const string SessionCcAnalisEditar = "pedido_cc_analis_editar";
        private const string SessionCdRazSocNuevo = "pedido_cd_razsoc_nuevo";
        private const string SessionCdRazSocEditar = "pedido_cd_razsoc_editar";
        private const string SessionCcMonedaNuevo = "pedido_cc_moneda_nuevo";
        private const string SessionCcMonedaEditar = "pedido_cc_moneda_editar";
        private const string SessionCcVtaNuevo = "pedido_cc_vta_nuevo";
        private const string SessionCcVtaEditar = "pedido_cc_vta_editar";
        private const string SessionCfechaEmisionNuevo = "pedido_fecha_emision_nuevo";
        private const string SessionCfechaEmisionEditar = "pedido_fecha_emision_editar";
        private const string SessionCfechaEntregaNuevo = "pedido_fecha_entrega_nuevo";
        private const string SessionCfechaEntregaEditar = "pedido_fecha_entrega_editar";
        private const string SessionCdAtencionNuevo = "pedido_cd_atencion_nuevo";
        private const string SessionCdAtencionEditar = "pedido_cd_atencion_editar";
        private const string SessionFmTipCamNuevo = "pedido_fm_tipcam_nuevo";
        private const string SessionFmTipCamEditar = "pedido_fm_tipcam_editar";
        private const string SessionDetalleNuevo = "pedido_DetailNuevo";
        private const string SessionDetalleEditar = "pedido_DetailEditar";
        private const string SessionCnSucNuevo = "pedido_cn_suc_nuevo";
        private const string SessionCnSucEditar = "pedido_cn_suc_editar";
        private const string SessionCn_lugNuevo = "pedido_Cn_lug_nuevo";
        private const string SessionCn_lugEditar = "pedido_Cn_lug_editar";
        private const string SessionIdContactoEntregaDirectaNuevo = "pedido_IdContactoEntregaDirecta_nuevo";
        private const string SessionIdContactoEntregaDirectaEditar = "pedido_IdContactoEntregaDirecta_editar";
        private const string SessionCC_transpNuevo = "pedido_CC_transp_nuevo";
        private const string SessionCC_transpEditar = "pedido_CC_transp_editar";
        private const string SessionContactoTransporteNuevo = "pedido_ContactoTransporte_nuevo";
        private const string SessionContactoTransporteEditar = "pedido_ContactoTransporte_editar";
        private const string SessionVt_observacionNuevo = "pedido_Vt_observacion_nuevo";
        private const string SessionVt_observacionEditar = "pedido_Vt_observacion_editar";
        private const string SessionCn_ocompraNuevo = "pedido_Cn_ocompra_nuevo";
        private const string SessionCn_ocompraEditar = "pedido_Cn_ocompra_editar";
        private const string SessionZonaLiberadaNuevo = "pedido_ZonaLiberada_nuevo";
        private const string SessionZonaLiberadaEditar = "pedido_ZonaLiberada_editar";
        private const string SessionCbRecojoNuevo = "pedido_cb_recojo_nuevo";
        private const string SessionCbRecojoEditar = "pedido_cb_recojo_editar";
        private const string SessionTiendaNuevo = "pedido_tienda_nuevo";
        private const string SessionTiendaEditar = "pedido_tienda_editar";
        private const string SessionIgv_boNuevo = "pedido_igv_bo_nuevo";
        private const string SessionIgv_boEditar = "pedido_igv_bo_editar";

        private string _sessionCcTipana;
        private string _sessionCcAnalis;
        private string _sessionCdRazSoc;
        private string _sessionCcMoneda;
        private string _sessionCcVta;
        private string _sessionCnSuc;
        private string _sessionCn_lug;
        private string _sessionIdContactoEntregaDirecta;
        private string _sessionCC_transp;
        private string _sessionContactoTransporte;
        private string _sessionVt_observacion;
        private string _sessionCn_ocompra;
        private string _sessionZonaLiberada;
        private string _sessionCbRecojo;
        private string _sessionTienda;
        private string _sessionIgv_bo;
        private string _sessionCfechaEmision;
        private string _sessionCfechaEntrega;
        private string _sessionCdAtencion;
        private string _sessionFmTipCam;
        private string _sessionDetalle;

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetSucursalesJson(string ccAnalis)
        {
            return JsonSuccess(GetSucursalesCliente(ccAnalis));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetLugarEntregaJson(string ccAnalis)
        {
            return JsonSuccess(GetLugarEntregaCliente(ccAnalis));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetContactoEntregaDirectaJson(string ccAnalis)
        {
            return JsonSuccess(GetContactoEntregaDirecta(ccAnalis));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetTransporteJson(string ccAnalis)
        {
            return JsonSuccess(GetTransporteCliente(ccAnalis));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetTransporteJsonTodos(string ccAnalis)
        {
            var datos = GetTransporteClienteTodos(ccAnalis);
            JArray arrRetorno = null;
            if (datos == null)
            {
                arrRetorno = new JArray();
            }
            else
            {
                arrRetorno = JArray.FromObject(datos);
                for (var i = 0; i < arrRetorno.Count; i++)
                {
                    var jObjeto = arrRetorno[i] as JObject;
                    var oCheck = new jsDataTableHelper.ValorCeldaCheckBox("chkTransporte", jObjeto["codigo"].ToString(), (jObjeto["Seleccionado"].ToString() == "S"));
                    jObjeto["check"] = JToken.FromObject(oCheck);
                }
            }
            return JsonSuccess(arrRetorno);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetLugarEntregaJsonTodos(string ccAnalis)
        {
            var datos = GetLugarEntregaClienteTodos(ccAnalis);
            JArray arrRetorno = null;
            if (datos == null)
            {
                arrRetorno = new JArray();
            }
            else
            {
                arrRetorno = JArray.FromObject(datos);
            }
            return JsonSuccess(arrRetorno);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetContactoEntregaDirectaJsonTodos(string ccAnalis)
        {
            var datos = GetContactoEntregaDirectaClienteTodos(ccAnalis);
            JArray arrRetorno = null;
            if (datos == null)
            {
                arrRetorno = new JArray();
            }
            else
            {
                arrRetorno = JArray.FromObject(datos);
            }
            return JsonSuccess(arrRetorno);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetContactoTransporteJsonTodos(string transporte)
        {
            var datos = ContactoTransportistaRepository.getByTransportista(transporte);
            JArray arrRetorno = null;
            if (datos == null)
            {
                arrRetorno = new JArray();
            }
            else
            {
                arrRetorno = JArray.FromObject(datos);
                for (var i = 0; i < arrRetorno.Count; i++)
                {
                    var jObjeto = arrRetorno[i] as JObject;
                    var oCheck = new jsDataTableHelper.ValorCeldaCheckBox("chkTransporte", jObjeto["codigo"].ToString(), (jObjeto["Seleccionado"].ToString() == "S"));
                    jObjeto["check"] = JToken.FromObject(oCheck);
                }
            }
            return JsonSuccess(arrRetorno);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetContactoTransporteJson(string transporte)
        {
            var datos = ContactoTransportistaRepository.getByTransportistaActivos(transporte);
            JArray arrRetorno = null;
            if (datos == null)
            {
                arrRetorno = new JArray();
            }
            else
            {
                arrRetorno = JArray.FromObject(datos);
            }
            return JsonSuccess(arrRetorno);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult setTransporteCliente(string transporte, string estado, string cliente)
        {
            SucursalClienteRepository.setTransporteCliente(transporte, estado, cliente);
            return JsonSuccess(1);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetColumnasGrillaSelect()
        {
            var aColumnas = new List<jsDataTableHelper.ConfigColumna>();
            aColumnas.Add(new jsDataTableHelper.ConfigColumna("Razón Social", "descri") { Clase = "clickeable" });
            aColumnas.Add(new jsDataTableHelper.ConfigColumna("check", false, true, true, false, "check-datatable"));

            return JsonSuccess(aColumnas);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetColumnasLugarEntrega()
        {
            var aColumnas = new List<jsDataTableHelper.ConfigColumna>();
            aColumnas.Add(new jsDataTableHelper.ConfigColumna("Dirección", "descri") { Clase = "clickeable" });

            return JsonSuccess(aColumnas);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetColumnasContactoEntregaDirecta()
        {
            var aColumnas = new List<jsDataTableHelper.ConfigColumna>();
            aColumnas.Add(new jsDataTableHelper.ConfigColumna("Sucursal", "idSucursal", true, true, false, false, "cn_suc", false));
            aColumnas.Add(new jsDataTableHelper.ConfigColumna("Apellidos y Nombres", "nombreContacto") { Clase = "clickeable" });
            //aColumnas.Add(new jsDataTableHelper.ConfigColumna("", "", false, true, true, false,"") { Clase = "btn btn-default btn-sm glyphicon glyphicon-pencil" });

            return JsonSuccess(aColumnas);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetContactoEditar(string cc_analis, string cn_suc, string cn_contacto)
        {
            List<TCONTACLIE> ListaContactos = new List<TCONTACLIE>();
            ContactoSucursalRepository repository = new ContactoSucursalRepository();
            ListaContactos = repository.GetContactoParaEditar(cc_analis, cn_suc, cn_contacto);
            return JsonSuccess(ListaContactos);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult ActualizarContacto(List<TCONTACLIE> listaContactos)
        {
            ContactoSucursalRepository repository = new ContactoSucursalRepository();
            repository.ActualizarContacto(listaContactos);
            return JsonSuccess("");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetColumnasGrillaSelectContacto()
        {
            var aColumnas = new List<jsDataTableHelper.ConfigColumna>();
            aColumnas.Add(new jsDataTableHelper.ConfigColumna("Nombre", "descri") { Clase = "clickeable" });
            aColumnas.Add(new jsDataTableHelper.ConfigColumna("check", false, true, true, false, "check-datatable"));

            return JsonSuccess(aColumnas);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetCondicionesVentasJson(string ccAnalis)
        {
            return JsonSuccess(CondicionesVentas(ccAnalis));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetProvinciaJSON(string departamento)
        {
            return JsonSuccess(GetProvincia(departamento));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetDistritoJSON(string departamento, string provincia)
        {
            return JsonSuccess(GetDistrito(departamento, provincia));
        }
        public ActionResult GetZonaJSON(string departamento, string provincia, string distrito)
        {
            return JsonSuccess(GetZona(departamento, provincia, distrito));
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Nuevo(int? reset)
        {
            if (reset.HasValue && reset.Value == 1)
            {
                ViewBag.Success = "";
                Session.Clear();
            }

            ViewBag.BaseUrl = Url.Content("~/");
            _sessionTypes = SessionTypes.Nuevo;

            var cnProforma = (string)RouteData.Values["id"];
            var saved = (bool?)TempData["Guardado"];
            var emailModel = (EnviarMailViewModel)TempData["EmailModel"];
            var cnPedido = (string)TempData["CnPedido"];

            if (saved.HasValue && saved.Value)
            {
                ViewBag.Success = "Pedido creado.";
            }

            if (emailModel != null)
            {
                ViewBag.EmailModel = emailModel;
            }

            if (!string.IsNullOrEmpty(cnPedido))
            {
                ViewBag.CnPedido = cnPedido;
            }

            string cCAnalis;
            string cCMoneda;
            string cCVta;
            string fechaEmision;
            string cCTienda;
            string cCIgv_bo;
            string cCZonaLiberada;
            string fechaEntrega;
            var esProforma = false;
            if (!string.IsNullOrEmpty(cnProforma))
            {
                var cotizacionEntity = CotizacionService.GetById(cnProforma);
                var detalleCotizacion = CotizacionService.DatosDetalleProformaParaPedido(cnProforma);
                var cotizacionAdicional = CotizacionService.GetAdicionalById(cnProforma);
                var estadoAnulado = ((int)EstadoPedidoTypes.Anulado).ToString();
                var estadoRechazado = ((int)EstadoPedidoTypes.Rechazado).ToString();
                var estadoConfirmado = ((int)EstadoPedidoTypes.Confirmado_Total).ToString();
                var estadoProcesado = ((int)EstadoPedidoTypes.Procesado).ToString();
                var modelCotizacion = new PedidoEditViewModel
                {
                    PedidoDetailViewModel = new PedidoDetailViewModel(),
                    ClienteNewViewModel = new ClienteNewViewModel(),
                    EnviarMailViewModel = new EnviarMailViewModel(),
                    LugarEntregaNewViewModel = new LugarEntregaNewViewModel(),
                    ContactoEntregaDirectaNewViewModel = new ContactoEntregaDirectaNewViewModel(),
                    TransportistaNewViewModel = new TransportistaNewViewModel(),
                    ContactoTransporteNewModel = new ContactoTransporteNewModel()
                };

                if (cotizacionEntity != null && cotizacionEntity.cb_estado == estadoAnulado)
                {
                    ViewBag.CotizacionError = "La cotización fue anulado.";
                    SetDropDownList(null);
                    return View(modelCotizacion);
                }
                if (cotizacionEntity != null && cotizacionEntity.cb_estado == estadoRechazado)
                {
                    ViewBag.CotizacionError = "La cotización fue Rechazado.";
                    SetDropDownList(null);
                    return View(modelCotizacion);
                }
                if (cotizacionEntity != null && cotizacionEntity.cb_estado == estadoConfirmado)
                {
                    ViewBag.CotizacionError = "La cotización fue confirmado.";
                    SetDropDownList(null);
                    return View(modelCotizacion);
                }
                if (cotizacionEntity != null && cotizacionEntity.cb_estado == estadoProcesado)
                {
                    ViewBag.CotizacionError = "La cotización fue procesado.";
                    SetDropDownList(null);
                    return View();
                }
                //Dictionary<string, string> respuesta = new Dictionary<string, string>();
                //respuesta = ArticuloService.ValidaProductosCotizacion(cnProforma);
                //string idRespuesta = respuesta["id"];
                //string msjRespuesta = respuesta["mensaje"];
                //if (idRespuesta == "0")
                //{
                //    return JsonError(msjRespuesta);
                //}

                if (cotizacionEntity != null)
                {
                    esProforma = true;
                    cCAnalis = cotizacionEntity.cc_analis;
                    cCMoneda = cotizacionEntity.cc_moneda;
                    cCVta = cotizacionEntity.cc_vta;
                    //fechaEmision = cotizacionEntity.df_emision.ToShortDateString();
                    fechaEmision = cotizacionEntity.df_emision.ToString("dd/MM/yyyy");
                    cCTienda = cotizacionAdicional.cc_tienda;
                    cCIgv_bo = cotizacionAdicional.igv_bo.ToString();
                    cCZonaLiberada = cotizacionAdicional.zonaLiberada.ToString();

                    //var detailDetail = cotizacionEntity.LDPROF_WEB.Select(item => new PedidoDetailViewModel
                    var detailDetail = detalleCotizacion.Select(item => new PedidoDetailViewModel
                    {
                        cc_artic = item.cc_artic,
                        cd_artic = item.cd_artic,
                        cc_unmed = item.cc_unmed,
                        cc_lista = item.cc_lista,
                        fq_cantidad = item.fq_cantidad,
                        fq_peso_teorico = (decimal)item.fq_peso_teorico,
                        fm_precio = item.fm_precio,
                        fm_precio2 = item.fm_precio2,
                        fm_precio_fin = item.fm_precio_fin,
                        fm_total = item.fm_total,
                        EsProforma = esProforma,
                        igv_bo = cotizacionAdicional.igv_bo,
                        zonaLiberada_bo = cotizacionAdicional.zonaLiberada
                    }).ToList();

                    Session[SessionDetalleNuevo] = detailDetail;
                }
                else
                {
                    cCAnalis = (string)Session[SessionCcAnalisNuevo];
                    cCMoneda = (string)Session[SessionCcMonedaNuevo];
                    cCVta = (string)Session[SessionCcVtaNuevo];
                    fechaEmision = (string)Session[SessionCfechaEmisionNuevo];
                    cCTienda = (string)Session[SessionTiendaNuevo];
                    cCIgv_bo = (string)Session[SessionIgv_boNuevo];
                    cCZonaLiberada = (string)Session[SessionZonaLiberadaNuevo];
                }
                fechaEntrega = fechaEmision;
            }
            else
            {
                cCAnalis = (string)Session[SessionCcAnalisNuevo];
                cCMoneda = (string)Session[SessionCcMonedaNuevo];
                cCVta = (string)Session[SessionCcVtaNuevo];
                fechaEmision = (string)Session[SessionCfechaEmisionNuevo];
                fechaEntrega = (string)Session[SessionCfechaEntregaNuevo];
                cCTienda = (string)Session[SessionTiendaNuevo];
                cCIgv_bo = (string)Session[SessionIgv_boNuevo];
                cCZonaLiberada = (string)Session[SessionZonaLiberadaNuevo];
            }
            var cNSuc = (string)Session[SessionCnSucNuevo];
            var cNlug = (string)Session[SessionCn_lugNuevo];
            var cNIdContactoEntregaDirecta = (string)Session[SessionIdContactoEntregaDirectaNuevo];
            var cNtransp = (string)Session[SessionCC_transpNuevo];
            var cNContactoTransporte = (string)Session[SessionContactoTransporteNuevo];
            var cNobservacion = (string)Session[SessionVt_observacionNuevo];
            var cNocompra = (string)Session[SessionCn_ocompraNuevo];
            var cbRecojo = (string)Session[SessionCbRecojoNuevo];
            //var CNTienda = (string)Session[SessionTiendaNuevo];


            SetDropDownList(cCAnalis, cNtransp);

            var vm = new PedidoEditViewModel
            {
                cn_proforma = cnProforma,
                cc_analis = cCAnalis,
                cc_moneda = cCMoneda,
                cc_vta = cCVta,
                cn_suc = cNSuc,
                Cn_lug = cNlug,
                IdContactoEntregaDirecta = cNIdContactoEntregaDirecta,
                CC_transp = cNtransp,
                ContactoTransporte = cNContactoTransporte,
                Vt_observacion = cNobservacion,
                cn_ocompra = cNocompra,
                cb_recojo = cbRecojo,
                Tienda = cCTienda,
                igv_bo = Convert.ToInt32(cCIgv_bo),
                zonaLiberada_bo = Convert.ToInt32(cCZonaLiberada),
                EsProforma = esProforma,
                PedidoDetailViewModel = new PedidoDetailViewModel(),
                ClienteNewViewModel = new ClienteNewViewModel(),
                EnviarMailViewModel = new EnviarMailViewModel(),
                LugarEntregaNewViewModel = new LugarEntregaNewViewModel(),
                ContactoEntregaDirectaNewViewModel = new ContactoEntregaDirectaNewViewModel(),
                TransportistaNewViewModel = new TransportistaNewViewModel(),
                ContactoTransporteNewModel = new ContactoTransporteNewModel()
            };
            if (!string.IsNullOrEmpty(fechaEmision) && !string.IsNullOrWhiteSpace(fechaEmision))
            {
                vm.FechaEmision = fechaEmision;
            }
            var dFechaEmision = (vm.FechaEmision).ConvertDateTime();
            vm.n_i_paralelo_venta = TipoCambioDiarioService.GetByFechaTipoCambio(dFechaEmision);
            if (!string.IsNullOrEmpty(fechaEntrega) && !string.IsNullOrWhiteSpace(fechaEntrega))
            {
                vm.FechaEntrega = fechaEntrega;
            }
            ViewBag.GetGrupo = GetGrupo();
            ViewBag.GetSubGrupo = GetSubGrupo();
            return View(vm);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Nuevo(FormCollection formCollection)
        {
            ViewBag.BaseUrl = Url.Content("~/");
            _sessionTypes = SessionTypes.Nuevo;

            SetValueModel(formCollection);

            var model = new PedidoEditViewModel
            {
                cn_pedido = _cnPedido,
                cn_proforma = _cnProforma,
                cc_tipana = _ccTipana,
                cc_analis = _ccAnalis,
                cd_razsoc = _cdRazsoc,
                cc_moneda = _ccMoneda,
                FechaEmision = _cFechaEmision,
                FechaEntrega = _cFechaEntrega,
                fm_tipcam = _fmTipCam,
                cc_vta = _ccVta,
                cn_suc = _cnSuc,
                Cn_lug = _Cn_lug,
                CC_transp = _CC_transp,
                Vt_observacion = _Vt_observacion,
                cn_ocompra = _cn_ocompra,
                cb_recojo = _cbRecojo,
                Tienda = _Tienda,
                igv_bo = _Igv_bo,
                ContactoTransporte = _ContactoTransporte,
                IdContactoEntregaDirecta = _IdContactoEntregaDirecta,
                zonaLiberada_bo = _zonaLiberada_bo,
                ClienteNewViewModel = new ClienteNewViewModel()
            };

            var emailModel = new EnviarMailViewModel
            {
                Asunto = formCollection["sAsunto"],
                Para = formCollection["sPara"],
                ConCopia = formCollection["sConCopia"],
                Mensaje = formCollection["sMensaje"]
            };

            try
            {
                ModelState.Clear();
                TryUpdateModel(model);
                if (!ModelState.IsValid)
                {
                    SetDropDownList(model.cc_analis, model.CC_transp);
                    ViewBag.GetGrupo = GetGrupo();
                    ViewBag.GetSubGrupo = GetSubGrupo();
                    model.EnviarMailViewModel = new EnviarMailViewModel();
                    model.LugarEntregaNewViewModel = new LugarEntregaNewViewModel();
                    model.ContactoEntregaDirectaNewViewModel = new ContactoEntregaDirectaNewViewModel();
                    model.TransportistaNewViewModel = new TransportistaNewViewModel();
                    model.ContactoTransporteNewModel = new ContactoTransporteNewModel();
                    return View(model);
                }
                ModeloValidoExtendido(model);
                var tempDetalle = Session[SessionDetalleNuevo];
                if (tempDetalle == null || ((IList<PedidoDetailViewModel>)(tempDetalle)).Count == 0)
                {
                    SetDropDownList(model.cc_analis, model.CC_transp);
                    ViewBag.GetGrupo = GetGrupo();
                    ViewBag.GetSubGrupo = GetSubGrupo();
                    model.EnviarMailViewModel = new EnviarMailViewModel();
                    model.LugarEntregaNewViewModel = new LugarEntregaNewViewModel();
                    model.ContactoEntregaDirectaNewViewModel = new ContactoEntregaDirectaNewViewModel();
                    model.TransportistaNewViewModel = new TransportistaNewViewModel();
                    model.ContactoTransporteNewModel = new ContactoTransporteNewModel();
                    const string messageValidation = "No existen artículos.";
                    ModelState.AddModelError(string.Empty, messageValidation);
                    ViewBag.MessageValidation = messageValidation;
                    return View(model);
                }

                var entityMaster = new LCPEDIDO_WEB
                {
                    cn_pedido = model.cn_pedido,
                    cn_proforma = model.cn_proforma,
                    cc_tipana = model.cc_tipana,
                    cc_analis = model.cc_analis,
                    cd_razsoc = model.cd_razsoc,
                    cc_moneda = model.cc_moneda,
                    cc_vta = model.cc_vta,
                    df_emision = model.FechaEmision.ConvertDateTime(),
                    fm_tipcam = model.fm_tipcam,
                    cn_suc = model.cn_suc,
                    cb_recojo = model.cb_recojo
                    //cc_tienda = model.Tienda
                };

                var detailPedidoCast = (IList<PedidoDetailViewModel>)(tempDetalle);
                var item = 0;
                var pedidoDetalleList = detailPedidoCast.Select(detail => new LDPEDIDO_WEB
                {
                    cn_item = (++item).ToString().PadLeft(2, '0'),
                    cc_artic = detail.cc_artic,
                    fq_cantidad = detail.fq_cantidad,
                    fq_stock = 0,
                    cc_lista = detail.cc_lista,
                    fm_precio = detail.fm_precio,
                    fm_precio2 = detail.fm_precio2,
                    fm_precio_fin = detail.fm_precio_fin,
                    fm_total = detail.fm_total
                }).ToList();

                PedidoService.PedidoDetalleServices = pedidoDetalleList;
                PedidoService.Guardar(entityMaster, model.igv_bo, EmpresaSegunBD(), model.zonaLiberada_bo);
                switch (model.cb_recojo)
                {
                    case "1":
                        model.Cn_lug = "";
                        model.CC_transp = "";
                        model.ContactoTransporte = "";
                        model.IdContactoEntregaDirecta = "";
                        break;
                    case "2":
                        model.CC_transp = "";
                        model.ContactoTransporte = "";
                        break;
                    case "3":
                        model.Cn_lug = "";
                        model.IdContactoEntregaDirecta = "";
                        break;
                }

                PedidoService.GuardarAdicional(entityMaster, User.Identity.Name, model.Cn_lug + "", model.CC_transp + "", model.Vt_observacion + "", model.ContactoTransporte + "", model.IdContactoEntregaDirecta + "", model.Tienda + "", model.FechaEntrega.ConvertDateTime(), model.igv_bo, model.cn_ocompra, model.zonaLiberada_bo);

                //Inicio: Registra la Nota de Pedido Tortuga
                LCPEDIDOADICIONAL_WEB PedidoAdicional = new LCPEDIDOADICIONAL_WEB();
                PedidoAdicional.Cn_lug = model.Cn_lug;
                PedidoAdicional.cn_ocompra = model.cn_ocompra;
                PedidoAdicional.Vt_observacion = model.Vt_observacion;
                PedidoAdicional.cc_tienda = model.Tienda;
                PedidoAdicional.CC_transp = model.CC_transp;
                PedidoAdicional.FechaEntrega = DateTime.Parse(model.FechaEntrega);
                PedidoAdicional.ContactoTransporte = model.ContactoTransporte;


                if (emailModel.Para.Length > 0) //Guardar y Enviar
                {
                    var xml = new XDocument(new XElement("DetallePedido",
                                     from pedidoDetalle in pedidoDetalleList
                                     select new XElement("Articulo",
                                       new XElement("cn_item", pedidoDetalle.cn_item),
                                       new XElement("cc_artic", pedidoDetalle.cc_artic),
                                       new XElement("cc_unmed", pedidoDetalle.MARTICUL.cc_unmed),
                                       new XElement("fq_cantped", pedidoDetalle.fq_cantidad),
                                       new XElement("fm_valunit", pedidoDetalle.fm_precio2),
                                       new XElement("fm_monvta", pedidoDetalle.fm_total),
                                       new XElement("cd_artic", pedidoDetalle.MARTICUL.cd_artic),
                                       new XElement("fq_embalaje", pedidoDetalle.MARTICUL.fq_embalaje),
                                       new XElement("fm_valuni_cd", pedidoDetalle.fm_precio_fin)
                                       )
                                ));

                    PedidoService.RegistrarNotaPedidoVenta(entityMaster, PedidoAdicional, xml.ToString(), User.Identity.Name);
                }
                //Fin: Registra la Nota de Pedido Tortuga

                if (!string.IsNullOrEmpty(model.cn_proforma) && !string.IsNullOrWhiteSpace(model.cn_proforma))
                {
                    var existeCotizacion = CotizacionService.GetById(model.cn_proforma);
                    if (existeCotizacion != null && existeCotizacion.cb_estado == ((int)EstadoPedidoTypes.Enviado).ToString())
                    {
                        var estadoConfirmado = ((int)EstadoPedidoTypes.Confirmado_Total).ToString();
                        CotizacionService.UpdateEstado(model.cn_proforma, estadoConfirmado);
                    }
                }
                SetSessionNull();
                TempData["Guardado"] = true;
                TempData["EmailModel"] = emailModel;
                emailModel.Asunto = Funciones.Replace(emailModel.Asunto, "[Nro]", entityMaster.cn_pedido);
                TempData["CnPedido"] = entityMaster.cn_pedido;
                RouteData.Values["id"] = null;

                return RedirectToAction<PedidoController>(x => x.Nuevo(0));
            }
            catch (DbEntityValidationException e)
            {
                var sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("Entidad de Tipo \"{0}\" en estado \"{1}\" tiene los siguientes errores de validación:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    var errores = eve.ValidationErrors;
                    foreach (var item in errores)
                    {
                        sb.AppendLine(string.Format("- Propiedad: \"{0}\", Error: \"{1}\"", item.PropertyName, item.ErrorMessage));
                    }
                }
                var messageValidation = sb.ToString();
                ModelState.AddModelError(string.Empty, messageValidation);
                ViewBag.MessageValidation = messageValidation;
                SetDropDownList(model.cc_analis, model.CC_transp);
                ViewBag.GetGrupo = GetGrupo();
                ViewBag.GetSubGrupo = GetSubGrupo();
                model.EnviarMailViewModel = new EnviarMailViewModel();
                model.LugarEntregaNewViewModel = new LugarEntregaNewViewModel();
                model.ContactoEntregaDirectaNewViewModel = new ContactoEntregaDirectaNewViewModel();
                model.TransportistaNewViewModel = new TransportistaNewViewModel();
                model.ContactoTransporteNewModel = new ContactoTransporteNewModel();
                return View(model);
            }
            catch (Exception ex)
            {
                var messageValidation = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ModelState.AddModelError(string.Empty, messageValidation);
                ViewBag.MessageValidation = messageValidation;
                SetDropDownList(model.cc_analis, model.CC_transp);
                ViewBag.GetGrupo = GetGrupo();
                ViewBag.GetSubGrupo = GetSubGrupo();
                model.EnviarMailViewModel = new EnviarMailViewModel();
                model.LugarEntregaNewViewModel = new LugarEntregaNewViewModel();
                model.ContactoEntregaDirectaNewViewModel = new ContactoEntregaDirectaNewViewModel();
                model.TransportistaNewViewModel = new TransportistaNewViewModel();
                model.ContactoTransporteNewModel = new ContactoTransporteNewModel();
                return View(model);
            }
        }

        private bool ModeloValidoExtendido(PedidoEditViewModel model)
        {
            bool bRetorno = true;
            switch (model.cb_recojo)
            {
                //CONTACTO DE ENTREGA DIRECTA
                //case "2"://Entrega a Cliente
                //    if (string.IsNullOrWhiteSpace(model.Cn_lug))
                //    {
                //        throw new Exception("Debe seleccionar un Lugar de Entrega Directa");
                //    }else if (string.IsNullOrWhiteSpace(model.IdContactoEntregaDirecta))
                //    {
                //        throw new Exception("Debe seleccionar un Contacto de Entrega Directa");
                //    }
                //    break;
                case "3"://Entrega por Transporte
                    if (string.IsNullOrWhiteSpace(model.CC_transp))
                    {
                        throw new Exception("Debe seleccionar una Entrega de Transporte");
                    }
                    else if (string.IsNullOrWhiteSpace(model.ContactoTransporte))
                    {
                        throw new Exception("Debe seleccionar un Contacto de Transporte");
                    }
                    break;
            }
            return bRetorno;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Editar(string id, int? page)
        {
            ViewBag.BaseUrl = Url.Content("~/");
            _sessionTypes = SessionTypes.Editar;

            //var saved = (bool?)TempData["Guardado"];

            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction<PedidoController>(x => x.Listado(page));
            }

            var pedido = PedidoService.GetById(id);
            var pedidoAdicional = PedidoService.GetAdicionalById(id);
            if (pedido == null)
            {
                return RedirectToAction<PedidoController>(x => x.Listado(page));
            }
            //pedidoAdicional.CC_transp = (pedidoAdicional.CC_transp + "       ").Substring(0, 5);
            if (pedidoAdicional.ContactoTransporte != null)
            {
                pedidoAdicional.ContactoTransporte = pedidoAdicional.ContactoTransporte.Trim();
            }
            //if (saved.HasValue && saved.Value)
            //{
            //    ViewBag.Success = "Pedido actualizado.";
            //}

            var vm = new PedidoEditViewModel
            {
                cn_pedido = pedido.cn_pedido,
                cn_proforma = pedido.cn_proforma,
                cc_analis = pedido.cc_analis,
                cc_moneda = pedido.cc_moneda,
                cc_vta = pedido.cc_vta,
                //FechaEmision = pedido.df_emision.ToShortDateString(),
                FechaEmision = pedido.df_emision.ToString("dd/MM/yyyy"),
                //FechaEntrega = pedidoAdicional.FechaEntrega.ToShortDateString(),
                FechaEntrega = pedidoAdicional.FechaEntrega.ToString("dd/MM/yyyy"),
                fm_tipcam = pedido.fm_tipcam,
                n_i_paralelo_venta = Convert.ToDouble(pedido.fm_tipcam),
                PedidoDetailViewModel = new PedidoDetailViewModel(),
                cb_estado = pedido.cb_estado,
                EnviarMailViewModel = new EnviarMailViewModel(),
                LugarEntregaNewViewModel = new LugarEntregaNewViewModel(),
                ContactoEntregaDirectaNewViewModel = new ContactoEntregaDirectaNewViewModel(),
                TransportistaNewViewModel = new TransportistaNewViewModel(),
                ContactoTransporteNewModel = new ContactoTransporteNewModel(),
                EsProforma = !string.IsNullOrEmpty(pedido.cn_proforma),
                cn_suc = pedido.cn_suc,
                Cn_lug = pedidoAdicional.Cn_lug,
                IdContactoEntregaDirecta = pedidoAdicional.IdContactoEntregaDirecta,
                Tienda = pedidoAdicional.cc_tienda,
                igv_bo = pedidoAdicional.igv_bo,
                zonaLiberada_bo = pedidoAdicional.zonaLiberada,
                CC_transp = pedidoAdicional.CC_transp,
                ContactoTransporte = pedidoAdicional.ContactoTransporte,
                Vt_observacion = pedidoAdicional.Vt_observacion,
                cn_ocompra = pedidoAdicional.cn_ocompra,
                cb_recojo = pedido.cb_recojo
            };

            var detalle = pedido.LDPEDIDO_WEB;
            var detailCotizacion = (from item in detalle
                                    let articulo = ArticuloService.GetByCodigo(item.cc_artic)
                                    select new PedidoDetailViewModel
                                    {
                                        cn_pedido = pedido.cn_pedido,
                                        cn_item = item.cn_item,
                                        cc_artic = item.cc_artic,
                                        cd_artic = articulo.cd_artic,
                                        cc_unmed = articulo.cc_unmed,
                                        fq_cantidad = item.fq_cantidad,
                                        fq_peso_teorico = (decimal)item.MARTICUL.fq_peso_teorico,
                                        fq_stock = item.fq_stock ?? 0,
                                        cc_lista = item.cc_lista,
                                        fm_precio = item.fm_precio,
                                        fm_precio2 = item.fm_precio2,
                                        fm_precio_fin = item.fm_precio_fin,
                                        fm_total = item.fm_total,
                                        igv_bo = pedidoAdicional.igv_bo,
                                        zonaLiberada_bo = pedidoAdicional.zonaLiberada,
                                        EsProforma = !string.IsNullOrEmpty(pedido.cn_proforma)
                                    }).ToList();

            Session[SessionDetalleEditar] = detailCotizacion;

            SetDropDownList(vm.cc_analis, vm.CC_transp);
            ViewBag.GetGrupo = GetGrupo();
            ViewBag.GetSubGrupo = GetSubGrupo();
            return View(vm);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(FormCollection formCollection, int? page)
        {
            Dictionary<string, object> form = new Dictionary<string, object>();
            formCollection.CopyTo(form);



            ViewBag.BaseUrl = Url.Content("~/");
            _sessionTypes = SessionTypes.Editar;

            SetValueModel(formCollection);

            var model = new PedidoEditViewModel
            {
                cn_pedido = _cnPedido,
                cn_proforma = _cnProforma,
                cc_tipana = _ccTipana,
                cc_analis = _ccAnalis,
                cd_razsoc = _cdRazsoc,
                cc_moneda = _ccMoneda,
                FechaEmision = _cFechaEmision,
                FechaEntrega = _cFechaEntrega,
                fm_tipcam = _fmTipCam,
                cc_vta = _ccVta,
                EnviarMailViewModel = new EnviarMailViewModel(),
                LugarEntregaNewViewModel = new LugarEntregaNewViewModel(),
                ContactoEntregaDirectaNewViewModel = new ContactoEntregaDirectaNewViewModel(),
                TransportistaNewViewModel = new TransportistaNewViewModel(),
                ContactoTransporteNewModel = new ContactoTransporteNewModel(),
                cn_suc = _cnSuc,
                Cn_lug = _Cn_lug,
                IdContactoEntregaDirecta = _IdContactoEntregaDirecta,
                CC_transp = _CC_transp,
                Vt_observacion = _Vt_observacion,
                cn_ocompra = _cn_ocompra,
                cb_recojo = _cbRecojo,
                Tienda = _Tienda,
                igv_bo = _Igv_bo,
                zonaLiberada_bo = _zonaLiberada_bo,
                ContactoTransporte = _ContactoTransporte
            };

            try
            {
                var estadoPedido = PedidoService.GetById(model.cn_pedido);
                var estadoProcesado = ((int)EstadoPedidoTypes.Procesado).ToString();
                var estadoEnviado = ((int)EstadoPedidoTypes.Enviado).ToString();
                var estadoAnulado = ((int)EstadoPedidoTypes.Anulado).ToString();
                if (estadoPedido != null && estadoPedido.cb_estado == estadoProcesado)
                {
                    ViewBag.Error = "El Pedido fue procesado.";
                    SetDropDownList(model.cc_analis, model.CC_transp);
                    ViewBag.GetGrupo = GetGrupo();
                    ViewBag.GetSubGrupo = GetSubGrupo();
                    return View(model);

                }
                //if (estadoPedido != null && estadoPedido.cb_estado == estadoEnviado)
                //{
                //    ViewBag.Warning = "La Pedido fue enviado.";
                //    SetDropDownList(model.cc_analis);
                //    ViewBag.GetGrupo = GetGrupo();
                //    ViewBag.GetSubGrupo = GetSubGrupo();
                //    return View(model);
                //}
                if (estadoPedido != null && estadoPedido.cb_estado == estadoAnulado)
                {
                    ViewBag.Warning = "La Pedido fue anulado.";
                    SetDropDownList(model.cc_analis, model.CC_transp);
                    ViewBag.GetGrupo = GetGrupo();
                    ViewBag.GetSubGrupo = GetSubGrupo();
                    return View(model);
                }

                ModelState.Clear();
                TryUpdateModel(model);
                if (!ModelState.IsValid)
                {
                    SetDropDownList(model.cc_analis, model.CC_transp);
                    ViewBag.GetGrupo = GetGrupo();
                    ViewBag.GetSubGrupo = GetSubGrupo();
                    return View(model);
                }

                ModeloValidoExtendido(model);
                var tempDetalle = Session[SessionDetalleEditar];
                var tempDetalleCast = ((tempDetalle) as IList<PedidoDetailViewModel>);
                if (tempDetalleCast == null || tempDetalleCast.Count == 0)
                {
                    SetDropDownList(model.cc_analis, model.CC_transp);
                    ViewBag.GetGrupo = GetGrupo();
                    ViewBag.GetSubGrupo = GetSubGrupo();
                    const string messageValidation = "No existen artículos.";
                    ModelState.AddModelError(string.Empty, messageValidation);
                    ViewBag.MessageValidation = messageValidation;
                    return View(model);
                }

                var entityMaster = new LCPEDIDO_WEB
                {
                    cn_pedido = model.cn_pedido,
                    cn_proforma = model.cn_proforma,
                    cc_tipana = model.cc_tipana,
                    cc_analis = model.cc_analis,
                    cd_razsoc = model.cd_razsoc,
                    cc_moneda = model.cc_moneda,
                    cc_vta = model.cc_vta,
                    df_emision = model.FechaEmision.ConvertDateTime(),
                    fm_tipcam = model.fm_tipcam,
                    cn_suc = model.cn_suc,
                    cb_recojo = model.cb_recojo,
                    cb_estado = "1"
                };

                var detailPedidoCast = (IList<PedidoDetailViewModel>)(tempDetalle);
                var item = 0;
                var pedidoDetalleList = detailPedidoCast.Select(detail => new LDPEDIDO_WEB
                {
                    cn_item = (++item).ToString().PadLeft(2, '0'),
                    cc_artic = detail.cc_artic,
                    fq_cantidad = detail.fq_cantidad,
                    fq_stock = 0,
                    cc_lista = detail.cc_lista,
                    fm_precio = detail.fm_precio,
                    fm_precio2 = detail.fm_precio2,
                    fm_precio_fin = detail.fm_precio_fin,
                    fm_total = detail.fm_total
                }).ToList();

                PedidoService.PedidoDetalleServices = pedidoDetalleList;
                PedidoService.Guardar(entityMaster, model.igv_bo, EmpresaSegunBD(), model.zonaLiberada_bo);
                switch (model.cb_recojo)
                {
                    case "1":
                        model.Cn_lug = "";
                        model.IdContactoEntregaDirecta = "";
                        model.CC_transp = "";
                        model.ContactoTransporte = "";
                        break;
                    case "2":
                        model.CC_transp = "";
                        model.ContactoTransporte = "";
                        break;
                    case "3":
                        model.Cn_lug = "";
                        model.IdContactoEntregaDirecta = "";
                        break;
                }
                PedidoService.GuardarAdicional(entityMaster, User.Identity.Name, model.Cn_lug + "", model.CC_transp + "", model.Vt_observacion + "", model.ContactoTransporte + "", model.IdContactoEntregaDirecta + "", model.Tienda + "", model.FechaEntrega.ConvertDateTime(), model.igv_bo, model.cn_ocompra, model.zonaLiberada_bo);



                SetSessionNull();
                TempData["Guardado"] = true;

                return RedirectToAction<PedidoController>(x => x.Listado(page));
                ///return RedirectToAction<PedidoController>(x => x.Editar(model.cn_pedido, page));
            }
            catch (DbEntityValidationException e)
            {
                var sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("Entidad de Tipo \"{0}\" en estado \"{1}\" tiene los siguientes errores de validación:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    var errores = eve.ValidationErrors;
                    foreach (var item in errores)
                    {
                        sb.AppendLine(string.Format("- Propiedad: \"{0}\", Error: \"{1}\"", item.PropertyName, item.ErrorMessage));
                    }
                }
                var messageValidation = sb.ToString();
                ModelState.AddModelError(string.Empty, messageValidation);
                ViewBag.MessageValidation = messageValidation;
                SetDropDownList(model.cc_analis, model.CC_transp);
                ViewBag.GetGrupo = GetGrupo();
                ViewBag.GetSubGrupo = GetSubGrupo();
                return View(model);
            }
            catch (Exception ex)
            {
                var messageValidation = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ModelState.AddModelError(string.Empty, messageValidation);
                ViewBag.MessageValidation = messageValidation.Replace("\r\n", "");
                SetDropDownList(model.cc_analis, model.CC_transp);
                ViewBag.GetGrupo = GetGrupo();
                ViewBag.GetSubGrupo = GetSubGrupo();
                return View(model);
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult Anulado(string id, int? page)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            {
                return Request.IsAjaxRequest() ? JsonError("parámetro id obligatorio.") : RedirectToAction<PedidoController>(x => x.Listado(page));
            }

            try
            {
                var pedido = PedidoService.GetById(id);

                if (pedido == null)
                {
                    return Request.IsAjaxRequest() ? JsonError("No existe pedido.") : RedirectToAction<PedidoController>(x => x.Listado(page));
                }
                if (!string.IsNullOrEmpty(pedido.cn_proforma))
                {
                    var cotizacion = CotizacionService.GetById(pedido.cn_proforma);
                    if (cotizacion != null)
                    {
                        var estadoEmitido = ((int)EstadoPedidoTypes.Emitido).ToString();
                        CotizacionService.UpdateEstado(pedido.cn_proforma, estadoEmitido);
                    }
                }


                var estadoAnulado = ((int)EstadoPedidoTypes.Anulado).ToString();
                PedidoService.UpdateEstado(id, estadoAnulado);

                if (!Request.IsAjaxRequest()) return View(id);
                const string success = "Cambio de estado a anulado.";
                return JsonSuccess(new { estado = success, id });
            }
            catch (DbEntityValidationException e)
            {
                var sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("Entidad de Tipo \"{0}\" en estado \"{1}\" tiene los siguientes errores de validación:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    var errores = eve.ValidationErrors;
                    foreach (var item in errores)
                    {
                        sb.AppendLine(string.Format("- Propiedad: \"{0}\", Error: \"{1}\"", item.PropertyName, item.ErrorMessage));
                    }
                }
                return JsonError(sb.ToString());
            }
            catch (Exception ex)
            {
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                if (Request.IsAjaxRequest())
                {
                    return JsonError(message);
                }
                return View(message);
            }
        }

        private static readonly string Remite = ConfigurationManager.AppSettings["mail_remitente"];
        private static readonly string Label = ConfigurationManager.AppSettings["mail_remitente_label"];
        private const string PathFormatPdf = "~/Content/pdf/pedido-{0}.pdf";

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetEmailCliente(string id, string tipo, string Nro)
        {
            int tipoMail = 2;
            //1 - Cotizacion
            //2 - Pedido u otros

            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            {
                return JsonError("No existe identificador.");
            }
            try
            {
                var cliente = ClienteService.GetEmailByCodigo(tipoMail, id, "", "");
                var email = new
                {
                    asunto = "Pedido " + (string.IsNullOrEmpty(Nro) ? "[Nro]" : Nro) + " - " + cliente.cd_razsoc,
                    para = ConfigurationManager.AppSettings["mail_BO"],//cliente.ct_email,
                    conCopia = User.Identity.Name
                };
                //return cliente == null ? JsonError("Cliente seleccionado no existe.") : JsonSuccess(email);
                return JsonSuccess(email);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult EnviarMail(EnviarMailViewModel model, string id, string idCliente)
        {
            int tipoMail = 2;
            //1 - Cotizacion
            //2 - Pedido

            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }


            var sb = new StringBuilder();
            //sb.AppendLine(model.Mensaje);
            try
            {

                //ClienteService.UpdateEmailByCodigo(idCliente, model.Para);

                //Actualizamos el ct_email de TCONTACLIE
                //ClienteService.ActualizarMailContacto(tipoMail, id, model.Para);

                RenderPdf(id, "RenderPdfPedido");
                var pdfPath = Server.MapPath(string.Format(PathFormatPdf, id));

                var pedido = PedidoService.GetById(id);
                if (pedido != null)
                {
                    var estadoEnviado = ((int)EstadoPedidoTypes.Enviado).ToString();
                    PedidoService.UpdateEstado(id, estadoEnviado);
                }
                string sRemitente = User.Identity.Name;
                var vendedor = VendedorService.GetByEmail(sRemitente);

                model.Mensaje = "[" + vendedor.ct_nombreCompleto + "]: " + model.Mensaje;
                sb.AppendLine(model.Mensaje);

                model.Asunto = Funciones.Replace(model.Asunto, "[Nro]", id);
                Mail.SendMail(sRemitente, vendedor.ct_nombreCompleto, model.Asunto, sb, model.Para, null, pdfPath, esHtml: true);//Cliente
                //Mail.SendMail(sRemitente, "Vendedor: " + vendedor.ct_nombreCompleto, model.Asunto, sb, Remite, null, pdfPath, esHtml: true);//BackOffice
                if (!string.IsNullOrEmpty(model.ConCopia))
                {
                    Mail.SendMail(sRemitente, Label, model.Asunto, sb, model.ConCopia, null, pdfPath, esHtml: true);//Vendedor
                }
            }
            catch (Exception ex)
            {
                var exception = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return JsonError(exception);
            }

            return JsonSuccess(1);
        }

        public ActionResult RenderPdf(string id, string viewName)
        {
            var pedido = PedidoService.GetById(id);
            var pedidoAdicional = PedidoService.GetAdicionalById(id);
            var cliente = ClienteService.GetByCodigo(pedido.cc_analis);
            var pedidoClienteVm = new PedidoClienteViewModel
            {
                Pedido = pedido,
                Cliente = cliente,
                Adicional = pedidoAdicional
            };

            var pdfOutput = ControllerContext.GeneratePdf(pedidoClienteVm, viewName);
            var fullPath = Server.MapPath(string.Format(PathFormatPdf, pedido.cn_pedido));

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            System.IO.File.WriteAllBytes(fullPath, pdfOutput);
            return View(viewName);
        }
        public ActionResult Descargar(string cn_pedido)
        {
            RenderPdf(cn_pedido, "RenderPdfPedido");
            var pdfPath = Server.MapPath(string.Format(PathFormatPdf, cn_pedido));

            byte[] filedata = System.IO.File.ReadAllBytes(pdfPath);
            string contentType = System.Net.Mime.MediaTypeNames.Application.Octet;

            string filename = "Pedido Nro." + cn_pedido + ".pdf";
            return File(filedata, contentType, filename);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult ClienteNew(ClienteNewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }

            var entity = new MCLIENTE
            {
                cc_tipana = "01",
                cc_analis = model.Ruc,
                cd_razsoc = model.RazonSocial.ToUpper(),
                ct_email = model.Email != null ? model.Email.ToLower() : null,
                c_fl_agente_percepcion = "0",
                c_fl_vinculacion = "0",
                cb_activo = "1",
                cb_embalaje = "0",
                cb_flete = "0",
                cb_moncred = "N",
                cb_monfac = "A",
                cb_proced = "N",
                cb_sucursal = "0",
                cb_limcred = "N",
                cd_direc = model.Domicilio,
                cn_telf1 = model.Telefono,
                cc_sector = model.cc_sector,
                cc_distrito = model.cc_distrito,
                cc_prov = model.cc_provincia,
                cc_dpto = model.cc_departamento
            };
            try
            {
                Dictionary<int, string> diccionario = new Dictionary<int, string>();
                diccionario = ClienteService.NuevoCliente(entity, User.Identity.Name);

                foreach (KeyValuePair<int, string> item in diccionario)
                {
                    if (item.Key == 1)
                    {
                        throw new Exception(item.Value);
                    }
                }
                //ClienteService.GuardarBasico(entity);
            }
            catch (Exception ex)
            {
                var exception = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return JsonError(exception);
            }
            return JsonSuccess(1);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult TransportistaNew(TransportistaNewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }

            dynamic entity = new ExpandoObject();

            entity.Ruc = model.Ruc;
            entity.RazonSocial = model.RazonSocial.ToUpper();
            entity.Domicilio = model.Domicilio;
            entity.Departamento = model.Departamento;
            entity.Provincia = model.Provincia;
            entity.Distrito = model.Distrito;
            entity.Cliente = model.Cliente;

            long codigo = 0;
            try
            {
                codigo = TransportistaRepository.add(entity);
            }
            catch (Exception ex)
            {
                var exception = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return JsonError(exception);
            }
            return JsonSuccess(new
            {
                codigo = codigo
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult LugarEntregaNew(LugarEntregaNewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }

            dynamic entity = new ExpandoObject();

            entity.Analisis = model.Analisis;
            entity.Direccion = model.Direccion;
            entity.Zona = model.Zona;
            entity.Departamento = model.Departamento;
            entity.Provincia = model.Provincia;
            entity.Distrito = model.Distrito;
            entity.Entrega = model.Entrega;
            entity.Cobranza = model.Cobranza;

            string codigo = "0";
            try
            {
                codigo = SucursalClienteRepository.add(entity);
            }
            catch (Exception ex)
            {
                var exception = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return JsonError(exception);
            }
            return JsonSuccess(new
            {
                codigo = codigo
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult ContactoEntregaDirectaNew(ContactoEntregaDirectaNewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }

            int ContVenta, ContCobranza, ContAlmacen;

            if (model.ContVenta)
            {
                ContVenta = 1;
            }
            else
            {
                ContVenta = 0;
            }

            if (model.ContCobranza)
            {
                ContCobranza = 1;
            }
            else
            {
                ContCobranza = 0;
            }

            if (model.ContAlmacen)
            {
                ContAlmacen = 1;
            }
            else
            {
                ContAlmacen = 0;
            }

            dynamic entity = new ExpandoObject();

            entity.Analisis = model.Analisis;
            entity.Surcursal = model.Surcursal;
            entity.Nombres = model.Nombres;
            entity.Telefono = model.Telefono;
            entity.Telefono2 = model.Telefono2;
            entity.Email = model.Email;
            entity.ContAlmacen = ContAlmacen;
            entity.ContCobranza = ContCobranza;
            entity.ContVenta = ContVenta;
            entity.CargoLaboral = model.CargoLaboral;
            entity.EnvioDocs = model.EnvioDocs;

            string codigo = "0";
            try
            {
                codigo = ClienteRepository.addContactoEntregaDirecta(entity);
            }
            catch (Exception ex)
            {
                var exception = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return JsonError(exception);
            }
            return JsonSuccess(new
            {
                codigo = codigo
            });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult setContactoTransporte(string transporte, string estado, string contacto)
        {
            dynamic entity = new ExpandoObject();
            entity.Transportista = transporte;
            entity.Contacto = contacto;
            entity.Activo = estado;

            ContactoTransportistaRepository.ActivaContactoTransportista(entity);
            return JsonSuccess(1);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult ContactoTransportistaNew(ContactoTransporteNewModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }

            dynamic entity = new ExpandoObject();

            entity.Nombres = model.Nombres;
            entity.Telefono1 = model.Telefono1;
            entity.Telefono2 = model.Telefono2;
            entity.Email = model.Email;
            entity.Transportista = model.Transportista;

            long codigo = 0;
            try
            {
                codigo = ContactoTransportistaRepository.add(entity);
            }
            catch (Exception ex)
            {
                var exception = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return JsonError(exception);
            }
            return JsonSuccess(new
            {
                codigo = codigo
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetClientesByParam(string param)
        {
            try
            {
                var lista = ClienteService
                .GetByRazonSocialOrRuc(param)
                .Select(x => new SelectListItem
                {
                    Text = x.cd_razsoc,
                    Value = x.cc_analis
                });
                lista = new SelectList(lista, "Value", "Text");
                return JsonSuccess(new { estado = 1, data = lista });
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetArticulosByParam(string param)
        {
            try
            {
                var lista = ArticuloService
                .GetByNombreOrCodigo(param)
                .Select(x => new SelectListItem
                {
                    Text = x.cd_artic,
                    Value = x.cc_artic
                });
                lista = new SelectList(lista, "Value", "Text");
                return JsonSuccess(new { estado = 1, data = lista });
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetArticulosByGrupoParam(string grupo, string subGrupo, string param, string cc_tienda)
        {
            try
            {
                if (subGrupo == "" || subGrupo == "Ninguno")
                {
                    subGrupo = "%";
                }
                if (grupo == "")
                {
                    grupo = "%";
                }
                var lista = ArticuloService
                .GetByNombreOrCodigoYGrupo(grupo, subGrupo, /*param,*/ cc_tienda)
                .Select(x => new SelectListItem
                {
                    Text = x.cd_artic,
                    Value = x.cc_artic
                });
                lista = new SelectList(lista, "Value", "Text");
                return JsonSuccess(new { estado = 1, data = lista });
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        //ValidarProductoPorStockYProduccion
        public ActionResult ValidaStockArticuloPedido(string cc_artic, string cc_tienda, decimal StockSolicitado, bool EsProforma, string cn_proforma, string cn_pedido)
        {
            try
            {
                Dictionary<string, string> respuesta = new Dictionary<string, string>();
                respuesta = ArticuloService.ValidaStockArticulo_LCPEDIDOWEB(cc_artic, cc_tienda, StockSolicitado, EsProforma, cn_proforma, cn_pedido);
                return JsonSuccess(respuesta);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult ValidaProductosCotizacion(string cnProforma)
        {
            try
            {
                Dictionary<string, string> respuesta = new Dictionary<string, string>();
                respuesta = ArticuloService.ValidaProductosCotizacion(cnProforma);
                return JsonSuccess(respuesta);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult ValidarStockPedido(string cnPedido, string listaArtic, string listaStockSolicitado, string ccTienda)
        {
            try
            {
                Dictionary<string, string> respuesta = new Dictionary<string, string>();
                respuesta = ArticuloService.ValidarStockPedido(cnPedido, listaArtic, listaStockSolicitado, ccTienda);
                return JsonSuccess(respuesta);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        #region SetValue
        private static string SetValue(string formValue, object sessionValue)
        {
            if (string.IsNullOrEmpty(formValue) || string.IsNullOrWhiteSpace(formValue))
            {
                return (string)sessionValue;
            }
            return formValue;
        }

        private static bool SetBoolean(string formValue, object sessionValue)
        {
            var valor = formValue;
            if (string.IsNullOrEmpty(valor) || string.IsNullOrWhiteSpace(valor))
            {
                valor = (string)sessionValue;
                if (string.IsNullOrEmpty(valor) || string.IsNullOrWhiteSpace(valor))
                {
                    return false;
                }
                bool valorBoolean;
                return !bool.TryParse(valor, out valorBoolean) ? valorBoolean : valorBoolean;
            }
            else
            {
                bool valorBoolean;
                return !bool.TryParse(valor, out valorBoolean) ? valorBoolean : valorBoolean;
            }
        }

        private static double SetDouble(string formValue, object sessionValue)
        {
            string sDouble;
            double nDouble;
            if (string.IsNullOrEmpty(formValue) || string.IsNullOrWhiteSpace(formValue))
            {
                sDouble = (string)sessionValue;
                double.TryParse(sDouble, out nDouble);
                return nDouble;
            }
            sDouble = formValue;
            double.TryParse(sDouble, out nDouble);
            return nDouble;
        }
        private static decimal SetDecimal(string formValue, object sessionValue)
        {
            string sDecimal;
            decimal nDecimal;
            if (string.IsNullOrEmpty(formValue) || string.IsNullOrWhiteSpace(formValue))
            {
                sDecimal = sessionValue as string;
                decimal.TryParse(sDecimal, out nDecimal);
                return nDecimal;
            }
            sDecimal = formValue;
            decimal.TryParse(sDecimal, NumberStyles.Currency, CultureInfo.InvariantCulture, out nDecimal);
            return nDecimal;
        }
        private void SetValueModel(NameValueCollection formCollection)
        {
            SetSessionName();

            _cnPedido = formCollection["cn_pedido"];
            _cnProforma = formCollection["cn_proforma"];
            _ccAnalis = SetValue(formCollection["scc_analis"], Session[_sessionCcAnalis]);
            _ccMoneda = SetValue(formCollection["scc_moneda"], Session[_sessionCcMoneda]);
            _ccVta = SetValue(formCollection["scc_vta"], Session[_sessionCcVta]);
            _cnSuc = SetValue(formCollection["scn_suc"], Session[_sessionCnSuc]);
            _Cn_lug = SetValue(formCollection["sCn_lug"], Session[_sessionCn_lug]);
            _IdContactoEntregaDirecta = SetValue(formCollection["sIdContactoEntregaDirecta"], Session[_sessionIdContactoEntregaDirecta]);
            _CC_transp = SetValue(formCollection["sCC_transp"], Session[_sessionCC_transp]);
            _ContactoTransporte = SetValue(formCollection["sContactoTransporte"], Session[_sessionContactoTransporte]);
            _Vt_observacion = SetValue(formCollection["sVt_observacion"], Session[_sessionVt_observacion]);
            _cn_ocompra = SetValue(formCollection["sCn_ocompra"], Session[_sessionCn_ocompra]);
            _cbRecojo = SetValue(formCollection["scb_recojo"], Session[_sessionCbRecojo]);
            _Tienda = SetValue(formCollection["sTienda"], Session[_sessionTienda]);
            _Igv_bo = Convert.ToInt32(SetValue(formCollection["sIgv_bo"], Session[_sessionIgv_bo]));
            _zonaLiberada_bo = Convert.ToInt32(SetValue(formCollection["sZonaLiberada"], Session[_sessionZonaLiberada]));
            _cFechaEmision = SetValue(formCollection["sfecha_emision"], Session[_sessionCfechaEmision]);
            _cFechaEntrega = SetValue(formCollection["sfecha_entrega"], Session[_sessionCfechaEntrega]);
            _fmTipCam = SetDecimal(formCollection["sfm_tipcam"], Session[_sessionFmTipCam]);
            _ccTipana = (string)Session[_sessionCcTipana];
            _cdRazsoc = (string)Session[_sessionCdRazSoc];

            if (!string.IsNullOrEmpty(_ccAnalis))
            {
                var cliente = ClienteService.GetByCodigo(_ccAnalis);
                _ccTipana = cliente.cc_tipana;
                _cdRazsoc = cliente.cd_razsoc;
            }

            Session[_sessionCcAnalis] = _ccAnalis;
            Session[_sessionCcTipana] = _ccTipana;
            Session[_sessionCdRazSoc] = _cdRazsoc;

            Session[_sessionCcMoneda] = _ccMoneda;
            Session[_sessionCcVta] = _ccVta;
            Session[_sessionCnSuc] = _cnSuc;
            Session[_sessionCn_lug] = _Cn_lug;
            Session[_sessionIdContactoEntregaDirecta] = _IdContactoEntregaDirecta;
            Session[_sessionCC_transp] = _CC_transp;
            Session[_sessionContactoTransporte] = _ContactoTransporte;
            Session[_sessionVt_observacion] = _Vt_observacion;
            Session[_sessionCn_ocompra] = _cn_ocompra;
            Session[_sessionCbRecojo] = _cbRecojo;
            Session[_sessionTienda] = _Tienda;
            Session[_sessionIgv_bo] = _Igv_bo;
            Session[_sessionZonaLiberada] = _zonaLiberada_bo;
            Session[_sessionCfechaEmision] = _cFechaEmision;
            Session[_sessionCfechaEntrega] = _cFechaEntrega;
            Session[_sessionCdAtencion] = _cdAtencion;
            Session[_sessionFmTipCam] = _fmTipCam;
        }
        #endregion

        #region SetSession
        private void SetSessionNull()
        {
            switch (_sessionTypes)
            {
                case SessionTypes.Nuevo:
                    Session[SessionCcTipanaNuevo] = null;
                    Session[SessionCcAnalisNuevo] = null;
                    Session[SessionCdRazSocNuevo] = null;
                    Session[SessionCcMonedaNuevo] = null;
                    Session[SessionCcVtaNuevo] = null;
                    Session[SessionCnSucNuevo] = null;
                    Session[SessionCn_lugNuevo] = null;
                    Session[SessionIdContactoEntregaDirectaNuevo] = null;
                    Session[SessionCC_transpNuevo] = null;
                    Session[SessionContactoTransporteNuevo] = null;
                    Session[SessionVt_observacionNuevo] = null;
                    Session[SessionCn_ocompraNuevo] = null;
                    Session[SessionCbRecojoNuevo] = null;
                    Session[SessionTiendaNuevo] = null;
                    Session[SessionIgv_boNuevo] = null;
                    Session[SessionZonaLiberadaNuevo] = null;
                    Session[SessionCfechaEmisionNuevo] = null;
                    Session[SessionCfechaEntregaNuevo] = null;
                    Session[SessionCdAtencionNuevo] = null;
                    Session[SessionFmTipCamNuevo] = null;
                    Session[SessionDetalleNuevo] = null;
                    break;
                case SessionTypes.Editar:
                    Session[SessionCcTipanaEditar] = null;
                    Session[SessionCcAnalisEditar] = null;
                    Session[SessionCdRazSocEditar] = null;
                    Session[SessionCcMonedaEditar] = null;
                    Session[SessionCcVtaEditar] = null;
                    Session[SessionCnSucEditar] = null;
                    Session[SessionCn_lugEditar] = null;
                    Session[SessionIdContactoEntregaDirectaEditar] = null;
                    Session[SessionCC_transpEditar] = null;
                    Session[SessionContactoTransporteEditar] = null;
                    Session[SessionVt_observacionEditar] = null;
                    Session[SessionCn_ocompraEditar] = null;
                    Session[SessionCbRecojoEditar] = null;
                    Session[SessionTiendaEditar] = null;
                    Session[SessionIgv_boEditar] = null;
                    Session[SessionZonaLiberadaEditar] = null;
                    Session[SessionCfechaEmisionEditar] = null;
                    Session[SessionCfechaEntregaEditar] = null;
                    Session[SessionCdAtencionEditar] = null;
                    Session[SessionFmTipCamEditar] = null;
                    Session[SessionDetalleEditar] = null;
                    break;
            }
        }
        private void SetSessionName()
        {
            switch (_sessionTypes)
            {
                case SessionTypes.Nuevo:
                    _sessionCcTipana = SessionCcTipanaNuevo;
                    _sessionCcAnalis = SessionCcAnalisNuevo;
                    _sessionCdRazSoc = SessionCdRazSocNuevo;
                    _sessionCcMoneda = SessionCcMonedaNuevo;
                    _sessionCcVta = SessionCcVtaNuevo;
                    _sessionCnSuc = SessionCnSucNuevo;
                    _sessionCn_lug = SessionCn_lugNuevo;
                    _sessionIdContactoEntregaDirecta = SessionIdContactoEntregaDirectaNuevo;
                    _sessionCC_transp = SessionCC_transpNuevo;
                    _sessionContactoTransporte = SessionContactoTransporteNuevo;
                    _sessionVt_observacion = SessionVt_observacionNuevo;
                    _sessionCn_ocompra = SessionCn_ocompraNuevo;
                    _sessionCbRecojo = SessionCbRecojoNuevo;
                    _sessionTienda = SessionTiendaNuevo;
                    _sessionIgv_bo = SessionIgv_boNuevo;
                    _sessionZonaLiberada = SessionZonaLiberadaNuevo;
                    _sessionCfechaEmision = SessionCfechaEmisionNuevo;
                    _sessionCfechaEntrega = SessionCfechaEntregaNuevo;
                    _sessionCdAtencion = SessionCdAtencionNuevo;
                    _sessionFmTipCam = SessionFmTipCamNuevo;
                    _sessionDetalle = SessionDetalleNuevo;
                    break;
                case SessionTypes.Editar:
                    _sessionCcTipana = SessionCcTipanaEditar;
                    _sessionCcAnalis = SessionCcAnalisEditar;
                    _sessionCdRazSoc = SessionCdRazSocEditar;
                    _sessionCcMoneda = SessionCcMonedaEditar;
                    _sessionCcVta = SessionCcVtaEditar;
                    _sessionCnSuc = SessionCnSucEditar;
                    _sessionCn_lug = SessionCn_lugEditar;
                    _sessionIdContactoEntregaDirecta = SessionIdContactoEntregaDirectaEditar;
                    _sessionCC_transp = SessionCC_transpEditar;
                    _sessionContactoTransporte = SessionContactoTransporteEditar;
                    _sessionVt_observacion = SessionVt_observacionEditar;
                    _sessionCn_ocompra = SessionCn_ocompraEditar;
                    _sessionCbRecojo = SessionCbRecojoEditar;
                    _sessionTienda = SessionTiendaEditar;
                    _sessionIgv_bo = SessionIgv_boEditar;
                    _sessionZonaLiberada = SessionZonaLiberadaEditar;
                    _sessionCfechaEmision = SessionCfechaEmisionEditar;
                    _sessionCfechaEntrega = SessionCfechaEntregaEditar;
                    _sessionCdAtencion = SessionCdAtencionEditar;
                    _sessionFmTipCam = SessionFmTipCamEditar;
                    _sessionDetalle = SessionDetalleEditar;
                    break;
            }
        }
        #endregion

        #region SetDropDownList
        private void SetDropDownList(string ccAnalis, string ccTransp = null)
        {
            ViewBag.GetClientes = GetClientes(ccAnalis);
            ViewBag.GetMonedas = GetMonedas();
            ViewBag.GetCondicionesVentas = CondicionesVentas(ccAnalis);
            ViewBag.GetArticulos = GetArticulos();
            ViewBag.GetSucursales = GetSucursalesCliente(ccAnalis);
            ViewBag.GetLugarEntrega = GetLugarEntregaCliente(ccAnalis);
            ViewBag.GetTransporte = GetTransporteCliente(ccAnalis);
            ViewBag.GetRecojo = GetRecojo();
            ViewBag.GetTiendas = GetTiendas();
            ViewBag.GetIgv = GetIgv();
            ViewBag.GetZonaLiberada = GetZonaLiberada();
            ViewBag.GetMostrarIGV = GetMostrarIGV();
            ViewBag.GetContactoTransporte = GetContactoTransporte(ccTransp);
            ViewBag.GetDepartamentos = GetDepartamentos();
            //
            ViewBag.GetContactoEntregaDirecta = GetContactoEntregaDirecta(ccAnalis);
        }
        private SelectList GetClientes(string ccAnalis = null)
        {
            IList<ClienteModel> listaModel = new List<ClienteModel>();
            if (!string.IsNullOrEmpty(ccAnalis))
            {
                var clienteSelect = ClienteService.GetByCodigo(ccAnalis);
                listaModel.Add(clienteSelect);
            }
            var selectList = listaModel.ToSelectList(x => x.cd_razsoc.Trim(), x => x.cc_analis,
                     FindTypes.Ninguno.ToString());
            return NewSelectList(selectList);
        }
        private SelectList GetEstados()
        {
            var lista = new List<SelectListItem>
                {
                    //new SelectListItem
                    //{
                    //    Text = FindTypes.Ninguno.ToString(),
                    //    Value = FindTypes.Ninguno.ToString(),
                    //    Selected = true
                    //},
                    new SelectListItem
                    {
                        Text = EstadoPedidoTypes.Emitido.ToString(),
                        Value = EstadoPedidoTypes.Emitido.ToString(),
                    },
                    new SelectListItem
                    {
                        Text = EstadoPedidoTypes.Enviado.ToString(),
                        Value = EstadoPedidoTypes.Enviado.ToString(),
                    },
                    new SelectListItem
                    {
                        Text = EstadoPedidoTypes.Procesado.ToString(),
                        Value = EstadoPedidoTypes.Procesado.ToString(),
                    },
                    new SelectListItem
                    {
                        Text = EstadoPedidoTypes.Anulado.ToString(),
                        Value = EstadoPedidoTypes.Anulado.ToString(),
                    }
                };
            return NewSelectList(lista);
        }
        private SelectList CondicionesVentas(string ccAnalis)
        {
            if (string.IsNullOrEmpty(ccAnalis))
            {
                ccAnalis = "";
            }
            var lista = CondicionesVentasService.GetAll(ccAnalis).ToSelectList(x => x.cd_vta.Trim(), x => x.cc_vta,
                    FindTypes.Ninguno.ToString());
            return NewSelectList(lista);
        }
        private SelectList GetMonedas()
        {
            var lista = MonedaService.GetAll().ToSelectList(x => x.cd_simbolo.Trim(), x => x.cc_moneda,
                    FindTypes.Ninguno.ToString());
            return NewSelectList(lista);
        }
        private SelectList GetArticulos()
        {
            var lista = //ArticuloService.GetAllCodDes()
                (new List<ArticuloModel>())
                .ToSelectList(x => x.cd_artic.Trim(), x => x.cc_artic,
                    FindTypes.Ninguno.ToString());
            return NewSelectList(lista);
        }
        private SelectList GetSucursalesCliente(string ccAnalis)
        {
            if (!string.IsNullOrWhiteSpace(ccAnalis) && !string.IsNullOrEmpty(ccAnalis))
            {
                var lista = SucursalClienteService
                .GetByCcAnalis(ccAnalis)
                .ToSelectList(x => x.cn_suc.Trim() + " - " + x.cd_direc.Trim(), x => x.cn_suc,
                    FindTypes.Ninguno.ToString());
                return NewSelectList(lista);
            }
            var listaVacia = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = FindTypes.Ninguno.ToString(),
                    Value = FindTypes.Ninguno.ToString(),
                    Selected = true
                }
            };
            return NewSelectList(listaVacia);
        }
        private SelectList GetLugarEntregaCliente(string ccAnalis)
        {
            if (!string.IsNullOrWhiteSpace(ccAnalis) && !string.IsNullOrEmpty(ccAnalis))
            {
                var lista = SucursalClienteService
                .GetLugarEntregaByCcAnalis(ccAnalis)
                .ToSelectList(x => x.cd_direc.Trim(), x => x.cn_suc,
                    FindTypes.Ninguno.ToString());
                return NewSelectList(lista);
            }
            var listaVacia = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = FindTypes.Ninguno.ToString(),
                    Value = FindTypes.Ninguno.ToString(),
                    Selected = true
                }
            };
            return NewSelectList(listaVacia);
        }
        private SelectList GetContactoEntregaDirecta(string ccAnalis)
        {
            if (!string.IsNullOrWhiteSpace(ccAnalis) && !string.IsNullOrEmpty(ccAnalis))
            {
                var lista = ClienteService
                .GetContactoEntregaDirectaByccAnalis(ccAnalis)
                .ToSelectList(x => x.nombreContacto.Trim(), x => x.idContacto,
                    FindTypes.Ninguno.ToString());
                return NewSelectList(lista);
            }
            var listaVacia = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = FindTypes.Ninguno.ToString(),
                    Value = FindTypes.Ninguno.ToString(),
                    Selected = true
                }
            };
            return NewSelectList(listaVacia);
        }

        private SelectList GetTransporteCliente(string ccAnalis)
        {
            if (!string.IsNullOrWhiteSpace(ccAnalis) && !string.IsNullOrEmpty(ccAnalis))
            {
                var lista = SucursalClienteService
                .GetTransporteByCcAnalis(ccAnalis)
                .ToSelectList(x => x.cd_direc.Trim(), x => x.cn_suc,
                    FindTypes.Ninguno.ToString());
                return NewSelectList(lista);
            }
            var listaVacia = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = FindTypes.Ninguno.ToString(),
                    Value = FindTypes.Ninguno.ToString(),
                    Selected = true
                }
            };
            return NewSelectList(listaVacia);
        }
        private SelectList GetContactoTransporte(string ccTransporte)
        {
            if (!string.IsNullOrWhiteSpace(ccTransporte) && !string.IsNullOrEmpty(ccTransporte))
            {
                var lista = ContactoTransportistaRepository.getByTransportistaActivos(ccTransporte)
                .ToSelectList(x => x.descri.Trim(), x => x.codigo);
                return NewSelectList(lista);
            }
            else
            {
                var listaVacia = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = FindTypes.Ninguno.ToString(),
                        Value = FindTypes.Ninguno.ToString(),
                        Selected = true
                    }
                };
                return NewSelectList(listaVacia);
            }
        }
        private SelectList GetDepartamentos()
        {
            var lista = UbigeoRepository
            .getDepartamentos()
            .ToSelectList(x => x.cd_dpto.Trim(), x => x.cc_dpto);
            return NewSelectList(lista);
        }
        private SelectList GetProvincia(string departamento)
        {
            var lista = UbigeoRepository
            .getProvincia(departamento)
            .ToSelectList(x => x.cd_prov.Trim(), x => x.cc_prov);
            return NewSelectList(lista);
        }
        private SelectList GetDistrito(string departamento, string provincia)
        {
            var lista = UbigeoRepository
            .getDistrito(departamento, provincia)
            .ToSelectList(x => x.cd_distrito.Trim(), x => x.cc_distrito);
            return NewSelectList(lista);
        }
        private SelectList GetZona(string departamento, string provincia, string distrito)
        {
            var lista = UbigeoRepository
            .getZona(departamento, provincia, distrito)
            .ToSelectList(x => x.cd_zona.Trim(), x => x.cc_zona);
            return NewSelectList(lista);
        }
        private IEnumerable<object> GetLugarEntregaClienteTodos(string ccAnalis)
        {
            return SucursalClienteRepository.GetLugarEntregaClienteTodos(ccAnalis);
        }
        private IEnumerable<object> GetContactoEntregaDirectaClienteTodos(string ccAnalis)
        {
            return ClienteRepository.GetContactoEntregaDirectaClienteTodos(ccAnalis);
        }
        private IEnumerable<object> GetTransporteClienteTodos(string ccAnalis)
        {
            return SucursalClienteRepository.GetTransporteClienteTodos(ccAnalis);
        }

        private SelectList GetRecojo()
        {
            var listaVacia = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Recojo en Almacén",
                    Value = "1",
                    Selected = true
                },
                new SelectListItem
                {
                    Text = "Entrega Directa",
                    Value = "2",
                },
                new SelectListItem
                {
                    Text = "Envío por Agencia",
                    Value = "3",
                }
            };
            return NewSelectList(listaVacia);
        }
        //private SelectList GetTienda()
        //{

        //    var listaVacia = new List<SelectListItem>
        //    {
        //        new SelectListItem
        //        {
        //            Text = "Recojo en Almacén",
        //            Value = "1",
        //            Selected = true
        //        },
        //        new SelectListItem
        //        {
        //            Text = "Entrega Directa",
        //            Value = "2",
        //        },
        //        new SelectListItem
        //        {
        //            Text = "Envío por Agencia",
        //            Value = "3",
        //        }
        //    };
        //    return NewSelectList(listaVacia);
        //}
        private SelectList GetGrupo()
        {
            //var lista = ProductoService.GetFamiliasSp().ToSelectList(x => x.cd_gruart, x => x.cc_gruartec,
            //        FindTypes.Ninguno.ToString());

            string empresa = "";
            var sqlDB = new System.Data.SqlClient.SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            if (sqlDB.InitialCatalog == "ZICO_ERP01")
            {
                empresa = "acetesa";
            }
            else
            {
                empresa = "galpesa";
            }


            var lista = ProductoService.ListarFamiliasArticulosPorEmpresa(empresa).ToSelectList(x => x.cd_gruart, x => x.cc_gruartec, FindTypes.Ninguno.ToString());

            return NewSelectList(lista);
        }
        private SelectList GetSubGrupo(string codSubGrupo = null)
        {
            //if (string.IsNullOrEmpty(codSubGrupo))
            //{
            //    return NewSelectList(null);
            //}
            //var lista = ProductoService.GetSubFamiliasByCodFamiliaSp(codSubGrupo)
            //    .ToSelectList(x => x.cd_subgruart, x => x.cc_subgruart,
            //        FindTypes.Ninguno.ToString());

            string empresa = "";
            var sqlDB = new System.Data.SqlClient.SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            if (sqlDB.InitialCatalog == "ZICO_ERP01")
            {
                empresa = "acetesa";
            }
            else
            {
                empresa = "galpesa";
            }

            if (string.IsNullOrEmpty(codSubGrupo))
            {
                return NewSelectList(null);
            }


            var lista = ProductoService.ListarSubFamiliasArticulosPorEmpresa(empresa, codSubGrupo)
                .ToSelectList(X => X.cd_subgruart, X => X.cc_subgruart, FindTypes.Ninguno.ToString());

            return NewSelectList(lista);
        }

        #endregion

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetSubGrupos(string codGrupo)
        {
            var list = GetSubGrupo(codGrupo);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult verificaRUC(string ruc)
        {
            var resultado = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.sunat.cloud/ruc/");
                    //HTTP GET
                    var responseTask = client.GetAsync(ruc);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();

                        resultado = readTask.Result;
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = "{\"error\":\"No se pudo validar el Nro de RUC.\"}";
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetPrecioLista(string ccArtic, string cc_tienda, int igv_bo)
        {
            var precios = ListaPrecioService.GetPreciosByCodArticulo(ccArtic, User.Identity.Name, cc_tienda, null, igv_bo);
            return JsonSuccess(precios);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult ChangeTipoCambio(string fechaEmision, SessionTypes sessionType)
        {
            _sessionTypes = sessionType;

            SetSessionName();

            if (string.IsNullOrEmpty(fechaEmision) || string.IsNullOrWhiteSpace(fechaEmision))
            {
                return JsonSuccess(-1);
            }

            if (!(fechaEmision).IsDateTime())
            {
                return JsonSuccess(-2);
            }

            var dFechaEmision = (fechaEmision).ConvertDateTime();
            var tipoCambioDiario = TipoCambioDiarioService.GetByFechaTipoCambio(dFechaEmision);
            Session[_sessionCfechaEmision] = fechaEmision;
            return JsonSuccess(tipoCambioDiario);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult ChangePreciosByMoneda(string tipoMoneda, string tipoCambio, SessionTypes sessionType)
        {
            _sessionTypes = sessionType;

            SetSessionName();

            var tempDetalle = Session[_sessionDetalle];
            var tempDetalleCast = ((tempDetalle) as List<PedidoDetailViewModel>);
            if (tempDetalleCast == null)
            {
                return JsonSuccess(0);
            }

            if (string.IsNullOrEmpty(tipoCambio))
            {
                return JsonSuccess(0);
            }

            decimal nTipoCambio;
            decimal.TryParse(tipoCambio, out nTipoCambio);

            if (nTipoCambio == default(decimal))
            {
                return JsonSuccess(0);
            }

            var moneda = MonedaService.GetCdMonedaByCcMoneda(tipoMoneda);

            if (moneda == null)
            {
                return JsonSuccess(0);
            }

            var sTipoMoneda = moneda.cd_simbolo.Trim();
            Session[_sessionCcMoneda] = tipoMoneda;
            foreach (var item in tempDetalleCast)
            {
                var listaPrecio = ListaPrecioService.GetCcListaByCcArtic(item.cc_artic);
                item.cc_lista = listaPrecio.cc_lista;
                var precios = ListaPrecioService.GetPreciosByCodArticulo(item.cc_artic, User.Identity.Name, null, item.cc_lista, item.igv_bo);
                if (precios != null)
                {
                    decimal precioLista = 0;
                    decimal precioLista2 = 0;
                    switch (sTipoMoneda)
                    {
                        case "S/.":
                            precioLista = precios.fm_precio_mn;
                            precioLista2 = precios.fm_precio2_mn;
                            break;
                        case "US$":
                            precioLista = precios.fm_precio_me;
                            precioLista2 = precios.fm_precio2_me;
                            break;
                    }
                    item.fm_precio = precioLista;
                    item.fm_precio2 = precioLista2;
                }
                var cantidad = item.fq_cantidad;
                decimal precioFinal = 0;
                switch (sTipoMoneda)
                {
                    case "S/.":
                        precioFinal = item.fm_precio_fin * nTipoCambio;
                        item.fm_precio_fin = precioFinal;
                        break;
                    case "US$":
                        precioFinal = item.fm_precio_fin / nTipoCambio;
                        item.fm_precio_fin = precioFinal;
                        break;
                }
                item.fm_total = cantidad * precioFinal;
            }
            Session[_sessionDetalle] = tempDetalleCast;
            return PartialView("_PartialPedidoDetail", tempDetalleCast);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult NuevoDetalle(PedidoEditViewModel model, SessionTypes sessionType)
        {
            _sessionTypes = sessionType;

            SetSessionName();

            var ccAnalis = model.cc_analis;
            if (!string.IsNullOrEmpty(ccAnalis) && !string.IsNullOrWhiteSpace(ccAnalis))
            {
                var cliente = ClienteService.GetByCodigo(ccAnalis);
                model.cc_tipana = cliente.cc_tipana;
                model.cd_razsoc = cliente.cd_razsoc;
                Session[_sessionCcTipana] = model.cc_tipana;
                Session[_sessionCcAnalis] = model.cc_analis;
                Session[_sessionCdRazSoc] = model.cd_razsoc;
                Session[_sessionCcMoneda] = model.cc_moneda;
                Session[_sessionCcVta] = model.cc_vta;
                Session[_sessionCfechaEmision] = model.FechaEmision;
                Session[_sessionCfechaEntrega] = model.FechaEntrega;
                Session[_sessionCnSuc] = model.cn_suc;
                Session[_sessionCn_lug] = model.Cn_lug;
                Session[_sessionIdContactoEntregaDirecta] = model.IdContactoEntregaDirecta;
                Session[_sessionCC_transp] = model.CC_transp;
                Session[_sessionContactoTransporte] = model.ContactoTransporte;
                Session[_sessionVt_observacion] = model.Vt_observacion;
                Session[_sessionCn_ocompra] = model.cn_ocompra;
                Session[_sessionCbRecojo] = model.cb_recojo;
                Session[_sessionTienda] = model.Tienda;
                Session[_sessionIgv_bo] = model.igv_bo;
            }
            var ccArtic = model.PedidoDetailViewModel.cc_artic;
            var cc_lista = model.PedidoDetailViewModel.cc_lista;
            model.PedidoDetailViewModel.cc_artic = ccArtic;


            if (!string.IsNullOrEmpty(ccArtic) && !string.IsNullOrWhiteSpace(ccArtic))
            {
                var articulo = ArticuloService.GetByCodigo(ccArtic);
                model.PedidoDetailViewModel.cc_unmed = articulo.cc_unmed;

                var listaPrecio = ListaPrecioService.GetCcListaByCcArtic(ccArtic);
                model.PedidoDetailViewModel.cc_lista = listaPrecio.cc_lista;

                var precios = ListaPrecioService.GetPreciosByCodArticulo(ccArtic, User.Identity.Name, model.Tienda, null, model.igv_bo);
                if (precios != null)
                {
                    decimal precioLista = 0;
                    decimal precioLista2 = 0;
                    var moneda = MonedaService.GetCdMonedaByCcMoneda(model.cc_moneda);
                    switch (moneda.cd_simbolo.Trim())
                    {
                        case "S/.":
                            precioLista = precios.fm_precio_mn;
                            precioLista2 = precios.fm_precio2_mn;
                            break;
                        case "US$":
                            precioLista = precios.fm_precio_me;
                            precioLista2 = precios.fm_precio2_me;
                            break;
                    }
                    model.PedidoDetailViewModel.fm_precio = precioLista;
                    model.PedidoDetailViewModel.fm_precio2 = precioLista2;
                }
                var cantidad = model.PedidoDetailViewModel.fq_cantidad;
                var precioFinal = model.PedidoDetailViewModel.fm_precio_fin;
                model.PedidoDetailViewModel.fm_total = cantidad * precioFinal;
                model.PedidoDetailViewModel.igv_bo = model.igv_bo;
                model.PedidoDetailViewModel.zonaLiberada_bo = model.zonaLiberada_bo;

            }
            ModelState.Clear();
            TryUpdateModel(model);
            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    return JsonValidationError();
                }
                return View();
            }
            //Peso Unit. x 1000 = Peso TM
            //var pesoTeorico = model.PedidoDetailViewModel.fq_peso_teorico;
            //model.PedidoDetailViewModel.fq_peso_teorico = pesoTeorico * 1000;
            //
            //Valida que la cantidad sea mayor a 0
            if (model.PedidoDetailViewModel.fq_cantidad <= (decimal)0.00)
            {
                var messageArticuloExiste = "Debe ingresar una cantidad válida";
                if (Request.IsAjaxRequest())
                {
                    return JsonError(messageArticuloExiste);
                }
            }

            var tempDetalle = Session[_sessionDetalle];
            if (tempDetalle == null)
            {
                model.PedidoDetailViewModel.cn_item = ("1").PadLeft(2, '0');
                var detail = new List<PedidoDetailViewModel> { model.PedidoDetailViewModel };
                Session[_sessionDetalle] = detail;
            }
            else
            {
                var detailCast = (List<PedidoDetailViewModel>)(tempDetalle);
                //Actualiza el cnItem
                int i = 0;
                foreach (var detalle in detailCast)
                {
                    i++;
                    var secuencial = i.ToString("00");
                    detalle.cn_item = secuencial;
                }
                var detail = detailCast
                .FirstOrDefault(x => x.cc_artic.Trim() == model.PedidoDetailViewModel.cc_artic.Trim());

                if (detail != null)
                {
                    var messageArticuloExiste = "Ya ha agregado el artículo: '" + model.PedidoDetailViewModel.cd_artic.Trim().ToUpper();
                    var isAjax = Request.IsAjaxRequest();
                    if (isAjax)
                    {


                        return JsonError(messageArticuloExiste);
                    }
                    ModelState.AddModelError(string.Empty, messageArticuloExiste);
                    //ViewBag.MessageValidation = messageArticuloExiste;
                    return View();

                }
                var ultimoDetalle = detailCast.LastOrDefault();
                var cnItem = ultimoDetalle != null ? ultimoDetalle.cn_item : "0";
                var nCnItem = Convert.ToInt32(cnItem);
                model.PedidoDetailViewModel.cn_item = (nCnItem + 1).ToString().PadLeft(2, '0');
                /*Quitar stock a descripcion Articulo*/
                var cdArtic = model.PedidoDetailViewModel.cd_artic;
                int ixFrom = cdArtic.IndexOf("==");
                int charsQ = cdArtic.Length - ixFrom;
                model.PedidoDetailViewModel.cd_artic = cdArtic.Remove(ixFrom, charsQ).Trim();

                detailCast.Add(model.PedidoDetailViewModel);
                Session[_sessionDetalle] = detailCast;

            }
            return PartialView("_PartialPedidoDetail", Session[_sessionDetalle]);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [HttpGet]
        public ActionResult EliminarFila(string id, SessionTypes sessionTypes)
        {
            _sessionTypes = sessionTypes;

            SetSessionName();

            var tempDetalle = Session[_sessionDetalle];
            if (tempDetalle == null)
            {
                return PartialView("_PartialPedidoDetail", null);
            }
            var detailCast = (List<PedidoDetailViewModel>)(tempDetalle);
            var detail = detailCast.FirstOrDefault(x => x.cc_artic.Trim() == id);
            if (detail != null)
            {
                detailCast.Remove(detail);
            }
            Session[_sessionDetalle] = detailCast;
            return PartialView("_PartialPedidoDetail", Session[_sessionDetalle]);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult EditarAjax(FormCollection formCollection, int? page)
        {
            _sessionTypes = SessionTypes.Editar;

            SetValueModel(formCollection);

            var model = new PedidoEditViewModel
            {
                cn_pedido = _cnPedido,
                cn_proforma = _cnProforma,
                cc_tipana = _ccTipana,
                cc_analis = _ccAnalis,
                cd_razsoc = _cdRazsoc,
                cc_moneda = _ccMoneda,
                FechaEmision = _cFechaEmision,
                FechaEntrega = _cFechaEntrega,
                fm_tipcam = _fmTipCam,
                cc_vta = _ccVta,
                EnviarMailViewModel = new EnviarMailViewModel(),
                LugarEntregaNewViewModel = new LugarEntregaNewViewModel(),
                ContactoEntregaDirectaNewViewModel = new ContactoEntregaDirectaNewViewModel(),
                TransportistaNewViewModel = new TransportistaNewViewModel(),
                ContactoTransporteNewModel = new ContactoTransporteNewModel(),
                cn_suc = _cnSuc,
                Cn_lug = _Cn_lug,
                IdContactoEntregaDirecta = _IdContactoEntregaDirecta,
                CC_transp = _CC_transp,
                ContactoTransporte = _ContactoTransporte,
                Vt_observacion = _Vt_observacion,
                cn_ocompra = _cn_ocompra,
                cb_recojo = _cbRecojo,
                Tienda = _Tienda,
                igv_bo = _Igv_bo,
                zonaLiberada_bo = _zonaLiberada_bo
            };

            try
            {
                var estadoPedido = PedidoService.GetById(model.cn_pedido);
                var estadoProcesado = ((int)EstadoPedidoTypes.Procesado).ToString();
                var estadoEnviado = ((int)EstadoPedidoTypes.Enviado).ToString();
                var estadoAnulado = ((int)EstadoPedidoTypes.Anulado).ToString();
                if (estadoPedido != null && estadoPedido.cb_estado == estadoProcesado)
                {
                    return JsonError("El Pedido fue procesado.");

                }
                if (estadoPedido != null && estadoPedido.cb_estado == estadoAnulado)
                {
                    return JsonError("La Pedido fue anulado.");
                }

                ModelState.Clear();
                TryUpdateModel(model);
                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    return JsonError(message);
                }
                try
                {
                    ModeloValidoExtendido(model);
                }
                catch (Exception exce)
                {
                    return JsonError(exce.Message);
                }

                var tempDetalle = Session[SessionDetalleEditar];
                var tempDetalleCast = ((tempDetalle) as IList<PedidoDetailViewModel>);
                if (tempDetalleCast == null || tempDetalleCast.Count == 0)
                {
                    return JsonError("No existen artículos.");
                }

                var entityMaster = new LCPEDIDO_WEB
                {
                    cn_pedido = model.cn_pedido,
                    cn_proforma = model.cn_proforma,
                    cc_tipana = model.cc_tipana,
                    cc_analis = model.cc_analis,
                    cd_razsoc = model.cd_razsoc,
                    cc_moneda = model.cc_moneda,
                    cc_vta = model.cc_vta,
                    df_emision = model.FechaEmision.ConvertDateTime(),
                    fm_tipcam = model.fm_tipcam,
                    cn_suc = model.cn_suc,
                    cb_recojo = model.cb_recojo,
                    cb_estado = "2"
                };

                var detailPedidoCast = (IList<PedidoDetailViewModel>)(tempDetalle);
                var item = 0;
                var pedidoDetalleList = detailPedidoCast.Select(detail => new LDPEDIDO_WEB
                {
                    cn_item = (++item).ToString().PadLeft(2, '0'),
                    cc_artic = detail.cc_artic,
                    fq_cantidad = detail.fq_cantidad,
                    fq_stock = 0,
                    cc_lista = detail.cc_lista,
                    fm_precio = detail.fm_precio,
                    fm_precio2 = detail.fm_precio2,
                    fm_precio_fin = detail.fm_precio_fin,
                    fm_total = detail.fm_total
                }).ToList();

                PedidoService.PedidoDetalleServices = pedidoDetalleList;
                PedidoService.Guardar(entityMaster, model.igv_bo, EmpresaSegunBD(), model.zonaLiberada_bo);
                switch (model.cb_recojo)
                {
                    case "1":
                        model.Cn_lug = "";
                        model.IdContactoEntregaDirecta = "";
                        model.CC_transp = "";
                        model.ContactoTransporte = "";
                        break;
                    case "2":
                        model.CC_transp = "";
                        model.ContactoTransporte = "";
                        break;
                    case "3":
                        model.Cn_lug = "";
                        model.IdContactoEntregaDirecta = "";
                        break;
                }
                PedidoService.GuardarAdicional(entityMaster, User.Identity.Name, model.Cn_lug + "", model.CC_transp + "", model.Vt_observacion + "", model.ContactoTransporte + "", model.IdContactoEntregaDirecta + "", model.Tienda + "", model.FechaEntrega.ConvertDateTime(), model.igv_bo, model.cn_ocompra, model.zonaLiberada_bo);

                //Inicio: Registra la Nota de Pedido Tortuga
                LCPEDIDOADICIONAL_WEB PedidoAdicional = new LCPEDIDOADICIONAL_WEB();
                PedidoAdicional.Cn_lug = model.Cn_lug;
                PedidoAdicional.cn_ocompra = model.cn_ocompra;
                PedidoAdicional.Vt_observacion = model.Vt_observacion;
                PedidoAdicional.cc_tienda = model.Tienda;
                PedidoAdicional.CC_transp = model.CC_transp;
                PedidoAdicional.FechaEntrega = DateTime.Parse(model.FechaEntrega);
                PedidoAdicional.ContactoTransporte = model.ContactoTransporte;

                    var xml = new XDocument(new XElement("DetallePedido",
                                     from pedidoDetalle in pedidoDetalleList
                                     select new XElement("Articulo",
                                       new XElement("cn_item", pedidoDetalle.cn_item),
                                       new XElement("cc_artic", pedidoDetalle.cc_artic),
                                       new XElement("cc_unmed", pedidoDetalle.MARTICUL.cc_unmed),
                                       new XElement("fq_cantped", pedidoDetalle.fq_cantidad),
                                       new XElement("fm_valunit", pedidoDetalle.fm_precio2),
                                       new XElement("fm_monvta", pedidoDetalle.fm_total),
                                       new XElement("cd_artic", pedidoDetalle.MARTICUL.cd_artic),
                                       new XElement("fq_embalaje", pedidoDetalle.MARTICUL.fq_embalaje),
                                       new XElement("fm_valuni_cd", pedidoDetalle.fm_precio_fin)
                                       )
                                ));

                var resultadoRNPV = PedidoService.RegistrarNotaPedidoVenta(entityMaster, PedidoAdicional, xml.ToString(), User.Identity.Name);
                if (resultadoRNPV["mensajeID"] == "0")
                {
                    return JsonError(resultadoRNPV["mensaje"]);
                }
                //Fin: Registra la Nota de Pedido Tortuga

                TempData["Guardado"] = true;

                return JsonSuccess(1);
            }
            catch (DbEntityValidationException e)
            {
                return JsonError(e.ToString());
            }
            catch (Exception ex)
            {
                return JsonError(ex.ToString());
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult StockSegunArticulo(string idArticulo)
        {
            List<ArticuloModel.Stock> lista = new List<ArticuloModel.Stock>();
            lista = ArticuloService.StockSegunArticulo(idArticulo);
            return JsonSuccess(lista);
        }
        private SelectList GetTiendas()
        {
            var lista = TiendaRepository.getTiendas().ToSelectList(x => x.descri.Trim(), x => x.codigo);
            return NewSelectList(lista);
        }

        //Dando de baja esta validacion
        //public ActionResult TiendaSegunVendedor()
        //{
        //    var lista = TiendaService.GetTiendaSegunVendedor(User.Identity.Name);
        //    return JsonSuccess(lista);
        //}

        private SelectList GetIgv()
        {
            var listaVacia = new List<SelectListItem>();
            var sqlDB = new System.Data.SqlClient.SqlConnectionStringBuilder(
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            //if (sqlDB.InitialCatalog == "ZICO_ERP04")
            //{
            //    listaVacia = new List<SelectListItem> {
            //        new SelectListItem
            //        {
            //            Text = "Sin IGV",
            //            Value = "0",
            //        }
            //    };
            //}
            //else
            //{
            listaVacia = new List<SelectListItem> {
                    new SelectListItem {
                        Text = "Con IGV",
                        Value = "1"
                    },
                    new SelectListItem {
                        Text = "Sin IGV",
                        Value = "0",
                    }

                };
            //}


            return NewSelectList(listaVacia);
        }

        private SelectList GetZonaLiberada()
        {
            var lista = new List<SelectListItem>();
            var sqlDB = new System.Data.SqlClient.SqlConnectionStringBuilder(
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            if (sqlDB.InitialCatalog == "ZICO_ERP04")
            {
                lista = new List<SelectListItem> {
                    new SelectListItem
                    {
                        Text = "Si",
                        Value = "1",
                    },
                    new SelectListItem {
                        Text = "No",
                        Value = "0"
                    }
                };
            }
            else
            {
                lista = new List<SelectListItem> {
                    new SelectListItem {
                        Text = "No",
                        Value = "0",
                    }
                };
            }


            return NewSelectList(lista);
        }

        private SelectList GetMostrarIGV()
        {
            var lista = new List<SelectListItem>();
            var sqlDB = new System.Data.SqlClient.SqlConnectionStringBuilder(
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            if (sqlDB.InitialCatalog == "ZICO_ERP04")
            {
                lista = new List<SelectListItem> {
                    new SelectListItem
                    {
                        Text = "Si",
                        Value = "1",
                    },
                    new SelectListItem {
                        Text = "No",
                        Value = "0"
                    }
                };
            }
            else
            {
                lista = new List<SelectListItem> {
                    new SelectListItem {
                        Text = "Si",
                        Value = "1",
                    }
                };
            }


            return NewSelectList(lista);
        }

        public ActionResult Sectores()
        {
            var lista = ClienteService.ListarSector();
            return JsonSuccess(lista);
        }
        public ActionResult Departamentos()
        {
            var lista = ClienteService.ListarDepartamento();
            return JsonSuccess(lista);
        }
        public ActionResult Provincias(string cc_dpto)
        {
            var lista = ClienteService.ListarProvincia(cc_dpto);
            return JsonSuccess(lista);
        }
        public ActionResult Distritos(string cc_dpto, string cc_prov)
        {
            var lista = ClienteService.ListarDistrito(cc_dpto, cc_prov);
            return JsonSuccess(lista);
        }
        public ActionResult ValidarVendedorCliente(string cc_analis)
        {
            var lista = ClienteService.ValidarRelacionVendedorCliente(cc_analis, User.Identity.Name);
            return JsonSuccess(lista);
        }
        public ActionResult ValidarClienteEnZonaLiberada(string ruc)
        {
            var resultado = ClienteService.ValidarClienteEnZonaLiberada(ruc);
            return JsonSuccess(resultado);
        }

        //public ActionResult RegistrarDocumentoOC(string idPedido, byte[] documento)
        //{
        //    string usuarioRegistro = User.Identity.Name;
        //    var resultado = "";// PedidoService.RegistrarDocumentoOCPorPedido(idPedido,usuarioRegistro, documento);
        //    return JsonSuccess(resultado);
        //}


        public ActionResult RegistrarDocumentoOC(string idPedido, HttpPostedFileWrapper myFile)
        {
            var fileName = Path.GetFileName(myFile.FileName);

            BinaryReader b = new BinaryReader(myFile.InputStream);
            byte[] binData = b.ReadBytes(myFile.ContentLength);

            //using (var ms = new MemoryStream())
            //{
            //    myFile.CopyTo(ms);
            //    byte[] fileBytes = ms.ToArray();
            //}

            string usuarioRegistro = User.Identity.Name;
            var resultado = PedidoService.RegistrarDocumentoOCPorPedido(idPedido, usuarioRegistro, binData);
            //    return JsonSuccess(resultado);

            //save fileName and fileBytes into database
            return JsonSuccess(resultado);
        }

        #region VALIDACIONES PEDIDO WEB A NOTA DE PEDIDO DIRECTO

        public ActionResult ValidaCreditoSobregiroPorPedido(string ruc, decimal totalPedido, string monedaPedido)
        {
            var resultado = PedidoService.ValidaCreditoSobregiroPorPedido(ruc, totalPedido, monedaPedido);

            return JsonSuccess(resultado);
        }

        #endregion

    }



}