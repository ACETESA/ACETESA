using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class VendedorViewModel
    {
        public class CorreoElectronico
        {
            [DataType(DataType.EmailAddress)]
            [EmailAddress(ErrorMessage = "Correo Inválido")]
            [Display(Name = "Correo Electrónico")]
            public string usuario { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string clave { get; set; }
        }

        public CorreoElectronico correoElectronico { get; set; }
    }
}