using Acetesa.TomaPedidos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Core.IBusiness
{
    public interface IPlanificacionVisitasService
    {
        List<PlanificacionVisita> getListaVisitasClientes(string correo, string ClienteID, string fechaInicio, string fechaFin, string estadoVisita, string planificacionID);
        List<PlanificacionVisita> ObtenerNumerosPlanificacionActivas();
        List<PlanificacionVisita> ObtenerNumerosPlanificacionNoActivas();
        List<PlanificacionVisita.MotivoVisitaCancelacion> ObtenerMotivosVisita();
        List<PlanificacionVisita.ContactoCliente> ObtenerContactoCliente(string ClienteID);
        List<PlanificacionVisita.Mensajes> RegistrarVisitaClientes(string correo, int PlanificacionID, string ClienteID, string ContactoID, string FechaVisita, int MotivoVisitaID, string ObservacionPlanificacion, int EsVisitaPlanificada, string FechaReal, string ObservacionVisita, string UbicacionLatitud, string UbicacionLongitud, int EsUbicado);
        List<PlanificacionVisita.MotivoVisitaCancelacion> ObtenerMotivosCancelacion();
        List<PlanificacionVisita.Mensajes> CancelarVisitaClientes(int VisitaClienteID, int MotivoCancelacionID, string ObservacionCancelacion);
        List<PlanificacionVisita> RecuperarVisitaClienteByID(int VisitaClienteID);
        List<PlanificacionVisita.Mensajes> EditarVisitaCliente(string correo, int VisitaClienteID, int PlanificacionID, string ClienteID, string ContactoID, string FechaVisita, int MotivoVisitaID, string ObservacionPlanificacion, int EsVisitaPlanificada, string FechaReal, string ObservacionVisita, string UbicacionLatitud, string UbicacionLongitud, int EsUbicado);
        List<PlanificacionVisita> ObtenerNumerosPlanificacionTodos();
        List<PlanificacionVisita> getSelectVisitasClientes(string ClienteID, string fechaEmision);


    }
}
