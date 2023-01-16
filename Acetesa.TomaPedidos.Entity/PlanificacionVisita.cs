using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Entity
{
    public class PlanificacionVisita
    {
        public int VisitaClienteID { get; set; }
        public int PlanificacionID { get; set; }
        public string ClienteID { get; set; }
        public string Cliente { get; set; }
        public string ContactoID { get; set; }
        public string FechaVisita { get; set; }
        public DateTime FechaVisitaDT { get; set; }
        public int MotivoVisitaID { get; set; }
        public string ObservacionPlanificacion { get; set; }
        public int EsVisitaPlanificada { get; set; }
        public int EstadoVisita { get; set; }
        public string FechaReal { get; set; }
        public string ObservacionVisita { get; set; }
        public string UbicacionLatitud { get; set; }
        public string UbicacionLongitud { get; set; }
        public int MotivoCancelacionID { get; set; }
        public string ObservacionCancelacion { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string DepartamentoId { get; set; }
        public string ProvinciaId { get; set; }
        public string DistritoId { get; set; }


        public class MotivoVisitaCancelacion
        {
            public int MotivoID { get; set; }
            public string Motivo { get; set; }
        }
        public class ContactoCliente
        {
            public string ContactoID { get; set; }
            public string Contacto { get; set; }
        }

        public class Mensajes
        {
            public int MensajeID { get; set; }
            public string Mensaje { get; set; }

        }


        public MotivoVisitaCancelacion motivoVisitaCancelacion;
        public ContactoCliente contactoCliente;
        public Mensajes mensajes;

    }
}
