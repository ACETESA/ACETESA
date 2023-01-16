using Acetesa.TomaPedidos.Core.IBusiness;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Core.Business
{
    public class PlanificacionVisitasService : IPlanificacionVisitasService
    {
        private readonly IPlanificacionVisitasRepository _planificacionVisitasRepository;

        public PlanificacionVisitasService(IPlanificacionVisitasRepository planificacionVisitasRepository)
        {
            if (planificacionVisitasRepository == null)
            {
                throw new ArgumentNullException("planificacionVisitasRepository");
            }
            _planificacionVisitasRepository = planificacionVisitasRepository;
        }

        public List<PlanificacionVisita> getListaVisitasClientes(string correo, string ClienteID, string fechaInicio, string fechaFin, string estadoVisita, string planificacionID)
        {
            if (ClienteID == "")
            {
                ClienteID = "%";
            }
            return _planificacionVisitasRepository.getListaVisitasClientes(correo, ClienteID, fechaInicio, fechaFin, estadoVisita, planificacionID);
        }

        public List<PlanificacionVisita> ObtenerNumerosPlanificacionActivas()
        {
            return _planificacionVisitasRepository.ObtenerNumerosPlanificacionActivas();
        }

        public List<PlanificacionVisita> ObtenerNumerosPlanificacionNoActivas()
        {
            return _planificacionVisitasRepository.ObtenerNumerosPlanificacionNoActivas();
        }
        public List<PlanificacionVisita.MotivoVisitaCancelacion> ObtenerMotivosVisita()
        {
            return _planificacionVisitasRepository.ObtenerMotivosVisita();
        }
        public List<PlanificacionVisita.ContactoCliente> ObtenerContactoCliente(string ClienteID)
        {
            return _planificacionVisitasRepository.ObtenerContactoCliente(ClienteID);
        }
        public List<PlanificacionVisita.Mensajes> RegistrarVisitaClientes(string correo, int PlanificacionID, string ClienteID, string ContactoID, string FechaVisita, int MotivoVisitaID, string ObservacionPlanificacion, int EsVisitaPlanificada, string FechaReal, string ObservacionVisita, string UbicacionLatitud, string UbicacionLongitud, int EsUbicado)
        {


            return _planificacionVisitasRepository.RegistrarVisitaClientes(correo, PlanificacionID, ClienteID, ContactoID, FechaVisita, MotivoVisitaID, ObservacionPlanificacion, EsVisitaPlanificada, FechaReal, ObservacionVisita, UbicacionLatitud, UbicacionLongitud, EsUbicado);
        }
        public List<PlanificacionVisita.MotivoVisitaCancelacion> ObtenerMotivosCancelacion()
        {
            return _planificacionVisitasRepository.ObtenerMotivosCancelacion();
        }
        public List<PlanificacionVisita.Mensajes> CancelarVisitaClientes(int VisitaClienteID, int MotivoCancelacionID, string ObservacionCancelacion)
        {
            return _planificacionVisitasRepository.CancelarVisitaClientes(VisitaClienteID, MotivoCancelacionID, ObservacionCancelacion);
        }
        public List<PlanificacionVisita> RecuperarVisitaClienteByID(int VisitaClienteID)
        {
            return _planificacionVisitasRepository.RecuperarVisitaClienteByID(VisitaClienteID);
        }
        public List<PlanificacionVisita.Mensajes> EditarVisitaCliente(string correo, int VisitaClienteID, int PlanificacionID, string ClienteID, string ContactoID, string FechaVisita, int MotivoVisitaID, string ObservacionPlanificacion, int EsVisitaPlanificada, string FechaReal, string ObservacionVisita, string UbicacionLatitud, string UbicacionLongitud, int EsUbicado)
        {
            return _planificacionVisitasRepository.EditarVisitaCliente(correo, VisitaClienteID, PlanificacionID, ClienteID, ContactoID, FechaVisita, MotivoVisitaID, ObservacionPlanificacion, EsVisitaPlanificada, FechaReal, ObservacionVisita, UbicacionLatitud, UbicacionLongitud, EsUbicado);
        }
        public List<PlanificacionVisita> ObtenerNumerosPlanificacionTodos()
        {
            return _planificacionVisitasRepository.ObtenerNumerosPlanificacionTodos();
        }
        public List<PlanificacionVisita> getSelectVisitasClientes(string ClienteID, string fechaEmision)
        {
            return _planificacionVisitasRepository.getSelectVisitasClientes(ClienteID,fechaEmision);
        }
    }
}
