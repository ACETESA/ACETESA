using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using Acetesa.TomaPedidos.AdminMvc.Infrastructure;
using Acetesa.TomaPedidos.AdminMvc.Models;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Transversal.Enums;
using Acetesa.TomaPedidos.Transversal.Extensions;
using Acetesa.TomaPedidos.Repository;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Acetesa.TomaPedidos.Entity;

namespace Acetesa.TomaPedidos.AdminMvc.Controllers
{
    [Authorize]
    public class ProductoController : BaseController
    {
        // GET: Producto

        public IProductoService ProductoService { get; set; }


        public ActionResult Index()
        {
            return View();
        }
        private SelectList GetListaPrecios()
        {
            var lista = ProductoService.GetListaPreciosSp().ToSelectList(x => x.cd_lista, x => x.cc_lista,
                    FindTypes.Ninguno.ToString());
            return NewSelectList(lista);
        }
        private SelectList GetListaPreciosByParam(string empresa)
        {
            var lista = ProductoService.ListarPreciosArticulosPorEmpresa(empresa).ToSelectList(x => x.cd_lista, x => x.cc_lista,FindTypes.Ninguno.ToString());
            return NewSelectList(lista);
        }
        private SelectList GetFamilias()
        {
            var lista = ProductoService.GetFamiliasSp().ToSelectList(x => x.cd_gruart, x => x.cc_gruartec,
                    FindTypes.Ninguno.ToString());
            return NewSelectList(lista);
        }
        private SelectList GetFamiliasByParam(string empresa)
        {
            //List<Tgruartec> listaGrupos = new List<Tgruartec>();
            var listaGrupos = ProductoService.ListarFamiliasArticulosPorEmpresa(empresa).ToSelectList(x => x.cd_gruart, x => x.cc_gruartec, FindTypes.Ninguno.ToString());
            return NewSelectList(listaGrupos);
        }
        private SelectList GetSubFamiliasByCodFamilia(string empresa = null, string codFamilia = null)
        {
            if (string.IsNullOrEmpty(codFamilia) || string.IsNullOrEmpty(empresa))
            {
                return NewSelectList(null);
            }
            //var lista = ProductoService.GetSubFamiliasByCodFamiliaSp(codFamilia)
            //    .ToSelectList(x => x.cd_subgruart, x => x.cc_subgruart,
            //        FindTypes.Ninguno.ToString());
            var lista = ProductoService.ListarSubFamiliasArticulosPorEmpresa(empresa, codFamilia)
                .ToSelectList(X => X.cd_subgruart, X => X.cc_subgruart, FindTypes.Ninguno.ToString());


            return NewSelectList(lista);
        }
        private SelectList GetSubFamiliasByParam(string empresa, string codFamilia = null)
        {
            if (string.IsNullOrEmpty(codFamilia))
            {
                return NewSelectList(null);
            }
            var lista = ProductoService.ListarSubFamiliasArticulosPorEmpresa(empresa,codFamilia);
            return NewSelectList(lista);
        }

        private SelectList GetTipos()
        {
            var lista = ProductoService.GetTipoProductoSp().ToSelectList(x => x.cd_tipoartic, x => x.cc_tipoartic,
                    FindTypes.Ninguno.ToString());
            return NewSelectList(lista);
        }

        private SelectList GetTiposByParam(string empresa)
        {
            var lista = ProductoService.ListarTipoArticulosPorEmpresa(empresa).ToSelectList(x => x.cd_tipoartic, x => x.cc_tipoartic,FindTypes.Ninguno.ToString());
            return NewSelectList(lista);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetSubFamilias(string empresa, string codFamilia)
        {
            var list = GetSubFamiliasByCodFamilia(empresa, codFamilia);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult Stock()
        //{
        //    ViewBag.GetFamilias = GetFamilias();
        //    ViewBag.GetSubFamilias = GetSubFamiliasByCodFamilia();
        //    ViewBag.GetTipos = GetTipos();
        //    var viewModel = new StockViewModel();
        //    return View(viewModel);
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Stock(StockViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        if (Request.IsAjaxRequest())
        //        {
        //            return JsonValidationError();
        //        }
        //        ViewBag.GetFamilias = GetFamilias();
        //        ViewBag.GetSubFamilias = GetSubFamiliasByCodFamilia(model.SubFamilia);
        //        ViewBag.GetTipos = GetTipos();
        //        return View();
        //    }

        //    //IEnumerable<StockModel> result;
        //    Dictionary<string, object> oResultado;
        //    try
        //    {
        //        //jlazaro
        //        oResultado = ProductoService.GetStockProductosStoreProcesure(DateTimeExtension.GetDateTimeNow(), model.Familia, model.SubFamilia, model.Tipo);
        //        //result = ProductoService.GetStockProductosSp(DateTimeExtension.GetDateTimeNow(), model.Familia, model.SubFamilia, model.Tipo);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (Request.IsAjaxRequest())
        //        {
        //            return JsonError(ex.Message);
        //        }
        //        throw;
        //    }
        //    //jlazaro
        //    var viewModel = new StockViewModel
        //    {
        //        DatosSP = oResultado["datos"] as List<Dictionary<string, object>>,
        //        CabecerasSP = oResultado["cabeceras"] as List<string>
        //    };
        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("_PartialStockList", viewModel);
        //    }
        //    oResultado = null;
        //    ViewBag.GetFamilias = GetFamilias();
        //    ViewBag.GetSubFamilias = GetSubFamiliasByCodFamilia(model.SubFamilia);
        //    ViewBag.GetTipos = GetTipos();
        //    return View(viewModel);
        //}

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Precio_Acetesa()
        {
            ViewBag.GetListaPrecios = GetListaPreciosByParam("acetesa");
            ViewBag.GetFamilias = GetFamiliasByParam("acetesa");
            ViewBag.GetSubFamilias = GetSubFamiliasByParam("acetesa");
            ViewBag.GetStocks = GetStocks();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Precio_Galpesa()
        {
            ViewBag.GetListaPrecios = GetListaPreciosByParam("galpesa");
            ViewBag.GetFamilias = GetFamiliasByParam("galpesa");
            ViewBag.GetSubFamilias = GetSubFamiliasByParam("galpesa");
            ViewBag.GetStocks = GetStocks();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Precio_Acetesa(PrecioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    return JsonValidationError();
                }
                ViewBag.GetListaPrecios = GetListaPreciosByParam("acetesa");
                ViewBag.GetFamilias = GetFamiliasByParam("acetesa");
                ViewBag.GetSubFamilias = GetSubFamiliasByParam("acetesa",model.SubFamilia);
                ViewBag.GetStocks = GetStocks();
                return View(model);
            }

            IEnumerable<PrecioModel> result;
            try
            {
                //result = ProductoService.GetPreciosProductosSp(model.ListaPrecios, model.Familia, model.SubFamilia, model.Stocks);
                result = ProductoService.ListarPreciosArticulosPorGrupoEmpresa(model.ListaPrecios, model.Familia,model.SubFamilia, model.Stocks,"acetesa");
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return JsonError(ex.Message);
                }
                throw;
            }
            //Busqueda sin resultados
            int filas = result.Count();
            if (filas == 0)
            {
                return new EmptyResult();
            }

            var precioModels = result as IList<PrecioModel> ?? result.ToList();
            var viewModel = new PrecioViewModel
            {
                TipoCambio = !precioModels.Any() ? 0 : precioModels.FirstOrDefault().ni_tipcam,
                PrecioModels = precioModels
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialPrecioList_Acetesa", viewModel);
            }
            ViewBag.GetListaPrecios = GetListaPreciosByParam("acetesa");
            ViewBag.GetFamilias = GetFamiliasByParam("acetesa");
            ViewBag.GetSubFamilias = GetSubFamiliasByParam("acetesa",model.SubFamilia);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Precio_Galpesa(PrecioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    return JsonValidationError();
                }
                ViewBag.GetListaPrecios = GetListaPreciosByParam("galpesa");
                ViewBag.GetFamilias = GetFamiliasByParam("galpesa");
                ViewBag.GetSubFamilias = GetSubFamiliasByParam("galpesa", model.SubFamilia);
                ViewBag.GetStocks = GetStocks();
                return View(model);
            }

            IEnumerable<PrecioModel> result;
            try
            {
                //result = ProductoService.GetPreciosProductosSp(model.ListaPrecios, model.Familia, model.SubFamilia, model.Stocks);
                result = ProductoService.ListarPreciosArticulosPorGrupoEmpresa(model.ListaPrecios, model.Familia, model.SubFamilia, model.Stocks, "galpesa");
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return JsonError(ex.Message);
                }
                throw;
            }
            //Busqueda sin resultados
            int filas = result.Count();
            if (filas == 0)
            {
                return new EmptyResult();
            }

            var precioModels = result as IList<PrecioModel> ?? result.ToList();
            var viewModel = new PrecioViewModel
            {
                TipoCambio = !precioModels.Any() ? 0 : precioModels.FirstOrDefault().ni_tipcam,
                PrecioModels = precioModels
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialPrecioList_Galpesa", viewModel);
            }
            ViewBag.GetListaPrecios = GetListaPreciosByParam("galpesa");
            ViewBag.GetFamilias = GetFamiliasByParam("galpesa");
            ViewBag.GetSubFamilias = GetSubFamiliasByParam("galpesa", model.SubFamilia);
            return View(viewModel);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult StockPorGrupo_Acetesa()
        {


            ViewBag.GetEmpresas = GetEmpresasByParam("acetesa");
            ViewBag.GetTiendas = GetTiendasByParam("acetesa");
            ViewBag.GetFamilias = GetFamiliasByParam("acetesa");
            ViewBag.GetSubFamilias = GetSubFamiliasByParam("acetesa");
            ViewBag.GetTipos = GetTiposByParam("acetesa");
            var viewModel = new StockPorGrupoViewModel();
            viewModel.Tipo = "A";
            return View(viewModel);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult StockPorGrupo_Galpesa()
        {
            ViewBag.GetEmpresas = GetEmpresasByParam("galpesa");
            ViewBag.GetTiendas = GetTiendasByParam("galpesa");
            ViewBag.GetFamilias = GetFamiliasByParam("galpesa");
            ViewBag.GetSubFamilias = GetSubFamiliasByParam("galpesa");
            ViewBag.GetTipos = GetTiposByParam("galpesa");
            var viewModel = new StockPorGrupoViewModel();
            viewModel.Tipo = "A";
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public FileContentResult descargar(StockPorGrupoViewModel model)
        {
            var fecha = (model.FechaInicio + " 23:59:59").ConvertDateTime();
            var empresa = model.Empresa.ToString();

            if (empresa == "Acetesa")
            {
                empresa = "1";
            }
            else
            {
                empresa = "2";
            }
            var result = ProductoService.GetStockPorGrupoProductoSp(empresa, model.Tienda, model.Familia, model.SubGrupo, model.Tipo, fecha).ToList();

            byte[] fileBytes = GeneraExcel(result);


            string mimetype = "application/vnd.ms-excel";
            return File(fileBytes, mimetype, "Stock.xlsx");
        }

        private byte[] GeneraExcel(List<StockPorGrupoModel> result)
        {
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("hoja1");
            GenerarCabeceraExcel(ref worksheet);
            int nFila = 3;
            foreach (var grupo in result.GroupBy(x => x.Art_Grupo))
            {
                var rango = worksheet.Cells["A" + nFila.ToString() + ":G" + nFila.ToString()];
                FormatearCeldaExcel(ref rango, grupo.Key);
                rango.Merge = true;

                rango.Style.Fill.BackgroundColor.SetColor(1, 139, 206, 132);
                nFila++;
                foreach (var subGroup in grupo.GroupBy(x => x.Art_SubGrupo))
                {
                    rango = worksheet.Cells["A" + nFila.ToString() + ":G" + nFila.ToString()];
                    FormatearCeldaExcel(ref rango, subGroup.Key);
                    rango.Merge = true;

                    rango.Style.Fill.BackgroundColor.SetColor(1, 236, 255, 231);
                    nFila++;
                    foreach (var group2 in subGroup.GroupBy(Y => new { Y.cd_desc, Y.fq_embalaje, Y.fq_unidades, Y.fq_peso, Y.tot_un, Y.tot_tn }))
                    {
                        rango = worksheet.Cells["A" + nFila.ToString()];
                        FormatearCeldaExcel(ref rango, group2.Key.cd_desc);
                        rango.Style.Fill.BackgroundColor.SetColor(Color.White);

                        rango = worksheet.Cells["B" + nFila.ToString()];
                        FormatearCeldaExcel(ref rango, group2.Key.fq_embalaje.ToString("#0.00"));
                        rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rango.Style.Fill.BackgroundColor.SetColor(Color.White);

                        rango = worksheet.Cells["C" + nFila.ToString()];
                        FormatearCeldaExcel(ref rango, group2.Key.fq_unidades.ToString("#,##0.00"));
                        rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rango.Style.Fill.BackgroundColor.SetColor(Color.White);

                        rango = worksheet.Cells["D" + nFila.ToString()];
                        FormatearCeldaExcel(ref rango, group2.Key.fq_peso.ToString("#,##0.00"));
                        rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rango.Style.Fill.BackgroundColor.SetColor(Color.White);

                        rango = worksheet.Cells["E" + nFila.ToString()];
                        FormatearCeldaExcel(ref rango, group2.Key.tot_un.ToString("#,##0.00"));
                        rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rango.Style.Fill.BackgroundColor.SetColor(Color.White);

                        rango = worksheet.Cells["F" + nFila.ToString()];
                        FormatearCeldaExcel(ref rango, group2.Sum(x => x.ReservadoUN).ToString("#,##0.00"));
                        rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rango.Style.Fill.BackgroundColor.SetColor(Color.White);

                        rango = worksheet.Cells["G" + nFila.ToString()];
                        FormatearCeldaExcel(ref rango, ((group2.Key.tot_un - group2.Sum(x => x.ReservadoUN)).ToString("#,##0.00")));
                        rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        rango.Style.Fill.BackgroundColor.SetColor(Color.White);

                        nFila++;
                    }
                }
            }
            worksheet.Column(1).AutoFit();
            worksheet.Column(2).AutoFit();
            worksheet.Column(3).AutoFit();
            worksheet.Column(4).AutoFit();
            worksheet.Column(5).AutoFit();
            worksheet.Column(6).AutoFit();
            worksheet.Column(7).AutoFit();

            return package.GetAsByteArray();
        }

        private void GenerarCabeceraExcel(ref ExcelWorksheet worksheet)
        {
            var rango = worksheet.Cells["A1:A2"];
            FormatearCeldaExcelCabecera(ref rango, "Descripción");

            rango = worksheet.Cells["B1:B2"];
            FormatearCeldaExcelCabecera(ref rango, "PU");

            rango = worksheet.Cells["C1:D1"];
            FormatearCeldaExcelCabecera(ref rango, "Stk Real");

            rango = worksheet.Cells["E1:G1"];
            FormatearCeldaExcelCabecera(ref rango, "Stk En UN");

            rango = worksheet.Cells["C2"];
            FormatearCeldaExcelCabecera(ref rango, "UN");

            rango = worksheet.Cells["D2"];
            FormatearCeldaExcelCabecera(ref rango, "TN");

            rango = worksheet.Cells["E2"];
            FormatearCeldaExcelCabecera(ref rango, "Total");

            rango = worksheet.Cells["F2"];
            FormatearCeldaExcelCabecera(ref rango, "Reser");

            rango = worksheet.Cells["G2"];
            FormatearCeldaExcelCabecera(ref rango, "Dispo");

        }

        private void FormatearCeldaExcelCabecera(ref ExcelRange rango, string sValor)
        {
            FormatearCeldaExcel(ref rango, sValor, true);
            rango.Merge = true;
            rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rango.Style.Fill.BackgroundColor.SetColor(0, 76, 175, 80);
            rango.Style.Font.Color.SetColor(Color.White);
        }

        private void FormatearCeldaExcel(ref ExcelRange rango, string sValor, bool bold = false)
        {
            rango.Value = sValor;
            rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rango.Style.Font.Bold = bold;
        }

        //Generar excel de precio y descarga
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public FileContentResult descargarExcelPrecio(PrecioViewModel model, string empresa)
        {
            //var result = ProductoService.GetPreciosProductosSp(model.ListaPrecios, model.Familia, model.SubFamilia, model.Stocks).ToList();
            var result = ProductoService.ListarPreciosArticulosPorGrupoEmpresa(model.ListaPrecios, model.Familia, model.SubFamilia, model.Stocks, empresa);
            byte[] fileBytes = GeneraExcelPrecio(result);
            string fecha = DateTime.UtcNow.ToString("ddMMMyyyy");

            string mimetype = "application/vnd.ms-excel";
            return File(fileBytes, mimetype, "Lista precios(" + fecha + ").xlsx");
        }

        private byte[] GeneraExcelPrecio(List<PrecioModel> result)
        {
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("hoja1");

            var rango = worksheet.Cells["F1"];
            FormatearCeldaExcelCabeceraPrecio(ref rango, "T.C:");

            rango = worksheet.Cells["G1"];
            FormatearCeldaExcelCabeceraPrecio(ref rango, result.First().ni_tipcam.ToString());

            rango = worksheet.Cells["H1"];
            FormatearCeldaExcelCabeceraPrecio(ref rango, "Fecha:");

            string fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            rango = worksheet.Cells["I1:J1"];
            FormatearCeldaExcelCabeceraPrecio(ref rango, fecha);

            GenerarCabeceraExcelPrecio(ref worksheet);
            int nFila = 4;
            foreach (var r in result)
            {
                rango = worksheet.Cells["A" + nFila.ToString()];
                FormatearCeldaExcelPrecio(ref rango, r.cc_artic);
                rango.Style.Fill.BackgroundColor.SetColor(Color.White);

                rango = worksheet.Cells["B" + nFila.ToString()];
                FormatearCeldaExcelPrecio(ref rango, r.cd_artic);
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rango.Style.Fill.BackgroundColor.SetColor(Color.White);

                rango = worksheet.Cells["C" + nFila.ToString()];
                FormatearCeldaExcelPrecio(ref rango, r.cc_unmed);
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.BackgroundColor.SetColor(Color.White);

                rango = worksheet.Cells["D" + nFila.ToString()];
                FormatearCeldaExcelPrecio(ref rango, r.cd_moneda);
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.BackgroundColor.SetColor(Color.White);

                rango = worksheet.Cells["E" + nFila.ToString()];
                FormatearCeldaExcelPrecio(ref rango, r.fq_peso_teorico);
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                rango.Style.Fill.BackgroundColor.SetColor(Color.White);

                rango = worksheet.Cells["F" + nFila.ToString()];
                FormatearCeldaExcelPrecio(ref rango, r.fq_unid_tm);
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                rango.Style.Fill.BackgroundColor.SetColor(Color.White);

                rango = worksheet.Cells["G" + nFila.ToString()];
                FormatearCeldaExcelPrecio(ref rango, r.fm_valorvta_tn);
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                rango.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(1, 254, 254, 219));

                rango = worksheet.Cells["H" + nFila.ToString()];
                FormatearCeldaExcelPrecio(ref rango, r.fm_valorunit);
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                rango.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(1, 254, 254, 219));

                rango = worksheet.Cells["I" + nFila.ToString()];
                FormatearCeldaExcelPrecio(ref rango, r.fm_valorvta_tn2);
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                rango.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(1, 239, 255, 221));

                rango = worksheet.Cells["J" + nFila.ToString()];
                FormatearCeldaExcelPrecio(ref rango, r.fm_valorunit2);
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                rango.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(1, 239, 255, 221));

                rango = worksheet.Cells["K" + nFila.ToString()];
                FormatearCeldaExcelPrecio(ref rango, r.ArticStk.ToString());
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                rango.Style.Fill.BackgroundColor.SetColor(Color.White);

                nFila++;
            }

            worksheet.Column(1).AutoFit();
            worksheet.Column(2).AutoFit();
            worksheet.Column(3).AutoFit();
            worksheet.Column(4).AutoFit();
            worksheet.Column(5).AutoFit();
            worksheet.Column(6).AutoFit();
            worksheet.Column(7).AutoFit();
            worksheet.Column(8).AutoFit();

            return package.GetAsByteArray();
        }

        private void GenerarCabeceraExcelPrecio(ref ExcelWorksheet worksheet)
        {
            var rango = worksheet.Cells["A2:A3"];
            FormatearCeldaExcelCabeceraPrecio(ref rango, "Código");

            rango = worksheet.Cells["B2:B3"];
            FormatearCeldaExcelCabeceraPrecio(ref rango, "Descripción");

            rango = worksheet.Cells["C2:C3"];
            FormatearCeldaExcelCabeceraPrecio(ref rango, "U.M.");

            rango = worksheet.Cells["D2:D3"];
            FormatearCeldaExcelCabeceraPrecio(ref rango, "Mon");

            rango = worksheet.Cells["E2:E3"];
            FormatearCeldaExcelCabeceraPrecio(ref rango, "Peso Unit");

            rango = worksheet.Cells["F2:F3"];
            FormatearCeldaExcelCabeceraPrecio(ref rango, "Unit x TN");

            rango = worksheet.Cells["G2:H2"];
            FormatearCeldaExcelCabeceraPrecio(ref rango, "Precio 1");

            rango = worksheet.Cells["I2:J2"];
            FormatearCeldaExcelCabeceraPrecio(ref rango, "Precio 2");

            rango = worksheet.Cells["G3"];
            FormatearCeldaExcelCabeceraPrecio(ref rango, "TN");

            rango = worksheet.Cells["H3"];
            FormatearCeldaExcelCabeceraPrecio(ref rango, "Unidad");

            rango = worksheet.Cells["I3"];
            FormatearCeldaExcelCabeceraPrecio(ref rango, "TN");

            rango = worksheet.Cells["J3"];
            FormatearCeldaExcelCabeceraPrecio(ref rango, "Unidad");

            rango = worksheet.Cells["K2:K3"];
            FormatearCeldaExcelCabeceraPrecio(ref rango, "Stk Disp.");
        }

        private void FormatearCeldaExcelCabeceraPrecio(ref ExcelRange rango, string sValor)
        {
            FormatearCeldaExcelPrecio(ref rango, sValor, true);
            rango.Merge = true;
            rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rango.Style.Fill.BackgroundColor.SetColor(0, 76, 175, 80);
            rango.Style.Font.Color.SetColor(Color.White);
        }

        private void FormatearCeldaExcelPrecio(ref ExcelRange rango, string sValor, bool bold = false)
        {
            rango.Value = sValor;
            rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rango.Style.Font.Bold = bold;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StockPorGrupo_Acetesa(StockPorGrupoViewModel model)
        {

            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    return JsonValidationError();
                }
                ViewBag.GetEmpresas = GetEmpresasByParam("acetesa");
                ViewBag.GetTiendas = GetTiendasByParam("acetesa");
                ViewBag.GetFamilias = GetFamiliasByParam("acetesa");
                ViewBag.GetSubFamilias = GetSubFamiliasByParam("acetesa",model.SubGrupo);
                ViewBag.GetTipos = GetTiposByParam("acetesa");
                return View();
            }

            IEnumerable<StockPorGrupoModel> result;
            try
            {
                var fecha = (model.FechaInicio + " 23:59:59").ConvertDateTime();
                var empresa = model.Empresa.ToString();


                //if (empresa == "Acetesa")
                //{
                //    empresa = "1";
                //}
                //else
                //{
                //    empresa = "2";
                //}
                empresa = "1"; // Acetesa
                result = ProductoService.GetStockPorGrupoProductoSp(empresa, model.Tienda, model.Familia, model.SubGrupo, model.Tipo, fecha);
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return JsonError(ex.Message);
                }
                throw;
            }
            var stockPorGrupoModels = result as IList<StockPorGrupoModel> ?? result.ToList();
            var viewModel = new StockPorGrupoViewModel
            {

                StockPorGrupoModels = stockPorGrupoModels
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialStockPorGrupoList", viewModel);
            }
            result = null;
            ViewBag.GetEmpresas = GetEmpresasByParam("acetesa");
            ViewBag.GetTiendas = GetTiendasByParam("acetesa");
            ViewBag.GetFamilias = GetFamiliasByParam("acetesa");
            ViewBag.GetSubFamilias = GetSubFamiliasByParam("acetesa", model.SubGrupo);
            ViewBag.GetTipos = GetTiposByParam("acetesa");
            return View(viewModel);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StockPorGrupo_Galpesa(StockPorGrupoViewModel model)
        {

            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    return JsonValidationError();
                }
                ViewBag.GetEmpresas = GetEmpresasByParam("galpesa");
                ViewBag.GetTiendas = GetTiendasByParam("galpesa");
                ViewBag.GetFamilias = GetFamiliasByParam("galpesa");
                ViewBag.GetSubFamilias = GetSubFamiliasByParam("galpesa", model.SubGrupo);
                ViewBag.GetTipos = GetTiposByParam("galpesa");
                return View();
            }

            IEnumerable<StockPorGrupoModel> result;
            try
            {
                var fecha = (model.FechaInicio + " 23:59:59").ConvertDateTime();
                var empresa = model.Empresa.ToString();


                //if (empresa == "Acetesa")
                //{
                //    empresa = "1";
                //}
                //else
                //{
                //    empresa = "2";
                //}
                empresa = "2"; //galpesa
                result = ProductoService.GetStockPorGrupoProductoSp(empresa, model.Tienda, model.Familia, model.SubGrupo, model.Tipo, fecha);
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return JsonError(ex.Message);
                }
                throw;
            }
            var stockPorGrupoModels = result as IList<StockPorGrupoModel> ?? result.ToList();
            var viewModel = new StockPorGrupoViewModel
            {

                StockPorGrupoModels = stockPorGrupoModels
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialStockPorGrupoList", viewModel);
            }
            result = null;
            ViewBag.GetEmpresas = GetEmpresasByParam("galpesa");
            ViewBag.GetTiendas = GetTiendasByParam("galpesa");
            ViewBag.GetFamilias = GetFamiliasByParam("galpesa");
            ViewBag.GetSubFamilias = GetSubFamiliasByParam("galpesa", model.SubGrupo);
            ViewBag.GetTipos = GetTiposByParam("galpesa");
            return View(viewModel);
        }

        #region SetDropDownList
        private SelectList GetEmpresas()
        {
            var codUsuario = User.Identity.Name;
            if (codUsuario.Contains("acetesa"))
            {
                var lista = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = EmpresaTypes.Acetesa.ToString(),
                        Value = "1",
                    }
                };
                return NewSelectList(lista);
            }
            else
            {
                var lista = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = EmpresaTypes.Galpesa.ToString(),
                        Value = "2",
                    }
                };
                return NewSelectList(lista);
            }

            //var lista = new List<SelectListItem>
            //    {
            //        new SelectListItem
            //        {
            //            Text = EmpresaTypes.Acetesa.ToString(),
            //            Value = "1",
            //        },
            //        new SelectListItem
            //        {
            //            Text = EmpresaTypes.Galpesa.ToString(),
            //            Value = "2",
            //        }
            //    };

        }

        private SelectList GetEmpresasByParam(string empresa)
        {
            if (empresa.Contains("acetesa"))
            {
                var lista = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = EmpresaTypes.Acetesa.ToString(),
                        Value = "1",
                    }
                };
                return NewSelectList(lista);
            }
            else
            {
                var lista = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = EmpresaTypes.Galpesa.ToString(),
                        Value = "2",
                    }
                };
                return NewSelectList(lista);
            }
        }

        private SelectList GetStocks()
        {
            var lista = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "Con Stock",
                        Value = "1",
                    },
                    new SelectListItem
                    {
                        Text = "Sin Stock",
                        Value = "0",
                    },
                   new SelectListItem
                    {
                        Text = "Todos",
                        Value = "2",
                    },
                };

            return NewSelectList(lista);

        }

        private SelectList GetTiendas()
        {
            var lista = TiendaRepository.getTiendas().ToSelectList(x => x.descri.Trim(), x => x.codigo);
            return NewSelectList(lista);
        }

        private SelectList GetTiendasByParam(string empresa)
        {
            var lista = TiendaRepository.getTiendasByParam(empresa).ToSelectList(x => x.descri.Trim(), x => x.codigo);
            return NewSelectList(lista);
        }
        #endregion

    }
}