using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acetesa.TomaPedidos.AdminMvc.Helpers
{
    public class Funciones
    {
        public static string Replace(string valor, string buscar, string reemplazo)
        {
            var ind = valor.ToLower().IndexOf(buscar.ToLower());
            if (ind < 0)
            {
                return valor;
            }
            else
            {
                return valor.Substring(0, ind) + reemplazo + valor.Substring(ind + buscar.Length);
            }
        }
    }
}