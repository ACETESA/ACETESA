using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Transversal.Enums;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class PedidoFindViewModel
    {
        private DateTime _fechaActual = DateTime.Today;
        private string _fechaInicio;
        private string _fechaFin;

        [RegularExpression(@"^(([1][0])|([2][0])|(\d{2}))\d{9}$", ErrorMessage = "RUC de {0} incorrecto.")]
        public string Cliente { get; set; }
        [Display(Name = "Fecha Inicio")]
        [Required(ErrorMessage = "Debe ingresar {0}.")]
        public string FechaInicio
        {
            get
            {
                if (string.IsNullOrEmpty(_fechaInicio) || string.IsNullOrWhiteSpace(_fechaInicio))
                {
                    _fechaInicio = new DateTime(_fechaActual.Year, _fechaActual.Month, 1).ToString("dd/MM/yyyy",
                    CultureInfo.CreateSpecificCulture("es-PE"));
                }
                return _fechaInicio;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _fechaInicio = value;
            }
        }
        [Display(Name = "Fecha Final")]
        [Required(ErrorMessage = "Debe ingresar {0}.")]
        public string FechaFinal
        {
            get
            {
                if (string.IsNullOrEmpty(_fechaFin) || string.IsNullOrWhiteSpace(_fechaFin))
                {
                    DateTime dFechaFin = new DateTime(_fechaActual.Year, _fechaActual.Month, 1).AddMonths(1).AddDays(-1);
                    _fechaFin = dFechaFin.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-PE"));
                }
                return _fechaFin;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _fechaFin = value;
            }
        }

        public EstadoPedidoTypes Estado { get; set; }
        public IEnumerable<PedidoListadoModel> CotizacionModels { get; set; }
        public PagedList.IPagedList<PedidoListadoModel> PagedListListaEntity { get; set; }

        public EnviarMailViewModel EnviarMailViewModel { get; set; }
    }
}