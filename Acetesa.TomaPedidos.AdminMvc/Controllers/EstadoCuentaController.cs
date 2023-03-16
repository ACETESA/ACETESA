using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Acetesa.TomaPedidos.AdminMvc.Helpers;
using Acetesa.TomaPedidos.AdminMvc.Infrastructure;
using Acetesa.TomaPedidos.AdminMvc.Models;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Transversal;
using Acetesa.TomaPedidos.Transversal.Enums;
using Acetesa.TomaPedidos.Transversal.Extensions;
using MvcRazorToPdf;

namespace Acetesa.TomaPedidos.AdminMvc.Controllers
{
    [Authorize]
    public class EstadoCuentaController : BaseController
    {
        private const string PathFormatPdf = "~/Content/pdf/Estado de cuenta - @.pdf";
        public IVendedorService VendedorService { get; set; }

        // GET: EstadoCuenta

        public IClienteService ClienteService { get; set; }
        public IEstadoCuentaService EstadoCuentaService { get; set; }

        private SelectList GetFamilias()
        {
            var lista = //ClienteService.GetAll()
                 (new List<ClienteModel>())
                .ToSelectList(x => x.cd_razsoc.Trim(), x => x.cc_analis,
                    FindTypes.Ninguno.ToString());
            return NewSelectList(lista);
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetClientes(string codFamilia)
        {
            var list = (new List<ClienteModel>());
            //ClienteService.GetAll();
            return Json(list, JsonRequestBehavior.AllowGet);

            //IList<ClienteModel> listaModel = new List<ClienteModel>();
            //listaModel = ClienteService.SelectClientesSegunCarteraVendedorYLibres(User.Identity.Name);

            //var selectList = listaModel.ToSelectList(x => x.cd_razsoc.Trim(), x => x.cc_analis,
            //         FindTypes.Ninguno.ToString());

            //return Json(selectList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EstadoCuentaPorCliente()
        {
            var model = new EstadoCuentaViewModel
            {
                EnviarMailViewModel = new EnviarMailViewModel()
            };
            ViewBag.BaseUrl = Url.Content("~/");
            var task = Task.Run(() => GetFamilias());
            //var task = await Task.Run(() => GetClientes());
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EstadoCuentaPorCliente(EstadoCuentaViewModel model)
        {
            string id = model.Cliente;

            var Resumen = EstadoCuentaService.GetResumenByRuc(id);
            var Detalle = EstadoCuentaService.GetDetalleByRuc(id);

            model.EstadoCuentaResumenModel = Resumen;
            model.EstadoCuentaDetalleModels = Detalle.ToList();
            model.EnviarMailViewModel = new EnviarMailViewModel();

            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialEstadoCuentaInformacion", model);
            }

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public async Task<ActionResult> Index()
        {
            var model = new EstadoCuentaViewModel
            {
                EnviarMailViewModel = new EnviarMailViewModel()
            };
            ViewBag.BaseUrl = Url.Content("~/");
            var task = await Task.Run(() => GetFamilias());
            //var task = await Task.Run(() => GetClientes());
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(EstadoCuentaViewModel model, string tipoEc)
        {
            ViewBag.BaseUrl = Url.Content("~/");
            if (!ModelState.IsValid)
            {
                ViewBag.GetClientes = GetFamilias();
                return View();
            }

            if (string.IsNullOrEmpty(tipoEc))
            {
                ModelState.AddModelError(string.Empty,"Debe indicar Detalle o Resumen.");
                ViewBag.GetClientes = GetFamilias();
                return View();
            }

            switch (tipoEc)
            {
                //case "Detalle":
                //    return RedirectToAction<EstadoCuentaController>(c => c.Detallado(model.Cliente));
                case "Resumen":
                    return RedirectToAction<EstadoCuentaController>(c => c.Resumen(model.Cliente));
                default:
                    ViewBag.GetClientes = GetFamilias();
                    return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public async Task<ActionResult> Resumen(string id)
        {
            //Inicio: Resumen
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction<EstadoCuentaController>(c => c.Index());
            }
            var model = new EstadoCuentaResumenViewModel
            {
                EstadoCuentaDetalleViewModel = new EstadoCuentaDetalleViewModel()

            };

            model.EstadoCuentaResumenModel = await Task.Run(() => EstadoCuentaService.GetResumenByRuc(id));
            //Fin: Resumen
            //Inicio: Detalle
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction<EstadoCuentaController>(c => c.Index());
            }
            model.EstadoCuentaDetalleViewModel.EstadoCuentaDetalleModels = await Task.Run(() => EstadoCuentaService.GetDetalleByRuc(id));
            //Fin: Detalle

            return View(model);
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
        public ActionResult RenderPdf(string id, string viewName)
        {
            //Inicio: Resumen
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction<EstadoCuentaController>(c => c.Index());
            }
            var rutaArchivo = PathFormatPdf.Replace("@", id);
            var model = new EstadoCuentaResumenViewModel
            {
                EstadoCuentaDetalleViewModel = new EstadoCuentaDetalleViewModel()
            };
            model.EstadoCuentaResumenModel =  EstadoCuentaService.GetResumenByRuc(id);
            //Fin: Resumen
            //Inicio: Detalle
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction<EstadoCuentaController>(c => c.Index());
            }
            model.EstadoCuentaDetalleViewModel.EstadoCuentaDetalleModels = EstadoCuentaService.GetDetalleByRuc(id);
            //Fin: Detalle
            var pdfOutput = ControllerContext.GeneratePdf(model, viewName);
            var fullPath = Server.MapPath(string.Format(rutaArchivo, model.EstadoCuentaResumenModel.Ruc));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            System.IO.File.WriteAllBytes(fullPath, pdfOutput);
            return View(viewName);
        }

        public FileContentResult descargar(string id, string razSoc)
        {
            var rutaArchivo = PathFormatPdf.Replace("@", id);
            byte[] fileBytes = null;
            string mimetype = "application/pdf";
            RenderPdf(id, "RenderPdfEstadoCuenta");
            var pdfPath = Server.MapPath(string.Format(rutaArchivo, id));
            fileBytes = System.IO.File.ReadAllBytes(pdfPath);
            string fechaHoy = DateTime.Now.ToString("ddMMMyy").Replace(".","");
            string nombreArchivo = "("+fechaHoy+")E.C - "+ razSoc +"(" + id + ").pdf";
            return File(fileBytes, mimetype, nombreArchivo);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetEmailCliente(string id)
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
                var cliente = ClienteService.GetEmailByCodigo(tipoMail, id,"","");
                var hoy = DateTime.Now.ToString("dd/MM/yyyy");
                var email = new
                {
                    asunto = cliente.cd_razsoc.Trim() + " - Envío de estado de cuenta al " + hoy,
                    para = cliente.ct_email,
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
        private static readonly string Remite = ConfigurationManager.AppSettings["mail_remitente"];
        private static readonly string Label = ConfigurationManager.AppSettings["mail_remitente_label"];
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult EnviarMail(EnviarMailViewModel model, string idCliente)
        {
            var rutaArchivo = PathFormatPdf.Replace("@", idCliente);
            byte[] fileBytes = null;
            string mimetype = "application/pdf";
            RenderPdf(idCliente, "RenderPdfEstadoCuenta");
            var pdfPath = Server.MapPath(string.Format(rutaArchivo, idCliente));

            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }

            var sb = new StringBuilder();
            sb.AppendLine(model.Mensaje);
            try
            {
                string sCredencialesDocVenta = ConfigurationManager.AppSettings["CREDENCIALES_DOC_VENTA"];
                if (!(string.IsNullOrEmpty(sCredencialesDocVenta)))
                {
                    var lsCredencialesDocVenta = sCredencialesDocVenta.Split(',');

                    //NetworkCredential theNetworkCredential = new NetworkCredential(lsCredencialesDocVenta[2], lsCredencialesDocVenta[3]);
                    //CredentialCache theNetCache = new CredentialCache();
                    //theNetCache.Add(new Uri(rutaArchivo), "Basic", theNetworkCredential);

                    //string[] theFiles = Directory.GetFiles(pdfPath);
                    //var userName = string.IsNullOrEmpty(lsCredencialesDocVenta[1]) ? lsCredencialesDocVenta[2] : string.Format(@"{0}\{1}", lsCredencialesDocVenta[1], lsCredencialesDocVenta[2]);
                    //var net = NetworkShareAccesser.Access(lsCredencialesDocVenta[0], lsCredencialesDocVenta[1], lsCredencialesDocVenta[2], lsCredencialesDocVenta[3]);

                    

                    using (NetworkShareAccesser.Access(lsCredencialesDocVenta[0], lsCredencialesDocVenta[1], lsCredencialesDocVenta[2], lsCredencialesDocVenta[3]))
                    {
                        var sqlDB = new System.Data.SqlClient.SqlConnectionStringBuilder(
                         ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                        string ruc_remitente;
                        if (sqlDB.InitialCatalog == "ZICO_ERP04")
                        {
                            ruc_remitente = "20309525532";
                        }
                        else
                        {
                            ruc_remitente = "20265733515";
                        }


                        //ClienteService.UpdateEmailByCodigo(idCliente, model.Para);
                        string sRemitente = User.Identity.Name;
                        var vendedor = VendedorService.GetByEmail(sRemitente);
                        var credentials = VendedorService.ObtenerCredencialesCorreoVendedor(User.Identity.Name);
                        //model.Asunto = Funciones.Replace(model.Asunto, "[Nro]", id);
                        Mail.SendMail(credentials.correo, credentials.clave, credentials.llave, sRemitente, vendedor.ct_nombreCompleto, model.Asunto, sb, model.Para, null, pdfPath, esHtml: true);//Cliente
                        Mail.SendMail(credentials.correo, credentials.clave, credentials.llave, sRemitente, "Vendedor: " + vendedor.ct_nombreCompleto, model.Asunto, sb, Remite, null, pdfPath, esHtml: true);//BackOffice
                        if (!string.IsNullOrEmpty(model.ConCopia))
                        {
                            Mail.SendMail(credentials.correo, credentials.clave, credentials.llave, sRemitente, Label, model.Asunto, sb, model.ConCopia, null, pdfPath, esHtml: true);//Vendedor
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var exception = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return JsonError(exception);
            }
            if (System.IO.File.Exists(rutaArchivo))
            {
                System.IO.File.Delete(rutaArchivo);
            }
            return JsonSuccess(1);
        }

    }
}