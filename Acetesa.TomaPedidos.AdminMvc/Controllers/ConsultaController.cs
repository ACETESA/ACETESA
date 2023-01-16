using Acetesa.TomaPedidos.Repository;
using Acetesa.TomaPedidos.Entity.Consultas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Acetesa.TomaPedidos.AdminMvc.App_Start;

namespace Acetesa.TomaPedidos.AdminMvc.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [BasicAuthentication]
    public class ConsultaController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Route("api/Consulta/getContactos")]
        [HttpGet]
        public List<Contacto> getContactos()
        {
            var httpRequest = HttpContext.Current.Request.QueryString;
            var usuario = httpRequest["usuario"];
            try
            {
                return ConsultasRepository.getContactos(usuario);
            }
            catch (Exception ex)
            {
                log.Error("No se pudo obtener los Contactos", ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se pudo obtener los Contactos"));
            }
        }

        [Route("api/Consulta/getClientes")]
        [HttpGet]
        public List<Cliente> getClientes()
        {
            var httpRequest = HttpContext.Current.Request.QueryString;
            var usuario = httpRequest["usuario"];
            try
            {
                return ConsultasRepository.getClientes(usuario);
            }
            catch (Exception ex)
            {
                log.Error("No se pudo obtener los Clientes", ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se pudo obtener los Clientes"));
            }
        }

        [Route("api/Consulta/getCliente")]
        [HttpGet]
        public Cliente getCliente()
        {
            var httpRequest = HttpContext.Current.Request.QueryString;
            var sRUC = httpRequest["ruc"];
            try
            {
                return ConsultasRepository.getCliente(sRUC);
            }
            catch (Exception ex)
            {
                log.Error("No se pudo obtener el Cliente", ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se pudo obtener el Cliente"));
            }
        }

        [Route("api/Consulta/getContacto")]
        [HttpGet]
        public Contacto getContacto()
        {
            var httpRequest = HttpContext.Current.Request.QueryString;
            var sRuc = httpRequest["ruc"];
            var sSucursal = httpRequest["ruc"];
            string sContacto = httpRequest["contacto"];
            try
            {
                return ConsultasRepository.getContacto(sRuc, sSucursal, sContacto);
            }
            catch (Exception ex)
            {
                log.Error("No se pudo obtener el contacto", ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se pudo obtener el contacto"));
            }
        }

        [Route("api/Consulta/setContacto")]
        [HttpPost]
        public HttpResponseMessage setContacto()
        {
            var httpRequest = HttpContext.Current.Request.Form;

            string codigo = httpRequest["CodigoContacto"];
            string cliente = httpRequest["RUC"];
            string sucursal = httpRequest["CodigoSucursal"];
            string nombres = httpRequest["NombresContacto"];
            string telefono1 = httpRequest["Telefono1"];
            string telefono2 = httpRequest["Telefono2"];
            string anexo = httpRequest["Anexo"];
            string mail = httpRequest["Email"];
            string tipo = httpRequest["TipoContacto"];
            string activo = httpRequest["Activo"];
            string usuario = httpRequest["usuarioLogin"];
            string cargo = httpRequest["CargoLaboral"];
            string envioDoc = httpRequest["EnvioDocs"];

            try
            {
                ConsultasRepository.setContacto(codigo, cliente, sucursal, nombres, telefono1, telefono2, anexo, mail, tipo, activo, usuario, cargo, envioDoc);
                return Request.CreateResponse(HttpStatusCode.Created, "OK");
            }
            catch (Exception ex)
            {
                log.Error("No se pudo grabar el contacto", ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se pudo grabar el contacto"));
            }
        }

        [Route("api/Consulta/login")]
        [HttpPost]
        public Usuario login()
        {
            var httpRequest = HttpContext.Current.Request.Form;

            string susuario = httpRequest["usuario"];
            string clave = httpRequest["clave"];

            try
            {
                var usuario = ConsultasRepository.login(susuario, clave);
                if (usuario == null)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se pudo iniciar sesión"));
                }
                usuario.Password = clave;

                return usuario;
            }
            catch (Exception ex)
            {
                log.Error("No se pudo iniciar sesión", ex);
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se pudo iniciar sesión"));
            }
        }
    }
}