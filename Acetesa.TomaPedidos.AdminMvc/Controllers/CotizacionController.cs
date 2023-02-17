using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Acetesa.TomaPedidos.AdminMvc.Helpers;
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

namespace Acetesa.TomaPedidos.AdminMvc.Controllers
{
    [Authorize]
    public class CotizacionController : BaseController
    {
        private const int PageSize = 50;

        public IClienteService ClienteService { get; set; }
        public IEstadoService EstadoService { get; set; }
        public ICotizacionService CotizacionService { get; set; }
        public IVendedorService VendedorService { get; set; }
        public ICotizacionDetalleService CotizacionDetalleService { get; set; }
        public IMonedaService MonedaService { get; set; }
        public IPlanificacionVisitasService PlanificacionVisitasService { get; set; }
        public ICondicionesVentasService CondicionesVentasService { get; set; }
        public IArticuloService ArticuloService { get; set; }
        public IListaPrecioService ListaPrecioService { get; set; }
        public ITipoCambioDiarioService TipoCambioDiarioService { get; set; }
        public ITiendaService TiendaService { get; set; }
        public IProductoService ProductoService { get; set; }
        public ISucursalClienteService SucursalClienteService  { get; set; }

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
            var model = new CotizacionFindViewModel();
            var pageNumber = (page ?? 1);
            try
            {
                var fechaInicio = (model.FechaInicio + " 00:00:00").ConvertDateTime();
                var fechaFinal = (model.FechaFinal + " 23:59:59").ConvertDateTime();
                var estadoTodos = "0";//((int)EstadoCotizacionTypes.Por_Enviar).ToString();
                var result = CotizacionService.GetCotizacionesByClienteFecInicioFecFinal(model.Cliente, fechaInicio, fechaFinal, User.Identity.Name, estadoTodos);
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
        public ActionResult GetSucursalesJson(string ccAnalis)
        {
            return JsonSuccess(GetSucursalesCliente(ccAnalis));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetVisitasClientesJson(string ccAnalis,string fechaEmision)
        {
            return JsonSuccess(GetVisitasClientes(ccAnalis, fechaEmision));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult ContactoPorSucursal(string cn_suc, string cc_analis)
        {
            ContactoSucursalRepository csr = new ContactoSucursalRepository();
            List<TCONTACLIE> listaContacto = new List<TCONTACLIE>();
            listaContacto = csr.ListarContactoPorSucursal(cn_suc, cc_analis);
            var lista = listaContacto.ToSelectList(x => x.cd_contacto.Trim(), x => x.cn_contacto);
            return JsonSuccess(listaContacto);
        }
        public SelectList SelectContactoPorSucursal(string cn_suc,string cc_analis)
        {
            if (!string.IsNullOrWhiteSpace(cn_suc) && !string.IsNullOrEmpty(cn_suc))
            {
                ContactoSucursalRepository csr = new ContactoSucursalRepository();
                List<TCONTACLIE> listaContacto = new List<TCONTACLIE>();
                listaContacto = csr.ListarContactoPorSucursal(cn_suc, cc_analis);
                var lista = listaContacto.ToSelectList(x => x.cd_contacto.Trim(), x => x.cn_contacto);
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

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetCondicionesVentasJson(string ccAnalis)
        {
            return JsonSuccess(CondicionesVentas(ccAnalis));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Listado(CotizacionFindViewModel model, int? page)
        {
            ViewBag.BaseUrl = Url.Content("~/");
            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    return JsonValidationError();
                }
                ViewBag.GetClientes = GetClientes();
                ViewBag.GetEstados = GetEstados();
                return View();
            }

            var pageNumber = (page ?? 1);
            try
            {
                var fechaInicio = (model.FechaInicio + " 00:00:00").ConvertDateTime();
                var fechaFinal = (model.FechaFinal + " 23:59:59").ConvertDateTime();
                var estadoPorEnviar = ((int)model.Estado).ToString();

                var result = CotizacionService.GetCotizacionesByClienteFecInicioFecFinal(model.Cliente, fechaInicio, fechaFinal, User.Identity.Name, estadoPorEnviar);
                //model.PagedListListaEntity = result.ToPagedList(pageNumber, PageSize);
                model.CotizacionModels = result;
                //var model = result;//.ToPagedList(pageNumber, PageSize);
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
            ViewBag.GetClientes = GetClientes();
            return View(model);
        }

        private string _cnProforma;
        private string _ccAnalis;
        private string _ccMoneda;
        private string _ccVta;
        private string _cFechaEmision;
        private string _cnSuc;
        private int _VisitaClienteID;
        private string _cnContacto;
        private decimal _fmTipCam;
        private int _ImprimirPrecioTN;
        private string _ccTipana;
        private string _cdRazsoc;
        private string _Tienda;
        private int _igv_bo;
        private int _zonaLiberada;
        private string _cnObservacion;

        private SessionTypes _sessionTypes = SessionTypes.NoAsignado;

        private const string SessionCcTipanaNuevo = "cc_tipana_nuevo";
        private const string SessionCcTipanaEditar = "cc_tipana_editar";
        private const string SessionCcAnalisNuevo = "cc_analis_nuevo";
        private const string SessionCcAnalisEditar = "cc_analis_editar";
        private const string SessionCdRazSocNuevo = "cd_razsoc_nuevo";
        private const string SessionCdRazSocEditar = "cd_razsoc_editar";
        private const string SessionCcMonedaNuevo = "cc_moneda_nuevo";
        private const string SessionCcMonedaEditar = "cc_moneda_editar";
        private const string SessionCcVtaNuevo = "cc_vta_nuevo";
        private const string SessionCcVtaEditar = "cc_vta_editar";
        private const string SessionCfechaEmisionNuevo = "fecha_emision_nuevo";
        private const string SessionCfechaEmisionEditar = "fecha_emision_editar";
        private const string SessionCnSucNuevo = "cn_suc_nuevo";
        private const string SessionVisitaClienteIDNuevo = "VisitaClienteID_nuevo";
        private const string SessionCnContactoNuevo = "cn_contacto_nuevo";
        private const string SessionCnSucEditar = "cn_suc_editar";
        private const string SessionVisitaClienteIDEditar = "VisitaClienteID_editar";
        private const string SessionCnContactoEditar = "cn_contacto_editar";
        private const string SessionFmTipCamNuevo = "fm_tipcam_nuevo";
        private const string SessionFmTipCamEditar = "fm_tipcam_editar";
        private const string SessionDetalleNuevo = "DetailNuevo";
        private const string SessionDetalleEditar = "DetailEditar";
        private const string SessionTiendaNuevo = "pedido_tienda_nuevo";
        private const string SessionTiendaEditar = "pedido_tienda_editar";
        private const string SessionIgv_boNuevo = "pedido_igv_bo_nuevo";
        private const string SessionIgv_boEditar = "pedido_igv_bo_editar";
        private const string SessionZonaLiberadaNuevo = "pedido_ZonaLiberada_nuevo";
        private const string SessionZonaLiberadaEditar = "pedido_ZonaLiberada_editar";
        private const string SessionImprimirPrecioTNNuevo = "pedido_imprimir_precio_TN_nuevo";
        private const string SessionImprimirPrecioTNEditar = "pedido_imprimir_precio_TN_editar";
        private const string SessionObservacionNuevo = "pedido_observacion_nuevo";
        private const string SessionObservacionEditar = "pedido_observacion_editar";

        private string _sessionCcTipana;
        private string _sessionCcAnalis;
        private string _sessionCdRazSoc;
        private string _sessionCcMoneda;
        private string _sessionCcVta;
        private string _sessionCfechaEmision;
        private string _sessionVisitaClienteID;
        private string _sessionCdAtencion;
        private string _sessionCnSuc;
        private string _sessionCnContacto;
        private string _sessionFmTipCam;
        private string _sessionImprimirPrecioTN;
        private string _sessionObservacion;
        private string _sessionDetalle;
        private string _sessionTienda;
        private string _sessionIgv_bo;
        private string _sessionZonaLiberada;

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Nuevo(int? reset)
        {


            if (reset.HasValue && reset.Value == 1)
            {
                Session.Clear();
            }

            ViewBag.BaseUrl = Url.Content("~/");
            _sessionTypes = SessionTypes.Nuevo;

            var saved = (bool?)TempData["Guardado"];
            var emailModel = (EnviarMailViewModel)TempData["EmailModel"];
            var cnProforma = (string)TempData["CnProforma"];

            if (saved.HasValue && saved.Value)
            {
                ViewBag.Success = "Cotización creada.";
            }

            if (emailModel != null)
            {
                ViewBag.EmailModel = emailModel;
            }

            if (!string.IsNullOrEmpty(cnProforma))
            {
                ViewBag.CnProforma = cnProforma;
            }

            SetDropDownList();
            ViewBag.GetGrupo = GetGrupo();
            ViewBag.GetSubGrupo = GetSubGrupo();
            var cCAnalis = Session[SessionCcAnalisNuevo];
            var cCMoneda = Session[SessionCcMonedaNuevo];
            var cCVta = Session[SessionCcVtaNuevo];
            var fechaEmision = (string)Session[SessionCfechaEmisionNuevo];
            var cnSuc = (string)Session[SessionCnSucNuevo];
            var VisitaClienteID = Session[SessionVisitaClienteIDNuevo];
            var cnContacto = (string)Session[SessionCnContactoNuevo];
            var CNTienda = (string)Session[SessionTiendaNuevo];
            var CNIgv_bo = "";
            try
            {
                CNIgv_bo = (string)Session[SessionIgv_boNuevo];
            }
            catch
            {
                CNIgv_bo = "";
            }
            var CNZonaLiberada = (string)Session[SessionZonaLiberadaNuevo];
            var CNimprimirPrecioTN = (string)Session[SessionImprimirPrecioTNNuevo];
            var CNobservacion = (string)Session[SessionObservacionNuevo];

            var vm = new CotizacionEditViewModel
            {
                cc_analis = (string)(cCAnalis),
                cc_moneda = (string)(cCMoneda),
                cc_vta = (string)(cCVta),
                CotizacionDetailViewModel = new CotizacionDetailViewModel(),
                ClienteNewViewModel = new ClienteNewViewModel(),
                EnviarMailViewModel = new EnviarMailViewModel(),
                Tienda = CNTienda,
                igv_bo = Convert.ToInt32(CNIgv_bo),
                zonaLiberada_bo = Convert.ToInt32(CNZonaLiberada),
                ContactoEntregaDirectaNewViewModel = new ContactoEntregaDirectaNewViewModel(),
                imprimirPrecioTN = Convert.ToInt32(CNimprimirPrecioTN),
                observacion = CNobservacion

            };
            if (!string.IsNullOrEmpty(fechaEmision) && !string.IsNullOrWhiteSpace(fechaEmision))
            {
                vm.FechaEmision = fechaEmision;
            }
            var dFechaEmision = (vm.FechaEmision).ConvertDateTime();
            vm.n_i_val_venta = TipoCambioDiarioService.GetByFechaTipoCambio(dFechaEmision);

            //if (vm.n_i_val_venta == 0.00)
            //{
            //    TempData["message"] = "No hay T.C registrado. No se pueden generar cotizaciones.";
            //    return RedirectToAction("Listado", "Cotizacion");
            //}

            return View(vm);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Nuevo(FormCollection formCollection)
        {
            Dictionary<string,string> diccionario = new Dictionary<string,string>();
            diccionario = formCollection.AllKeys.ToDictionary(k => k, v => formCollection[v]);

            ViewBag.BaseUrl = Url.Content("~/");
            _sessionTypes = SessionTypes.Nuevo;

            SetValueModel(formCollection);

            var model = new CotizacionEditViewModel
            {
                cn_proforma = _cnProforma,
                cc_tipana = _ccTipana,
                cc_analis = _ccAnalis,
                cd_razsoc = _cdRazsoc,
                cc_moneda = _ccMoneda,
                FechaEmision = _cFechaEmision,
                cn_suc = _cnSuc,
                VisitaClienteID = _VisitaClienteID,
                cn_contacto = _cnContacto,
                fm_tipcam = _fmTipCam,
                imprimirPrecioTN = Convert.ToInt32(_ImprimirPrecioTN),
                observacion = String.IsNullOrEmpty(_cnObservacion)? " " : _cnObservacion,
                cc_vta = _ccVta,
                Tienda = _Tienda,
                igv_bo = _igv_bo,
                zonaLiberada_bo = _zonaLiberada,
                ClienteNewViewModel = new ClienteNewViewModel(),
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
                    SetDropDownList();
                    ViewBag.GetGrupo = GetGrupo();
                    ViewBag.GetSubGrupo = GetSubGrupo();
                    model.EnviarMailViewModel = new EnviarMailViewModel();
                    return View(model);
                }
                var tempDetalle = Session[SessionDetalleNuevo];
                if (tempDetalle == null || ((List<CotizacionDetailViewModel>)(tempDetalle)).Count == 0)
                {
                    SetDropDownList();
                    ViewBag.GetGrupo = GetGrupo();
                    ViewBag.GetSubGrupo = GetSubGrupo();
                    model.EnviarMailViewModel = new EnviarMailViewModel();
                    const string messageValidation = "No existen artículos.";
                    ModelState.AddModelError(string.Empty, messageValidation);
                    ViewBag.MessageValidation = messageValidation;
                    return View(model);
                }

                var entityMaster = new LCPROF_WEB
                {
                    cn_proforma = model.cn_proforma,
                    cc_tipana = model.cc_tipana,
                    cc_analis = model.cc_analis,
                    cd_razsoc = model.cd_razsoc,
                    cc_moneda = model.cc_moneda,
                    cc_vta = model.cc_vta,
                    df_emision = model.FechaEmision.ConvertDateTime(),
                    fm_tipcam = model.fm_tipcam,
                    VisitaClienteID = model.VisitaClienteID
                };

                var detailCotizacionCast = (List<CotizacionDetailViewModel>)(tempDetalle);
                var item = 0;
                var cotizacionDetalleList = detailCotizacionCast.Select(detail => new LDPROF_WEB
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

                CotizacionService.CotizacionDetalleServices = cotizacionDetalleList;
                CotizacionService.Guardar(entityMaster, model.igv_bo, EmpresaSegunBD(), model.zonaLiberada_bo);
                CotizacionService.GuardarAdicional(entityMaster, User.Identity.Name, model.Tienda, model.igv_bo, model.cn_suc, model.cn_contacto, Convert.ToBoolean(model.imprimirPrecioTN), model.observacion, model.zonaLiberada_bo);
                SetSessionNull();
                TempData["Guardado"] = true;
                emailModel.Asunto = Funciones.Replace(emailModel.Asunto, "[Nro]", entityMaster.cn_proforma);
                TempData["EmailModel"] = emailModel;
                TempData["CnProforma"] = entityMaster.cn_proforma;
                return RedirectToAction<CotizacionController>(x => x.Nuevo(0));
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
                SetDropDownList();
                ViewBag.GetGrupo = GetGrupo();
                ViewBag.GetSubGrupo = GetSubGrupo();
                model.EnviarMailViewModel = new EnviarMailViewModel();
                return View(model);
            }
            catch (Exception ex)
            {
                var messageValidation = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ModelState.AddModelError(string.Empty, messageValidation);
                ViewBag.MessageValidation = messageValidation;
                SetDropDownList();
                ViewBag.GetGrupo = GetGrupo();
                ViewBag.GetSubGrupo = GetSubGrupo();
                model.EnviarMailViewModel = new EnviarMailViewModel();
                return View(model);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Editar(string id, int? page)
        {
            ViewBag.BaseUrl = Url.Content("~/");
            _sessionTypes = SessionTypes.Editar;

            var saved = (bool?)TempData["Guardado"];

            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction<CotizacionController>(x => x.Listado(page));
            }

            var cotizacion = CotizacionService.GetById(id);
            var cotizacionAdicional = CotizacionService.GetAdicionalById(id);


            if (cotizacion == null)
            {
                return RedirectToAction<CotizacionController>(x => x.Listado(page));
            }

            if (saved.HasValue && saved.Value)
            {
                ViewBag.Success = "Cotización actualizado.";
            }

            var vm = new CotizacionEditViewModel
            {
                cn_proforma = cotizacion.cn_proforma,
                cc_analis = cotizacion.cc_analis,
                cc_moneda = cotizacion.cc_moneda,
                cc_vta = cotizacion.cc_vta,
                FechaEmision = cotizacion.df_emision.ToString("dd/MM/yyyy"),
                cn_suc = cotizacionAdicional.cn_suc,
                cn_contacto = cotizacionAdicional.cn_contacto,
                fm_tipcam = cotizacion.fm_tipcam,
                n_i_val_venta = Convert.ToDouble(cotizacion.fm_tipcam),
                CotizacionDetailViewModel = new CotizacionDetailViewModel(),
                cb_estado = cotizacion.cb_estado,
                Tienda = cotizacionAdicional.cc_tienda,
                igv_bo = cotizacionAdicional.igv_bo,
                zonaLiberada_bo = cotizacionAdicional.zonaLiberada,
                EnviarMailViewModel = new EnviarMailViewModel(),
                ContactoEntregaDirectaNewViewModel = new ContactoEntregaDirectaNewViewModel(),
                imprimirPrecioTN = Convert.ToInt32(cotizacionAdicional.imprimirPrecioTN),
                observacion = cotizacionAdicional.observacion,
                VisitaClienteID = cotizacion.VisitaClienteID
            };

            var detalle = cotizacion.LDPROF_WEB;
            var detailCotizacion = (from item in detalle
                                    let articulo = ArticuloService.GetByCodigo(item.cc_artic)
                                    select new CotizacionDetailViewModel
                                    {
                                        cn_proforma = cotizacion.cn_proforma,
                                        cn_item = item.cn_item,
                                        cc_artic = item.cc_artic,
                                        cd_artic = articulo.cd_artic,
                                        cc_unmed = articulo.cc_unmed,
                                        fq_cantidad = item.fq_cantidad,
                                        fq_peso_teorico =(decimal) item.MARTICUL.fq_peso_teorico,
                                        fq_stock = item.fq_stock,
                                        cc_lista = item.cc_lista,
                                        fm_precio = item.fm_precio,
                                        fm_precio2 = item.fm_precio2,
                                        fm_precio_fin = item.fm_precio_fin,
                                        fm_total = item.fm_total,
                                        igv_bo = cotizacionAdicional.igv_bo,
                                        zonaLiberada_bo = cotizacionAdicional.zonaLiberada
                                    }).ToList();

            Session[SessionDetalleEditar] = detailCotizacion;
            SetDropDownList(cotizacion.cc_analis, cotizacionAdicional.cn_suc, cotizacion.df_emision.ToString("dd/MM/yyyy"));

            ViewBag.GetGrupo = GetGrupo();
            ViewBag.GetSubGrupo = GetSubGrupo();

            return View(vm);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(FormCollection formCollection, int? page)
        {
            ViewBag.BaseUrl = Url.Content("~/");
            _sessionTypes = SessionTypes.Editar;

            SetValueModel(formCollection);

            var model = new CotizacionEditViewModel
            {
                cn_proforma = _cnProforma,
                cc_tipana = _ccTipana,
                cc_analis = _ccAnalis,
                cd_razsoc = _cdRazsoc,
                cc_moneda = _ccMoneda,
                FechaEmision = _cFechaEmision,
                cn_suc = _cnSuc,
                VisitaClienteID = _VisitaClienteID,
                cn_contacto = _cnContacto,
                fm_tipcam = _fmTipCam,
                imprimirPrecioTN = Convert.ToInt32(_ImprimirPrecioTN),
                cc_vta = _ccVta,
                Tienda = _Tienda,
                igv_bo = _igv_bo,
                zonaLiberada_bo = _zonaLiberada,
                EnviarMailViewModel = new EnviarMailViewModel(),
                observacion = String.IsNullOrEmpty(_cnObservacion) ? " " : _cnObservacion,
            };

            try
            {
                var estadoCotizacion = CotizacionService.GetById(model.cn_proforma);
                var estadoConfirmado = ((int)EstadoCotizacionTypes.Compra_Total_Cerrada).ToString();
                var estadoRechazado = ((int)EstadoCotizacionTypes.Rechazado).ToString();
                var estadoAnulado = ((int)EstadoCotizacionTypes.Anulado).ToString();
                if (estadoCotizacion != null && estadoCotizacion.cb_estado == estadoConfirmado)
                {
                    ViewBag.Warning = "La cotización fue confirmada.";
                    SetDropDownList(model.cc_analis, model.cn_suc, model.df_emision.ToString("dd/MM/yyyy"));
                    ViewBag.GetGrupo = GetGrupo();
                    ViewBag.GetSubGrupo = GetSubGrupo();
                    return View(model);

                }
                if (estadoCotizacion != null && estadoCotizacion.cb_estado == estadoRechazado)
                {
                    ViewBag.Error = "La cotización fue rechazada.";
                    SetDropDownList(model.cc_analis, model.cn_suc, model.df_emision.ToString("dd/MM/yyyy"));
                    ViewBag.GetGrupo = GetGrupo();
                    ViewBag.GetSubGrupo = GetSubGrupo();
                    return View(model);
                }
                if (estadoCotizacion != null && estadoCotizacion.cb_estado == estadoAnulado)
                {
                    ViewBag.Error = "La cotización fue anulado.";
                    SetDropDownList(model.cc_analis, model.cn_suc, model.df_emision.ToString("dd/MM/yyyy"));
                    ViewBag.GetGrupo = GetGrupo();
                    ViewBag.GetSubGrupo = GetSubGrupo();
                    return View(model);
                }

                ModelState.Clear();
                TryUpdateModel(model);
                if (!ModelState.IsValid)
                {
                    SetDropDownList(model.cc_analis, model.cn_suc, model.df_emision.ToString("dd/MM/yyyy"));
                    ViewBag.GetGrupo = GetGrupo();
                    ViewBag.GetSubGrupo = GetSubGrupo();
                    return View(model);
                }
                var tempDetalle = Session[SessionDetalleEditar];
                var tempDetalleCast = ((tempDetalle) as List<CotizacionDetailViewModel>);
                if (tempDetalleCast == null || tempDetalleCast.Count == 0)
                {
                    SetDropDownList(model.cc_analis, model.cn_suc, model.df_emision.ToString("dd/MM/yyyy"));
                    const string messageValidation = "No existen artículos.";
                    ModelState.AddModelError(string.Empty, messageValidation);
                    ViewBag.MessageValidation = messageValidation;
                    return View(model);
                }

                var entityMaster = new LCPROF_WEB
                {
                    cn_proforma = model.cn_proforma,
                    cc_tipana = model.cc_tipana,
                    cc_analis = model.cc_analis,
                    cd_razsoc = model.cd_razsoc,
                    cc_moneda = model.cc_moneda,
                    cc_vta = model.cc_vta,
                    df_emision = model.FechaEmision.ConvertDateTime(),
                    fm_tipcam = model.fm_tipcam,
                    VisitaClienteID = model.VisitaClienteID
                };

                var detailCotizacionCast = (List<CotizacionDetailViewModel>)(tempDetalle);
                var item = 0;
                var cotizacionDetalleList = detailCotizacionCast.Select(detail => new LDPROF_WEB
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

                CotizacionService.CotizacionDetalleServices = cotizacionDetalleList;
                CotizacionService.Guardar(entityMaster, model.igv_bo, EmpresaSegunBD(), model.zonaLiberada_bo);
                CotizacionService.GuardarAdicional(entityMaster, User.Identity.Name, model.Tienda, model.igv_bo, model.cn_suc, model.cn_contacto, Convert.ToBoolean(model.imprimirPrecioTN),model.observacion, model.zonaLiberada_bo);
                SetSessionNull();
                TempData["Guardado"] = true;


                return RedirectToAction<CotizacionController>(x => x.Editar(model.cn_proforma, page));
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
                SetDropDownList(model.cc_analis, model.cn_suc, model.df_emision.ToString("dd/MM/yyyy"));
                ViewBag.GetGrupo = GetGrupo();
                ViewBag.GetSubGrupo = GetSubGrupo();


                return View(model);
            }
            catch (Exception ex)
            {
                var messageValidation = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ModelState.AddModelError(string.Empty, messageValidation);
                ViewBag.MessageValidation = messageValidation.Replace("\r\n", "");
                SetDropDownList(model.cc_analis, model.cn_suc, model.df_emision.ToString("dd/MM/yyyy"));
                ViewBag.GetGrupo = GetGrupo();
                ViewBag.GetSubGrupo = GetSubGrupo();
                return View(model);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult Rechazado(string id, int? page)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            {
                return Request.IsAjaxRequest() ? JsonError("parámetro id obligatorio.") : RedirectToAction<CotizacionController>(x => x.Listado(page));
            }

            try
            {
                var cotizacion = CotizacionService.GetById(id);

                if (cotizacion == null)
                {
                    return Request.IsAjaxRequest() ? JsonError("No existe cotización.") : RedirectToAction<CotizacionController>(x => x.Listado(page));
                }

                var estadoRechazado = ((int)EstadoCotizacionTypes.Rechazado).ToString();
                CotizacionService.UpdateEstado(id, estadoRechazado);
                //CotizacionService.AnularRestablecerProforma(id);

                if (!Request.IsAjaxRequest()) return View(id);
                const string success = "Cambio de estado a rechazado.";
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

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult Anulado(string id, int? page)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            {
                return Request.IsAjaxRequest() ? JsonError("parámetro id obligatorio.") : RedirectToAction<CotizacionController>(x => x.Listado(page));
            }
            try
            {
                var cotizacion = CotizacionService.GetById(id);

                if (cotizacion == null)
                {
                    return Request.IsAjaxRequest() ? JsonError("No existe cotización.") : RedirectToAction<CotizacionController>(x => x.Listado(page));
                }

                var estadoAnulado = ((int)EstadoCotizacionTypes.Anulado).ToString();
                string mensaje = CotizacionService.AnularRestablecerProforma(id);

                if (!Request.IsAjaxRequest()) return View(id);
                string success = mensaje; //const
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
        private const string PathFormatPdf = "~/Content/pdf/cotizacion-{0}.pdf";

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetEmailCliente(string id, string tipo, string Nro, string cn_contacto, string cn_suc)
        {
            int tipoMail = 1;
            //1 - Cotizacion
            //2 - Pedido u otros

            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            {
                return JsonError("No existe identificador.");
            }
            try
            {
                var cliente = ClienteService.GetEmailByCodigo(tipoMail, id, cn_contacto, cn_suc);
                var email = new
                {
                    asunto = "Cotización " + (string.IsNullOrEmpty(Nro) ? "[Nro]" : Nro) + " - " + cliente.cd_razsoc,
                    para = cliente.ct_email,
                    conCopia = User.Identity.Name
                };
                return cliente == null ? JsonError("Cliente seleccionado no existe.") : JsonSuccess(email);
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
            int tipoMail=1;
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
                ClienteService.ActualizarMailContacto(tipoMail, id, model.Para);


                RenderPdf(id, "RenderPdfCotizacion");
                var pdfPath = Server.MapPath(string.Format(PathFormatPdf, id));

                var cotizacion = CotizacionService.GetById(id);
                if (cotizacion != null)
                {
                    var estadoEnviado = ((int)EstadoCotizacionTypes.Pendiente_Respuesta).ToString();
                    CotizacionService.UpdateEstado(id, estadoEnviado);
                }
                string sRemitente = User.Identity.Name;
                var vendedor = VendedorService.GetByEmail(sRemitente);

                model.Mensaje = "[" + vendedor.ct_nombreCompleto + "]: " + model.Mensaje;
                sb.AppendLine(model.Mensaje);

                model.Asunto = Funciones.Replace(model.Asunto, "[Nro]", id);
                Mail.SendMail(sRemitente, vendedor.ct_nombreCompleto, model.Asunto, sb, model.Para, null, pdfPath,esHtml:true);//Cliente
                //Mail.SendMail(sRemitente, "Vendedor: " + vendedor.ct_nombreCompleto, model.Asunto, sb, Remite, null, pdfPath,esHtml:true);//BackOffice
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
            var cotizacion = CotizacionService.GetById(id);
            var cotizacionAdicional = CotizacionService.GetAdicionalById(id);
            var cotizacionClienteVm = new CotizacionClienteViewModel
            {
                Cotizacion = cotizacion,
                Adicional = cotizacionAdicional
            };

            var pdfOutput = ControllerContext.GeneratePdf(cotizacionClienteVm, viewName);
            var fullPath = Server.MapPath(string.Format(PathFormatPdf, cotizacion.cn_proforma));

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            System.IO.File.WriteAllBytes(fullPath, pdfOutput);
            return View(viewName);
        }
        public ActionResult Descargar(string cn_cotizacion)
        {
            RenderPdf(cn_cotizacion, "RenderPdfCotizacion");
            var pdfPath = Server.MapPath(string.Format(PathFormatPdf, cn_cotizacion));

            byte[] filedata = System.IO.File.ReadAllBytes(pdfPath);
            string contentType = System.Net.Mime.MediaTypeNames.Application.Octet;

            string filename = "Cotizacion Nro." + cn_cotizacion + ".pdf";
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
                    if (item.Key == 1) {
                        throw new Exception(item.Value);
                    }
                }
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
                    Value = x.cc_artic.Trim()
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
                if (subGrupo == "" || subGrupo== "Ninguno")
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

        #region SetValue
        private static string SetValue(string formValue, object sessionValue)
        {
            if (string.IsNullOrEmpty(formValue) || string.IsNullOrWhiteSpace(formValue))
            {
                try
                {
                    return (string)sessionValue;

                }
                catch 
                {
                    return sessionValue.ToString();
                }
            }
            return formValue;
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

            _cnProforma = formCollection["cn_proforma"];
            _ccAnalis = SetValue(formCollection["scc_analis"], Session[_sessionCcAnalis]);
            _ccMoneda = SetValue(formCollection["scc_moneda"], Session[_sessionCcMoneda]);
            _ccVta = SetValue(formCollection["scc_vta"], Session[_sessionCcVta]);
            _cFechaEmision = SetValue(formCollection["sfecha_emision"], Session[_sessionCfechaEmision]);
            _VisitaClienteID = Convert.ToInt32(SetValue(formCollection["sVisitaClienteID"], Session[_sessionVisitaClienteID]));
            _cnSuc = SetValue(formCollection["scn_suc"], Session[_sessionCnSuc]);
            _cnContacto = SetValue(formCollection["scn_contacto"], Session[_sessionCnContacto]);
            _fmTipCam = SetDecimal(formCollection["sfm_tipcam"], Session[_sessionFmTipCam]);
            _ImprimirPrecioTN = Convert.ToInt32(SetValue(formCollection["sImprimirPrecioTN"], Session[_sessionImprimirPrecioTN]));
            _Tienda = SetValue(formCollection["sTienda"], Session[_sessionTienda]);
            _igv_bo = Convert.ToInt32(SetValue(formCollection["sIgv_bo"], Session[_sessionIgv_bo]));
            _zonaLiberada = Convert.ToInt32(SetValue(formCollection["sZonaLiberada"], Session[_sessionZonaLiberada]));
            _cnObservacion = SetValue(formCollection["sObservacion"], Session[_sessionObservacion]);

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
            Session[_sessionCfechaEmision] = _cFechaEmision;
            Session[_sessionVisitaClienteID] = _VisitaClienteID;
            Session[_sessionTienda] = _Tienda;
            Session[_sessionIgv_bo] = _igv_bo;
            Session[_sessionZonaLiberada] = _zonaLiberada;
            Session[_sessionCnSuc] = _cnSuc;
            Session[_sessionCnContacto] = _cnContacto;
            Session[_sessionFmTipCam] = _fmTipCam;
            Session[_sessionImprimirPrecioTN] = _ImprimirPrecioTN;
            Session[_sessionObservacion] = _cnObservacion;
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
                    Session[SessionCfechaEmisionNuevo] = null;
                    Session[SessionVisitaClienteIDNuevo] = null;
                    Session[SessionCnSucNuevo] = null;
                    Session[SessionCnContactoNuevo] = null;
                    Session[SessionFmTipCamNuevo] = null;
                    Session[SessionTiendaNuevo] = null;
                    Session[SessionIgv_boNuevo] = null;
                    Session[SessionZonaLiberadaNuevo] = null;
                    Session[SessionDetalleNuevo] = null;
                    break;
                case SessionTypes.Editar:
                    Session[SessionCcTipanaEditar] = null;
                    Session[SessionCcAnalisEditar] = null;
                    Session[SessionCdRazSocEditar] = null;
                    Session[SessionCcMonedaEditar] = null;
                    Session[SessionCcVtaEditar] = null;
                    Session[SessionCfechaEmisionEditar] = null;
                    Session[SessionVisitaClienteIDEditar] = null;
                    Session[SessionCnSucEditar] = null;
                    Session[SessionCnContactoEditar] = null;
                    Session[SessionFmTipCamEditar] = null;
                    Session[SessionIgv_boEditar] = null;
                    Session[SessionZonaLiberadaEditar] = null;
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
                    _sessionCfechaEmision = SessionCfechaEmisionNuevo;
                    _sessionVisitaClienteID = SessionVisitaClienteIDNuevo;
                    _sessionCnSuc = SessionCnSucNuevo;
                    _sessionCnContacto = SessionCnContactoNuevo;
                    _sessionFmTipCam = SessionFmTipCamNuevo;
                    _sessionDetalle = SessionDetalleNuevo;
                    _sessionTienda = SessionTiendaNuevo;
                    _sessionIgv_bo = SessionIgv_boNuevo;
                    _sessionZonaLiberada = SessionZonaLiberadaNuevo;
                    break;
                case SessionTypes.Editar:
                    _sessionCcTipana = SessionCcTipanaEditar;
                    _sessionCcAnalis = SessionCcAnalisEditar;
                    _sessionCdRazSoc = SessionCdRazSocEditar;
                    _sessionCcMoneda = SessionCcMonedaEditar;
                    _sessionCcVta = SessionCcVtaEditar;
                    _sessionCfechaEmision = SessionCfechaEmisionEditar;
                    _sessionVisitaClienteID = SessionVisitaClienteIDEditar;
                    _sessionCnSuc = SessionCnSucEditar;
                    _sessionCnContacto = SessionCnContactoEditar;
                    _sessionFmTipCam = SessionFmTipCamEditar;
                    _sessionDetalle = SessionDetalleEditar;
                    _sessionTienda = SessionTiendaEditar;
                    _sessionIgv_bo = SessionIgv_boEditar;
                    _sessionZonaLiberada = SessionZonaLiberadaEditar;
                    break;
            }
        }
        #endregion

        #region SetDropDownList
        private void SetDropDownList(string idCliente = null, string cn_suc = null, string fechaEmision = null)
        {
            ViewBag.GetClientes = GetClientes(idCliente);
            ViewBag.GetMonedas = GetMonedas();
            ViewBag.GetSectores = Sectores();
            ViewBag.GetCondicionesVentas = CondicionesVentas(idCliente);
            ViewBag.GetArticulos = GetArticulos();
            ViewBag.GetTiendas = GetTiendas();
            ViewBag.GetIgv = GetIgv();
            ViewBag.GetMostrarIGV = GetMostrarIGV();
            ViewBag.GetZonaLiberada = GetZonaLiberada();
            ViewBag.GetImprimirPrecioTN = GetImprimirPrecioTN();
            ViewBag.GetSucursales = GetSucursalesCliente(idCliente);
            ViewBag.GetVisitasClientes = GetVisitasClientes(idCliente, fechaEmision);
            ViewBag.GetContactos = SelectContactoPorSucursal(cn_suc, idCliente);
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
                    new SelectListItem{
                        Text = "Todos",
                        Value = "0",
                    },
                    new SelectListItem
                    {
                        Text = EstadoCotizacionTypes.Por_Enviar.ToString().Replace("_"," "),
                        Value = EstadoCotizacionTypes.Por_Enviar.ToString(),
                    },
                    new SelectListItem
                    {
                        Text = EstadoCotizacionTypes.Pendiente_Respuesta.ToString().Replace("_"," "),
                        Value = EstadoCotizacionTypes.Pendiente_Respuesta.ToString(),
                    },
                    new SelectListItem
                    {
                        Text = EstadoCotizacionTypes.Compra_Parcial.ToString().Replace("_"," "),
                        Value = EstadoCotizacionTypes.Compra_Parcial.ToString(),
                    },

                    new SelectListItem
                    {
                        Text = "Compras Cerradas",
                        Value = "9",
                    },

                    //new SelectListItem
                    //{
                    //    Text = EstadoCotizacionTypes.Compra_Parcial_Cerrada.ToString().Replace("_"," "),
                    //    Value = EstadoCotizacionTypes.Compra_Parcial_Cerrada.ToString(),
                    //},
                    //new SelectListItem
                    //{
                    //    Text = EstadoCotizacionTypes.Compra_Total_Cerrada.ToString().Replace("_"," "),
                    //    Value = EstadoCotizacionTypes.Compra_Total_Cerrada.ToString(),
                    //},


                    new SelectListItem
                    {
                        Text = EstadoCotizacionTypes.Rechazado.ToString(),
                        Value = EstadoCotizacionTypes.Rechazado.ToString(),
                    },
                    new SelectListItem
                    {
                        Text = EstadoCotizacionTypes.Anulado.ToString(),
                        Value = EstadoCotizacionTypes.Anulado.ToString(),
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

        private SelectList GetVisitasClientes(string ccAnalis, string fechaEmision)
        {
            if (!string.IsNullOrWhiteSpace(ccAnalis) && !string.IsNullOrEmpty(ccAnalis))
            {
                //var lista = SucursalClienteService
                //.GetByCcAnalis(ccAnalis)
                //.ToSelectList(x => x.cn_suc.Trim() + " - " + x.cd_direc.Trim(), x => x.cn_suc,
                //    FindTypes.Ninguno.ToString());
                var lista = PlanificacionVisitasService
                    .getSelectVisitasClientes(ccAnalis, fechaEmision)
                    .ToSelectList(x => x.Descripcion, x=> x.VisitaClienteID.ToString());
                return NewSelectList(lista);
            }
            var listaVacia = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = FindTypes.Ninguno.ToString(),
                    Value = "0",
                    Selected = true
                }
            };
            return NewSelectList(listaVacia);
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
        private SelectList GetArticulos()
        {
            var lista = //ArticuloService.GetAllCodDes()
                (new List<ArticuloModel>())
                .ToSelectList(x => x.cd_artic.Trim(), x => x.cc_artic,
                    FindTypes.Ninguno.ToString());
            return NewSelectList(lista);
        }
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
            //return NewSelectList(lista);
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
        [AllowAnonymous]
        public JsonResult GetSubGrupos(string codGrupo)
        {
            var list = GetSubGrupo(codGrupo);
            return Json(list, JsonRequestBehavior.AllowGet);
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
            var tempDetalleCast = ((tempDetalle) as List<CotizacionDetailViewModel>);
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
            return PartialView("_PartialCotizacionDetail", tempDetalleCast);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult NuevoDetalle(CotizacionEditViewModel model, SessionTypes sessionType)
        {
            try
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
                    Session[_sessionTienda] = model.Tienda;
                    Session[_sessionIgv_bo] = model.igv_bo;
                    Session[_sessionZonaLiberada] = model.zonaLiberada_bo;
                }
                var ccArtic = model.CotizacionDetailViewModel.cc_artic.Trim();
                var cc_lista = model.CotizacionDetailViewModel.cc_lista;
                model.CotizacionDetailViewModel.cc_artic = ccArtic;
                if (!string.IsNullOrEmpty(ccArtic) && !string.IsNullOrWhiteSpace(ccArtic))
                {
                    var articulo = ArticuloService.GetByCodigo(ccArtic);
                    model.CotizacionDetailViewModel.cc_unmed = articulo.cc_unmed;

                    var listaPrecio = ListaPrecioService.GetCcListaByCcArtic(ccArtic);
                    model.CotizacionDetailViewModel.cc_lista = listaPrecio.cc_lista;

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
                        model.CotizacionDetailViewModel.fm_precio = precioLista;
                        model.CotizacionDetailViewModel.fm_precio2 = precioLista2;
                    }
                    var cantidad = model.CotizacionDetailViewModel.fq_cantidad;
                    var precioFinal = model.CotizacionDetailViewModel.fm_precio_fin;
                    model.CotizacionDetailViewModel.fm_total = cantidad * precioFinal;
                    model.CotizacionDetailViewModel.igv_bo = model.igv_bo;
                    model.CotizacionDetailViewModel.zonaLiberada_bo = model.zonaLiberada_bo;
                    
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
                //var pesoTeorico = model.CotizacionDetailViewModel.fq_peso_teorico;
                //model.CotizacionDetailViewModel.fq_peso_teorico = pesoTeorico * 1000;
                //

                //Valida que la cantidad sea mayor a 0
                if (model.CotizacionDetailViewModel.fq_cantidad <= (decimal)0.00)
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
                    model.CotizacionDetailViewModel.cn_item = ("1").PadLeft(2, '0');
                    var detailCotizacion = new List<CotizacionDetailViewModel> { model.CotizacionDetailViewModel };
                    Session[_sessionDetalle] = detailCotizacion;
                }
                else
                {
                    var detailCotizacionCast = (List<CotizacionDetailViewModel>)(tempDetalle);
                    //Actualiza el cnItem
                    int i = 0;
                    foreach (var detalle in detailCotizacionCast)
                    {
                        i++;
                        var secuencial = i.ToString("00");
                        detalle.cn_item = secuencial;
                    }
                    //Valida si el articulo ya ha sido agregado
                    var detail = detailCotizacionCast
                        .FirstOrDefault(x => x.cc_artic.Trim() == model.CotizacionDetailViewModel.cc_artic.Trim());
                    if (detail != null)
                    {
                        var messageArticuloExiste = "Ya ha agregado el artículo: '" + model.CotizacionDetailViewModel.cd_artic.Trim().ToUpper();
                        if (Request.IsAjaxRequest())
                        {
                            return JsonError(messageArticuloExiste);
                        }
                    }

                    var ultimoDetalle = detailCotizacionCast.LastOrDefault();
                    var cnItem = ultimoDetalle != null ? ultimoDetalle.cn_item : "0";
                    var nCnItem = Convert.ToInt32(cnItem);
                    model.CotizacionDetailViewModel.cn_item = (nCnItem + 1).ToString().PadLeft(2, '0');

                    detailCotizacionCast.Add(model.CotizacionDetailViewModel);
                    Session[_sessionDetalle] = detailCotizacionCast;
                }
                return PartialView("_PartialCotizacionDetail", Session[_sessionDetalle]);
            }
            catch (Exception e)
            {
                return JsonError(e.ToString());
            }
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
                return PartialView("_PartialCotizacionDetail", null);
            }
            var detailCotizacionCast = (List<CotizacionDetailViewModel>)(tempDetalle);
            var detail = detailCotizacionCast.FirstOrDefault(x => x.cc_artic.Trim() == id);
            if (detail != null)
            {
                detailCotizacionCast.Remove(detail);
            }
            Session[_sessionDetalle] = detailCotizacionCast;
            return PartialView("_PartialCotizacionDetail", Session[_sessionDetalle]);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult EditarAjax(FormCollection formCollection, int? page)
        {
            _sessionTypes = SessionTypes.Editar;

            SetValueModel(formCollection);

            var model = new CotizacionEditViewModel
            {
                cn_proforma = _cnProforma,
                cc_tipana = _ccTipana,
                cc_analis = _ccAnalis,
                cd_razsoc = _cdRazsoc,
                cc_moneda = _ccMoneda,
                FechaEmision = _cFechaEmision,
                cn_suc = _cnSuc,
                VisitaClienteID = _VisitaClienteID,
                cn_contacto = _cnContacto,
                fm_tipcam = _fmTipCam,
                imprimirPrecioTN = Convert.ToInt32(_ImprimirPrecioTN),
                observacion = String.IsNullOrEmpty(_cnObservacion) ? " " : _cnObservacion,
                cc_vta = _ccVta,
                Tienda = _Tienda,
                igv_bo = _igv_bo,
                zonaLiberada_bo = _zonaLiberada,
                EnviarMailViewModel = new EnviarMailViewModel(),
            };

            try
            {
                var estadoCotizacion = CotizacionService.GetById(model.cn_proforma);
                var estadoConfirmado = ((int)EstadoCotizacionTypes.Compra_Total_Cerrada).ToString();
                var estadoRechazado = ((int)EstadoCotizacionTypes.Rechazado).ToString();
                var estadoAnulado = ((int)EstadoCotizacionTypes.Anulado).ToString();
                if (estadoCotizacion != null && estadoCotizacion.cb_estado == estadoConfirmado)
                {
                    return JsonError("La cotización fue confirmada.");
                }
                if (estadoCotizacion != null && estadoCotizacion.cb_estado == estadoRechazado)
                {
                    return JsonError("La cotización fue rechazada.");
                }
                if (estadoCotizacion != null && estadoCotizacion.cb_estado == estadoAnulado)
                {
                    return JsonError("La cotización fue anulado.");
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
                var tempDetalle = Session[SessionDetalleEditar];
                var tempDetalleCast = ((tempDetalle) as List<CotizacionDetailViewModel>);
                if (tempDetalleCast == null || tempDetalleCast.Count == 0)
                {
                    SetDropDownList(model.cc_analis,fechaEmision: model.df_emision.ToString("dd/MM/yyyy"));
                    const string messageValidation = "No existen artículos.";
                    ModelState.AddModelError(string.Empty, messageValidation);
                    ViewBag.MessageValidation = messageValidation;
                    return View(model);
                }

                var entityMaster = new LCPROF_WEB
                {
                    cn_proforma = model.cn_proforma,
                    cc_tipana = model.cc_tipana,
                    cc_analis = model.cc_analis,
                    cd_razsoc = model.cd_razsoc,
                    cc_moneda = model.cc_moneda,//DESCOMENTAR
                    cc_vta = model.cc_vta,
                    df_emision = model.FechaEmision.ConvertDateTime(),
                    fm_tipcam = model.fm_tipcam,
                    VisitaClienteID = model.VisitaClienteID
                };

                var detailCotizacionCast = (List<CotizacionDetailViewModel>)(tempDetalle);
                var item = 0;
                var cotizacionDetalleList = detailCotizacionCast.Select(detail => new LDPROF_WEB
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

                CotizacionService.CotizacionDetalleServices = cotizacionDetalleList;
                CotizacionService.Guardar(entityMaster, model.igv_bo, EmpresaSegunBD(), model.zonaLiberada_bo);
                CotizacionService.GuardarAdicional(entityMaster, User.Identity.Name, model.Tienda, model.igv_bo, model.cn_suc, model.cn_contacto,Convert.ToBoolean(model.imprimirPrecioTN),model.observacion,model.zonaLiberada_bo);

                TempData["Guardado"] = true;


                return JsonSuccess(1);
            }
            catch (DbEntityValidationException e)
            {
                return JsonError(e.ToString());
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult ObtenerGrupoSubgrupoArticulo(string idArticulo)
        {
            List<ArticuloModel.ArticuloGS> lista = new List<ArticuloModel.ArticuloGS>();
            lista = ArticuloService.GrupoSubgrupoSegunArtic(idArticulo);
            return JsonSuccess(lista);
        }
        private SelectList GetTiendas()
        {
            var lista = TiendaRepository.getTiendas().ToSelectList(x => x.descri.Trim(), x => x.codigo);
            return NewSelectList(lista);
        }
        //public ActionResult TiendaSegunVendedor()
        //{
        //    var lista = TiendaService.GetTiendaSegunVendedor(User.Identity.Name);
        //    return JsonSuccess(lista);
        //}
        //private SelectList GetIgv()
        //{
        //    var listaVacia = new List<SelectListItem>();
        //    var sqlDB = new System.Data.SqlClient.SqlConnectionStringBuilder(
        //    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //    if (sqlDB.InitialCatalog == "ZICO_ERP04")
        //    {
        //        listaVacia = new List<SelectListItem> {
        //            new SelectListItem
        //            {
        //                Text = "Sin IGV",
        //                Value = "0",
        //            }
        //        };
        //    }
        //    else
        //    {
        //        listaVacia = new List<SelectListItem> {
        //            new SelectListItem {
        //                Text = "Sin IGV",
        //                Value = "0",
        //            },
        //            new SelectListItem {
        //                Text = "Con IGV",
        //                Value = "1"
        //            }
        //        };
        //    }


        //    return NewSelectList(listaVacia);
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

        private SelectList GetImprimirPrecioTN()
        {
            var listaVacia = new List<SelectListItem>();

                listaVacia = new List<SelectListItem> {
                    new SelectListItem {
                        Text = "No",
                        Value = "0",
                    },
                    new SelectListItem {
                        Text = "Si",
                        Value = "1"
                    }
                };
            return NewSelectList(listaVacia);
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

        public ActionResult ListaMotivosRechazoCotizacion()
        {
            List<Entity.CotizacionMotivoRechazo> lista = new List<CotizacionMotivoRechazo>();
            lista = CotizacionService.ListaMotivosRechazoCotizacion();
            return JsonSuccess(lista);
        }

        public ActionResult RegistarRechazoCotizacion(string cn_proforma, int idMotivo, string mensajeRechazo)
        {
            cn_proforma = cn_proforma.Replace("C", "");
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            diccionario = CotizacionService.RegistrarRechazoCotizacion(cn_proforma, idMotivo ,mensajeRechazo);
            return JsonSuccess(diccionario);
        }

        public ActionResult RegistarCierreCotizacionParcial(string cn_proforma, int idMotivo, string mensajeRechazo)
        {
            cn_proforma = cn_proforma.Replace("C", "");
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            diccionario = CotizacionService.RegistrarCierreCotizacionParcial(cn_proforma, idMotivo, mensajeRechazo);
            return JsonSuccess(diccionario);
        }

        public ActionResult StockTodasTiendasPorArticulo(string idArticulo)
        {
            var listaStockArticulos = ArticuloService.ObtenerStockTodasTiendasPorArticulo(idArticulo);
            return JsonSuccess(listaStockArticulos);
        }
    }
}