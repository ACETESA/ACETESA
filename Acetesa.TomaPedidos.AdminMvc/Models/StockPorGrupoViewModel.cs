using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Transversal.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class StockPorGrupoViewModel
    {
        private DateTime _fechaActual = DateTime.Today;
        private string _fechaInicio;

        public EmpresaTypes Empresa { get; set; }

        public string Tienda { get; set; }

        [Display(Name = "Grupo")]
        //[Required(ErrorMessage = "Campo requerido")]
        public string Familia { get; set; }

        [Display(Name = "Sub Grupo")]
        public string SubGrupo { get; set; } 

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "Campo requerido")]
        public string Tipo { get; set; }


        [Display(Name = "Fecha Inicio")]
        [Required(ErrorMessage = "Debe ingresar {0}.")]
        public string FechaInicio
        {
            get
            {
                if (string.IsNullOrEmpty(_fechaInicio) || string.IsNullOrWhiteSpace(_fechaInicio))
                {
         

                    //DateTime dFechaIni = new DateTime(_fechaActual.Year, _fechaActual.Month, 1).AddMonths(1).AddDays(-1);
                    _fechaInicio = _fechaActual.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-PE"));
                }
                return _fechaInicio;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _fechaInicio = value;
            }
        }
        public IEnumerable<StockPorGrupoModel> StockPorGrupoModels { get; set; }
    }
}