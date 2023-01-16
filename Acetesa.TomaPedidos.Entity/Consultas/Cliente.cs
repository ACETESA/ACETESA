using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Entity.Consultas
{
    public class Cliente
    {
        public string NombreComercial { get; set; }
        public string Ruc { get; set; }
        public List<Sucursal> Sucursales { get; set; }
        public Cliente()
        {
            Sucursales = new List<Sucursal>();
        }
    }
}
