using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Mvc;
using Acetesa.TomaPedidos.AdminMvc.Infrastructure;
using Acetesa.TomaPedidos.AdminMvc.Models;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.Transversal;
using Acetesa.TomaPedidos.Transversal.Enums;
using Acetesa.TomaPedidos.Transversal.Extensions;
using MvcRazorToPdf;
using PagedList;
using Acetesa.TomaPedidos.AdminMvc.Helpers;
using System.Net.Mail;
using System.Net.Mime;

namespace Acetesa.TomaPedidos.AdminMvc.Controllers
{
    [Authorize]
    public class VentaController : BaseController
    {
        public IVentaService VentaService { get; set; }
        public IClienteService ClienteService { get; set; }
        public IPedidoService PedidoService { get; set; }
        public IVendedorService VendedorService { get; set; }


        // GET: Venta
        public ActionResult Index()
        {
            return View();
        }

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

        public ActionResult ValidarVendedorCliente(string cc_analis)
        {
            var lista = ClienteService.ValidarRelacionVendedorCliente(cc_analis, User.Identity.Name);
            return JsonSuccess(lista);
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

        #region SetDropDownList
        private void SetDropDownList(string ccAnalis)
        {
            ViewBag.GetClientes = GetClientes(ccAnalis);
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
        #endregion



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Venta()
        {
            ViewBag.BaseUrl = Url.Content("~/");
            ViewBag.GetClientes = GetClientes();
            var viewModel = new VentaViewModel()
            {
                EnviarMailViewModel = new EnviarMailViewModel()
            };
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Venta(VentaViewModel model)
        {
            ViewBag.BaseUrl = Url.Content("~/");
            ViewBag.GetClientes = GetClientes();
            if (!ModelState.IsValid)
            {
                if (Request.IsAjaxRequest())
                {
                    return JsonValidationError();
                }
                return View();
            }

            Dictionary<string, object> oResultado;
            try
            {
                var fechaInicio = (model.FechaInicio + " 00:00:00").ConvertDateTime();
                var fechaFinal = (model.FechaFinal + " 23:59:59").ConvertDateTime();
                oResultado = VentaService.GetVentaStoreProcedure(fechaInicio, fechaFinal, model.Cliente, User.Identity.Name);
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return JsonError(ex.Message);
                }
                throw;
            }

            var viewModel = new VentaViewModel
            {
                DatosSP = oResultado["datos"] as List<Dictionary<string, object>>,
                CabecerasSP = oResultado["cabeceras"] as List<string>,
                EnviarMailViewModel = new EnviarMailViewModel()

            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialVentaList", viewModel);
            }
            oResultado = null;
            return View(viewModel);
        }

        //public FileContentResult descargar(string documento, string path)
        //{

        //    //var sDatosNro = documento.Split('-');
        //    //string NombreArchivo = "";
        //    //switch (sDatosNro[0])
        //    //{
        //    //    case "FA":
        //    //        NombreArchivo = ConfigurationManager.AppSettings["FORMATO_FILE_FACTURA"];
        //    //        break;
        //    //    case "BO":
        //    //        NombreArchivo = ConfigurationManager.AppSettings["FORMATO_FILE_BOLETA"];
        //    //        break;
        //    //    case "NC":
        //    //        NombreArchivo = ConfigurationManager.AppSettings["FORMATO_FILE_NOTA_CREDITO"];
        //    //        break;
        //    //    case "ND":
        //    //        NombreArchivo = ConfigurationManager.AppSettings["FORMATO_FILE_NOTA_DEBITO"];
        //    //        break;
        //    //}
        //    //NombreArchivo += sDatosNro[1].Substring(0, 3) + "-" + sDatosNro[1].Substring(3);
        //    //NombreArchivo += ".pdf";
        //    //string mimetype = "application/pdf";
        //    //path = path.Replace("@", @"\");
        //    //string sCredencialesDocVenta = ConfigurationManager.AppSettings["CREDENCIALES_DOC_VENTA"];
        //    //byte[] fileBytes = null;
        //    //if (string.IsNullOrEmpty(sCredencialesDocVenta))
        //    //{
        //    //    fileBytes = System.IO.File.ReadAllBytes(path);
        //    //}
        //    //else 
        //    //{
        //    //    var lsCredencialesDocVenta = sCredencialesDocVenta.Split(',');
        //    //    using (NetworkShareAccesser.Access(lsCredencialesDocVenta[0], lsCredencialesDocVenta[1], lsCredencialesDocVenta[2], lsCredencialesDocVenta[3]))
        //    //    {
        //    //        fileBytes = System.IO.File.ReadAllBytes(path);
        //    //    }
        //    //}
        //    //return File(fileBytes, mimetype, NombreArchivo);
        //}
        public FileContentResult descargarPDF(string documento)
        {
            byte[] byteDocumento = new byte[9000];
            string mimetype = "application/pdf";
            string nombreDocumento = "";

            try
            {
                string empresaEmisora = EmpresaSegunBD();

                string tipoDocumentoRelacionado = "";
                var sDatosNro = documento.Split('-');
                switch (sDatosNro[0])
                {
                    case "FA":
                        tipoDocumentoRelacionado = "01";
                        break;
                    case "BO":
                        tipoDocumentoRelacionado = "03";
                        break;
                    case "NC":
                        tipoDocumentoRelacionado = "07";
                        break;
                    case "ND":
                        tipoDocumentoRelacionado = "08";
                        break;
                }

                string serieDocumento = sDatosNro[1].Substring(0, 3);
                string correlativoDocumento = sDatosNro[1].Substring(3, 8);

                DocumentosModel documentosModel = new DocumentosModel();
                documentosModel.empresaEmisora = empresaEmisora == "ACETESA" ? "20265733515" : "20309525532";
                documentosModel.idTipoDocumento = 1;
                documentosModel.tipoDocRelacionado = tipoDocumentoRelacionado;
                documentosModel.serieDocRelacionado = serieDocumento;
                documentosModel.correlativoDocRelacionado = correlativoDocumento;
                var diccionarioDocumento = VentaService.RecuperarDocumentosPorComprobante(documentosModel);
                nombreDocumento = diccionarioDocumento.Keys.First();
                byteDocumento = diccionarioDocumento.Values.First();



            }
            catch (Exception e)
            {
                Console.Error.Write(e.Message);
            }

            return File(byteDocumento, mimetype, nombreDocumento);
        }

        public FileContentResult descargarZIP(string documento)
        {
            byte[] byteDocumento = new byte[9000];
            string mimetype = "application/zip";
            string nombreDocumento = "";

            try
            {
                string empresaEmisora = EmpresaSegunBD();

                string tipoDocumentoRelacionado = "";
                var sDatosNro = documento.Split('-');
                switch (sDatosNro[0])
                {
                    case "FA":
                        tipoDocumentoRelacionado = "01";
                        break;
                    case "BO":
                        tipoDocumentoRelacionado = "03";
                        break;
                    case "NC":
                        tipoDocumentoRelacionado = "07";
                        break;
                    case "ND":
                        tipoDocumentoRelacionado = "08";
                        break;
                }

                string serieDocumento = sDatosNro[1].Substring(0, 3);
                string correlativoDocumento = sDatosNro[1].Substring(3, 8);

                DocumentosModel documentosModel = new DocumentosModel();
                documentosModel.empresaEmisora = empresaEmisora == "ACETESA" ? "20265733515" : "20309525532";
                documentosModel.idTipoDocumento = 2;
                documentosModel.tipoDocRelacionado = tipoDocumentoRelacionado;
                documentosModel.serieDocRelacionado = serieDocumento;
                documentosModel.correlativoDocRelacionado = correlativoDocumento;
                var diccionarioDocumento = VentaService.RecuperarDocumentosPorComprobante(documentosModel);
                nombreDocumento = diccionarioDocumento.Keys.First();
                byteDocumento = diccionarioDocumento.Values.First();

            }
            catch (Exception e)
            {
                Console.Error.Write(e.Message);
            }

            return File(byteDocumento, mimetype, nombreDocumento);
        }
        public FileContentResult descargarXML(string documento)
        {
            byte[] byteDocumento = new byte[9000];
            string mimetype = "application/xml";
            string nombreDocumento = "";

            try
            {
                string empresaEmisora = EmpresaSegunBD();

                string tipoDocumentoRelacionado = "";
                var sDatosNro = documento.Split('-');
                switch (sDatosNro[0])
                {
                    case "FA":
                        tipoDocumentoRelacionado = "01";
                        break;
                    case "BO":
                        tipoDocumentoRelacionado = "03";
                        break;
                    case "NC":
                        tipoDocumentoRelacionado = "07";
                        break;
                    case "ND":
                        tipoDocumentoRelacionado = "08";
                        break;
                }

                string serieDocumento = sDatosNro[1].Substring(0, 3);
                string correlativoDocumento = sDatosNro[1].Substring(3, 8);

                DocumentosModel documentosModel = new DocumentosModel();
                documentosModel.empresaEmisora = empresaEmisora == "ACETESA" ? "20265733515" : "20309525532";
                documentosModel.idTipoDocumento = 3;
                documentosModel.tipoDocRelacionado = tipoDocumentoRelacionado;
                documentosModel.serieDocRelacionado = serieDocumento;
                documentosModel.correlativoDocRelacionado = correlativoDocumento;
                var diccionarioDocumento = VentaService.RecuperarDocumentosPorComprobante(documentosModel);
                nombreDocumento = diccionarioDocumento.Keys.First();
                byteDocumento = diccionarioDocumento.Values.First();

            }
            catch (Exception e)
            {
                Console.Error.Write(e.Message);
            }

            return File(byteDocumento, mimetype, nombreDocumento);
        }



        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult GetEmailCliente(string id, string Nro)
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
                    asunto = cliente.cd_razsoc.Trim() + " - Envío de documento: " + Nro,
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
        private const string PathFormatPdf = "~/Content/pdf/documento-{0}.pdf";
        private static readonly string Remite = ConfigurationManager.AppSettings["mail_remitente"];
        private static readonly string Label = ConfigurationManager.AppSettings["mail_remitente_label"];
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult EnviarMail(EnviarMailViewModel model, string id, string idCliente, string rutaPDF)
        {
            //rutaPDF = rutaPDF.Replace("@", @"\");
            //string rutaXml = rutaPDF.Replace(".pdf", ".xml");
            //string rutaZip = rutaPDF.Replace(".pdf", ".zip");

            byte[] PDF = null;
            byte[] CDR = null;
            byte[] XML = null;

            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }
            if (rutaPDF == ("PDF-"))
            {
                PDF = obtenerBytePDF(id);
            }
            if (rutaPDF == ("ZIP-"))
            {
                CDR = obtenerByteCDR(id);
            }
            if (rutaPDF == ("XML"))
            {
                XML = obtenerByteXML(id);
            }
            if (rutaPDF == ("PDF-ZIP-"))
            {
                PDF = obtenerBytePDF(id);
                CDR = obtenerByteCDR(id);
            }
            if (rutaPDF == ("PDF-XML"))
            {
                PDF = obtenerBytePDF(id);
                XML = obtenerByteXML(id);
            }
            if (rutaPDF == ("ZIP-XML"))
            {
                CDR = obtenerByteCDR(id);
                XML = obtenerByteXML(id);
            }
            if (rutaPDF == ("PDF-ZIP-XML"))
            {
                PDF = obtenerBytePDF(id);
                CDR = obtenerByteCDR(id);
                XML = obtenerByteXML(id);
            }


            var sb = new StringBuilder();
            sb.AppendLine(model.Mensaje);
            try
            {



                //ClienteService.UpdateEmailByCodigo(idCliente, model.Para);
                string sRemitente = User.Identity.Name;
                var vendedor = VendedorService.GetByEmail(sRemitente);
                model.Asunto = Funciones.Replace(model.Asunto, "[Nro]", id);
                SendMail(sRemitente, vendedor.ct_nombreCompleto, model.Asunto, sb, model.Para, null, PDF, XML, CDR, esHtml: true,nombreAdjuntos:id);//Cliente
                SendMail(sRemitente, "Vendedor: " + vendedor.ct_nombreCompleto, model.Asunto, sb, Remite, null, PDF, XML, CDR, esHtml: true,nombreAdjuntos:id);//BackOffice
                if (!string.IsNullOrEmpty(model.ConCopia))
                {
                    SendMail(sRemitente, Label, model.Asunto, sb, model.ConCopia, null, PDF, XML, CDR, esHtml: true,nombreAdjuntos:id);//Vendedor
                }

                //string sCredencialesDocVenta = ConfigurationManager.AppSettings["CREDENCIALES_DOC_VENTA"];
                //if (!(string.IsNullOrEmpty(sCredencialesDocVenta)))
                //{
                //    var lsCredencialesDocVenta = sCredencialesDocVenta.Split(',');
                //    using (NetworkShareAccesser.Access(lsCredencialesDocVenta[0], lsCredencialesDocVenta[1], lsCredencialesDocVenta[2], lsCredencialesDocVenta[3]))
                //    {
                //        var sqlDB = new System.Data.SqlClient.SqlConnectionStringBuilder(
                //         ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                //        string ruc_remitente;
                //        if (sqlDB.InitialCatalog == "ZICO_ERP04")
                //        {
                //            ruc_remitente = "20309525532";
                //        }
                //        else
                //        {
                //            ruc_remitente = "20265733515";
                //        }
                //        //Validamos que existan las rutas
                //        if (!System.IO.File.Exists(rutaXml))
                //        {
                //            rutaXml = rutaXml.Replace(ruc_remitente, "R-" + ruc_remitente);
                //            if (!System.IO.File.Exists(rutaXml))
                //            {
                //                rutaXml = null;
                //            }
                //        }
                //        if (!System.IO.File.Exists(rutaZip))
                //        {
                //            rutaZip = rutaZip.Replace(ruc_remitente, "R-" + ruc_remitente);
                //            if (!System.IO.File.Exists(rutaZip))
                //            {
                //                rutaZip = null;
                //            }
                //        }


                //    }
                //}
            }
            catch (Exception ex)
            {
                var exception = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return JsonError(exception);
            }
            return JsonSuccess(1);
        }

        public byte[] obtenerBytePDF(string documento)
        {
            byte[] byteDocumento = new byte[9000];
            string mimetype = "application/pdf";
            string nombreDocumento = "";

            try
            {
                string empresaEmisora = EmpresaSegunBD();

                string tipoDocumentoRelacionado = "";
                var sDatosNro = documento.Split('-');
                switch (sDatosNro[0])
                {
                    case "FA":
                        tipoDocumentoRelacionado = "01";
                        break;
                    case "BO":
                        tipoDocumentoRelacionado = "03";
                        break;
                    case "NC":
                        tipoDocumentoRelacionado = "07";
                        break;
                    case "ND":
                        tipoDocumentoRelacionado = "08";
                        break;
                }

                string serieDocumento = sDatosNro[1].Substring(0, 3);
                string correlativoDocumento = sDatosNro[1].Substring(3, 8);

                DocumentosModel documentosModel = new DocumentosModel();
                documentosModel.empresaEmisora = empresaEmisora == "ACETESA" ? "20265733515" : "20309525532";
                documentosModel.idTipoDocumento = 1;
                documentosModel.tipoDocRelacionado = tipoDocumentoRelacionado;
                documentosModel.serieDocRelacionado = serieDocumento;
                documentosModel.correlativoDocRelacionado = correlativoDocumento;
                var diccionarioDocumento = VentaService.RecuperarDocumentosPorComprobante(documentosModel);
                nombreDocumento = diccionarioDocumento.Keys.First();
                byteDocumento = diccionarioDocumento.Values.First();



            }
            catch (Exception e)
            {
                Console.Error.Write(e.Message);
            }

            return byteDocumento;
        }
        public byte[] obtenerByteCDR(string documento) {
            byte[] byteDocumento = new byte[9000];
            string mimetype = "application/zip";
            string nombreDocumento = "";

            try
            {
                string empresaEmisora = EmpresaSegunBD();

                string tipoDocumentoRelacionado = "";
                var sDatosNro = documento.Split('-');
                switch (sDatosNro[0])
                {
                    case "FA":
                        tipoDocumentoRelacionado = "01";
                        break;
                    case "BO":
                        tipoDocumentoRelacionado = "03";
                        break;
                    case "NC":
                        tipoDocumentoRelacionado = "07";
                        break;
                    case "ND":
                        tipoDocumentoRelacionado = "08";
                        break;
                }

                string serieDocumento = sDatosNro[1].Substring(0, 3);
                string correlativoDocumento = sDatosNro[1].Substring(3, 8);

                DocumentosModel documentosModel = new DocumentosModel();
                documentosModel.empresaEmisora = empresaEmisora == "ACETESA" ? "20265733515" : "20309525532";
                documentosModel.idTipoDocumento = 2;
                documentosModel.tipoDocRelacionado = tipoDocumentoRelacionado;
                documentosModel.serieDocRelacionado = serieDocumento;
                documentosModel.correlativoDocRelacionado = correlativoDocumento;
                var diccionarioDocumento = VentaService.RecuperarDocumentosPorComprobante(documentosModel);
                nombreDocumento = diccionarioDocumento.Keys.First();
                byteDocumento = diccionarioDocumento.Values.First();

            }
            catch (Exception e)
            {
                Console.Error.Write(e.Message);
            }

            return byteDocumento;
        }
        public byte[] obtenerByteXML(string documento)
        {
            byte[] byteDocumento = new byte[9000];
            string mimetype = "application/xml";
            string nombreDocumento = "";

            try
            {
                string empresaEmisora = EmpresaSegunBD();

                string tipoDocumentoRelacionado = "";
                var sDatosNro = documento.Split('-');
                switch (sDatosNro[0])
                {
                    case "FA":
                        tipoDocumentoRelacionado = "01";
                        break;
                    case "BO":
                        tipoDocumentoRelacionado = "03";
                        break;
                    case "NC":
                        tipoDocumentoRelacionado = "07";
                        break;
                    case "ND":
                        tipoDocumentoRelacionado = "08";
                        break;
                }

                string serieDocumento = sDatosNro[1].Substring(0, 3);
                string correlativoDocumento = sDatosNro[1].Substring(3, 8);

                DocumentosModel documentosModel = new DocumentosModel();
                documentosModel.empresaEmisora = empresaEmisora == "ACETESA" ? "20265733515" : "20309525532";
                documentosModel.idTipoDocumento = 3;
                documentosModel.tipoDocRelacionado = tipoDocumentoRelacionado;
                documentosModel.serieDocRelacionado = serieDocumento;
                documentosModel.correlativoDocRelacionado = correlativoDocumento;
                var diccionarioDocumento = VentaService.RecuperarDocumentosPorComprobante(documentosModel);
                nombreDocumento = diccionarioDocumento.Keys.First();
                byteDocumento = diccionarioDocumento.Values.First();

            }
            catch (Exception e)
            {
                Console.Error.Write(e.Message);
            }

            return byteDocumento;
        }


        public static void SendMail(
        string emailAddress, string labelAddress, string subject, StringBuilder body, string toEmail, string conCopia, byte[] attachmentFilename, byte[] attachmentFilename2 = null, byte[] attachmentFilename3 = null, bool esHtml = false, string copiaOculta = null, string nombreAdjuntos = null)
        {
            var mail = new MailMessage();
            var smtpServer = new SmtpClient();

            try
            {
                mail.From = new MailAddress(emailAddress, labelAddress);
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = esHtml;
                mail.Subject = subject;
                mail.Body = body.ToString();

                mail.To.Add(toEmail);
                if (!string.IsNullOrEmpty(conCopia)) mail.CC.Add(new MailAddress(conCopia, ""));
                if (!string.IsNullOrEmpty(copiaOculta)) mail.Bcc.Add(new MailAddress(copiaOculta, ""));
                //Attachment 1
                if (!(attachmentFilename == null))
                {
                    string fileName = nombreAdjuntos + ".pdf";
                    MemoryStream stream = new MemoryStream(attachmentFilename);
                    mail.Attachments.Add(new Attachment(stream, fileName, MediaTypeNames.Application.Pdf));

                }


                ////Attachment 2
                if (!(attachmentFilename2 == null))
                {
                    string fileName = nombreAdjuntos + ".xml";
                    MemoryStream stream = new MemoryStream(attachmentFilename2);
                    mail.Attachments.Add(new Attachment(stream, fileName, MediaTypeNames.Text.Xml));

                }


                ////Attachment 3
                if (!(attachmentFilename3 == null))
                {
                    string fileName = nombreAdjuntos + ".zip";
                    MemoryStream stream = new MemoryStream(attachmentFilename3);
                    mail.Attachments.Add(new Attachment(stream, fileName, MediaTypeNames.Application.Zip));

                }


                smtpServer.Send(mail);
            }
            finally
            {
                smtpServer.Dispose();
                mail.Dispose();
            }
        }

    }







    public class NetworkShareAccesser : IDisposable
    {
        private string _remoteUncName;
        private string _remoteComputerName;

        public string RemoteComputerName
        {
            get
            {
                return this._remoteComputerName;
            }
            set
            {
                this._remoteComputerName = value;
                this._remoteUncName = @"\\" + this._remoteComputerName;
            }
        }

        public string UserName
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }

        #region Consts
        private const int RESOURCE_CONNECTED = 0x00000001;
        private const int RESOURCE_GLOBALNET = 0x00000002;
        private const int RESOURCE_REMEMBERED = 0x00000003;
        private const int RESOURCETYPE_ANY = 0x00000000;
        private const int RESOURCETYPE_DISK = 0x00000001;
        private const int RESOURCETYPE_PRINT = 0x00000002;
        private const int RESOURCEDISPLAYTYPE_GENERIC = 0x00000000;
        private const int RESOURCEDISPLAYTYPE_DOMAIN = 0x00000001;
        private const int RESOURCEDISPLAYTYPE_SERVER = 0x00000002;
        private const int RESOURCEDISPLAYTYPE_SHARE = 0x00000003;
        private const int RESOURCEDISPLAYTYPE_FILE = 0x00000004;
        private const int RESOURCEDISPLAYTYPE_GROUP = 0x00000005;
        private const int RESOURCEUSAGE_CONNECTABLE = 0x00000001;
        private const int RESOURCEUSAGE_CONTAINER = 0x00000002;
        private const int CONNECT_INTERACTIVE = 0x00000008;
        private const int CONNECT_PROMPT = 0x00000010;
        private const int CONNECT_REDIRECT = 0x00000080;
        private const int CONNECT_UPDATE_PROFILE = 0x00000001;
        private const int CONNECT_COMMANDLINE = 0x00000800;
        private const int CONNECT_CMD_SAVECRED = 0x00001000;
        private const int CONNECT_LOCALDRIVE = 0x00000100;
        #endregion

        #region Errors
        private const int NO_ERROR = 0;
        private const int ERROR_ACCESS_DENIED = 5;
        private const int ERROR_ALREADY_ASSIGNED = 85;
        private const int ERROR_BAD_DEVICE = 1200;
        private const int ERROR_BAD_NET_NAME = 67;
        private const int ERROR_BAD_PROVIDER = 1204;
        private const int ERROR_CANCELLED = 1223;
        private const int ERROR_EXTENDED_ERROR = 1208;
        private const int ERROR_INVALID_ADDRESS = 487;
        private const int ERROR_INVALID_PARAMETER = 87;
        private const int ERROR_INVALID_PASSWORD = 1216;
        private const int ERROR_MORE_DATA = 234;
        private const int ERROR_NO_MORE_ITEMS = 259;
        private const int ERROR_NO_NET_OR_BAD_PATH = 1203;
        private const int ERROR_NO_NETWORK = 1222;
        private const int ERROR_BAD_PROFILE = 1206;
        private const int ERROR_CANNOT_OPEN_PROFILE = 1205;
        private const int ERROR_DEVICE_IN_USE = 2404;
        private const int ERROR_NOT_CONNECTED = 2250;
        private const int ERROR_OPEN_FILES = 2401;
        #endregion
        #region PInvoke Signatures
        [DllImport("Mpr.dll")]
        private static extern int WNetUseConnection(
            IntPtr hwndOwner,
            NETRESOURCE lpNetResource,
            string lpPassword,
            string lpUserID,
            int dwFlags,
            string lpAccessName,
            string lpBufferSize,
            string lpResult
            );
        [DllImport("Mpr.dll")]
        private static extern int WNetCancelConnection2(
            string lpName,
            int dwFlags,
            bool fForce
            );
        [StructLayout(LayoutKind.Sequential)]
        private class NETRESOURCE
        {
            public int dwScope = 0;
            public int dwType = 0;
            public int dwDisplayType = 0;
            public int dwUsage = 0;
            public string lpLocalName = "";
            public string lpRemoteName = "";
            public string lpComment = "";
            public string lpProvider = "";
        }
        #endregion
        public static NetworkShareAccesser Access(string remoteComputerName)
        {
            return new NetworkShareAccesser(remoteComputerName);
        }

        public static NetworkShareAccesser Access(string remoteComputerName, string domainOrComuterName, string userName, string password)
        {
            return new NetworkShareAccesser(remoteComputerName,
                                            domainOrComuterName + @"\" + userName,
                                            password);
        }
        public static NetworkShareAccesser Access(string remoteComputerName, string userName, string password)
        {
            return new NetworkShareAccesser(remoteComputerName,
                                            userName,
                                            password);
        }
        private NetworkShareAccesser(string remoteComputerName)
        {
            RemoteComputerName = remoteComputerName;

            this.ConnectToShare(this._remoteUncName, null, null, true);
        }
        private NetworkShareAccesser(string remoteComputerName, string userName, string password)
        {
            RemoteComputerName = remoteComputerName;
            UserName = userName;
            Password = password;
            this.ConnectToShare(this._remoteUncName, this.UserName, this.Password, false);
        }
        private void ConnectToShare(string remoteUnc, string username, string password, bool promptUser)
        {
            NETRESOURCE nr = new NETRESOURCE
            {
                dwType = RESOURCETYPE_DISK,
                lpRemoteName = remoteUnc
            };

            int result;
            if (promptUser)
            {
                result = WNetUseConnection(IntPtr.Zero, nr, "", "", CONNECT_INTERACTIVE | CONNECT_PROMPT, null, null, null);
            }
            else
            {
                result = WNetUseConnection(IntPtr.Zero, nr, password, username, 0, null, null, null);
            }

            if (result != NO_ERROR)
            {
                throw new Win32Exception(result);
            }
        }
        private void DisconnectFromShare(string remoteUnc)
        {
            int result = WNetCancelConnection2(remoteUnc, CONNECT_UPDATE_PROFILE, false);
            if (result != NO_ERROR)
            {
                throw new Win32Exception(result);
            }
        }
        public void Dispose()
        {
            this.DisconnectFromShare(this._remoteUncName);
        }
        //[AcceptVerbs(HttpVerbs.Post)]
        //[HttpPost]
        //public ActionResult EnviarMail(EnviarMailViewModel model, string id, string idCliente)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return JsonValidationError();
        //    }

        //    var sb = new StringBuilder();
        //    sb.AppendLine(model.Mensaje);
        //    try
        //    {

        //        ClienteService.UpdateEmailByCodigo(idCliente, model.Para);

        //        RenderPdf(id, "RenderPdfCotizacion");
        //        var pdfPath = Server.MapPath(string.Format(PathFormatPdf, id));

        //        var cotizacion = CotizacionService.GetById(id);
        //        if (cotizacion != null)
        //        {
        //            var estadoEnviado = ((int)EstadoTypes.Enviado).ToString();
        //            CotizacionService.UpdateEstado(id, estadoEnviado);
        //        }
        //        string sRemitente = User.Identity.Name;
        //        var vendedor = VendedorService.GetByEmail(sRemitente);
        //        model.Asunto = Funciones.Replace(model.Asunto, "[Nro]", id);
        //        Mail.SendMail(sRemitente, vendedor.ct_nombreCompleto, model.Asunto, sb, model.Para, null, pdfPath);//Cliente
        //        Mail.SendMail(sRemitente, "Vendedor: " + vendedor.ct_nombreCompleto, model.Asunto, sb, Remite, null, pdfPath);//BackOffice
        //        if (!string.IsNullOrEmpty(model.ConCopia))
        //        {
        //            Mail.SendMail(sRemitente, Label, model.Asunto, sb, model.ConCopia, null, pdfPath);//Vendedor
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        var exception = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //        return JsonError(exception);
        //    }

        //    return JsonSuccess(1);
        //}
    }

}