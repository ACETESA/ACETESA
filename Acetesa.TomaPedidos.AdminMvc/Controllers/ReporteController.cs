using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Acetesa.TomaPedidos.AdminMvc.Infrastructure;
using Acetesa.TomaPedidos.AdminMvc.Models;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Transversal.Enums;
using Acetesa.TomaPedidos.Transversal.Extensions;

namespace Acetesa.TomaPedidos.AdminMvc.Controllers
{
    [Authorize]
    public class ReporteController : BaseController
    {
        public IProductoService ProductoService { get; set; }

        // GET: Reporte
        public ActionResult Index()
        {
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "admin")]
        public ActionResult EstadisticaVentas()
        {
            ViewBag.GetEmpresas = GetEmpresas();
            var viewModel = new EstadisticaVentasViewModel();
            return View(viewModel);
        }
        
        [AcceptVerbs(HttpVerbs.Post)]

        [HttpPost]
        public ActionResult getDatosReporte(string sFechaInicio, string sFechaFinal, string sVendedor, string sEmpresa, string sTienda)
        {
            ViewBag.GetEmpresas = GetEmpresas();
            try
            {
                var fechaInicio = (sFechaInicio + " 00:00:00").ConvertDateTime();
                var fechaFinal = (sFechaFinal + " 23:59:59").ConvertDateTime();
                
                var oResultado = ProductoService.GetEstadisticaVentas(fechaInicio, fechaFinal, sVendedor, sEmpresa, sTienda);
                return JsonSuccess(oResultado);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        #region SetDropDownList
        private SelectList GetEmpresas()
        {
            var lista = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = EmpresaTypes.Acetesa.ToString(),
                        Value = "1",
                    },
                    new SelectListItem
                    {
                        Text = EmpresaTypes.Galpesa.ToString(),
                        Value = "2",
                    }
                };
            return NewSelectList(lista);
        }
        #endregion
    }
}