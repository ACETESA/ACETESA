﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acetesa.TomaPedidos.AdminMvc.Models
{
    public class CotizacionMotivoRechazoModel
    {
        public int idMotivo { get; set; }
        public string descripcion { get; set; }
        public bool estado { get; set; }
    }
}