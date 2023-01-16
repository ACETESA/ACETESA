using Acetesa.TomaPedidos.AdminMvc.Infrastructure;
using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acetesa.TomaPedidos.AdminMvc.Controllers
{
    [Authorize]
    public class PlanificacionVisitasController : BaseController
    {
        public IPlanificacionVisitasService PlanificacionVisitasService { get; set; }
        public UbigeoRepository UbigeoRepository { get; set; }

        // GET: PlanificacionVisitas
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Listado()
        {
            return View();
        }

        public ActionResult ListadoVisitas(string clienteID, string fechaInicio, string fechaFin, string estadoVisita, string planificacionID)
        {
            var listado = PlanificacionVisitasService.getListaVisitasClientes(User.Identity.Name, clienteID, fechaInicio, fechaFin, estadoVisita, planificacionID);

            return JsonSuccess(listado);
        }

        public ActionResult ListaNumeroPlanificacionActivas()
        {
            var listado = PlanificacionVisitasService.ObtenerNumerosPlanificacionActivas();

            return JsonSuccess(listado);
        }

        public ActionResult ListaNumeroPlanificacionNoActivas()
        {
            var listado = PlanificacionVisitasService.ObtenerNumerosPlanificacionNoActivas();

            return JsonSuccess(listado);
        }

        public ActionResult ListaMotivosVisita()
        {
            var listado = PlanificacionVisitasService.ObtenerMotivosVisita();

            return JsonSuccess(listado);
        }

        public ActionResult ListaContactoCliente(string clienteID)
        {
            if (clienteID == null)
            {
                return JsonSuccess("");
            }
            var listado = PlanificacionVisitasService.ObtenerContactoCliente(clienteID);

            return JsonSuccess(listado);
        }

        public ActionResult RegistrarVisitaCliente(int planificacionID, string clienteID, string contactoID, string FechaVisita, int motivoVisitaID, string observacionPlanificacion, int esVisitaPlanificada, string fechaReal, string observacionVisita, string ubicacionLatitud, string ubicacionLongitud, int esUbicado)
        {
            var listado = PlanificacionVisitasService.RegistrarVisitaClientes(User.Identity.Name, planificacionID, clienteID, contactoID, FechaVisita, motivoVisitaID, observacionPlanificacion, esVisitaPlanificada, fechaReal, observacionVisita, ubicacionLatitud, ubicacionLongitud, esUbicado);

            return JsonSuccess(listado);
        }

        public ActionResult ListaMotivosCancelacion()
        {
            var listado = PlanificacionVisitasService.ObtenerMotivosCancelacion();

            return JsonSuccess(listado);
        }
        public ActionResult CancelarVisitaCliente(int VisitaClienteID, int MotivoCancelacionID, string ObservacionCancelacion)
        {
            var listado = PlanificacionVisitasService.CancelarVisitaClientes(VisitaClienteID, MotivoCancelacionID, ObservacionCancelacion);

            return JsonSuccess(listado);
        }

        public ActionResult RecuperarVisitaCliente(int visitaClienteID)
        {
            var listado = PlanificacionVisitasService.RecuperarVisitaClienteByID(visitaClienteID);

            return JsonSuccess(listado);
        }

        public ActionResult EditarVisitaCliente(int visitaClienteID, int planificacionID, string clienteID, string contactoID, string FechaVisita, int motivoVisitaID, string observacionPlanificacion, int esVisitaPlanificada, string fechaReal, string observacionVisita, string ubicacionLatitud, string ubicacionLongitud, int esUbicado)
        {
            var listado = PlanificacionVisitasService.EditarVisitaCliente(User.Identity.Name, visitaClienteID, planificacionID, clienteID, contactoID, FechaVisita, motivoVisitaID, observacionPlanificacion, esVisitaPlanificada, fechaReal, observacionVisita, ubicacionLatitud, ubicacionLongitud, esUbicado);

            return JsonSuccess(listado);
        }

        public ActionResult ListaNumeroPlanificacionTodos()
        {
            var listado = PlanificacionVisitasService.ObtenerNumerosPlanificacionTodos();

            return JsonSuccess(listado);
        }

        public ActionResult ListaDepartamentos()
        {
            var listado = UbigeoRepository.getDepartamentos();
            return JsonSuccess(listado);
        }

        public ActionResult ListaProvincia(string departamentoId)
        {
            var listado = UbigeoRepository.getProvincia(departamentoId);
            return JsonSuccess(listado);
        }

        public ActionResult ListaDistrito(string departamentoId, string provinciaId)
        {
            var listado = UbigeoRepository.getDistrito(departamentoId, provinciaId);
            return JsonSuccess(listado);
        }
    }
}