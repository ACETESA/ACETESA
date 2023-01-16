using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Repository
{
    public class UbigeoRepository
    {
        public static List<dynamic> getDepartamentos()
        {
            List<dynamic> usuario = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_DEPARTAMENTOS"))
                {
                    usuario = oMapper.BuscarTodos<dynamic>(oComando);
                }
            }
            return usuario;
        }
        public static List<dynamic> getProvincia(string dep)
        {
            List<dynamic> usuario = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_PROVINCIA"))
                {
                    oComando.setParamValue("sDepartamento", dep);
                    usuario = oMapper.BuscarTodos<dynamic>(oComando);
                }
            }
            return usuario;
        }
        public static List<dynamic> getDistrito(string dep, string prov)
        {
            List<dynamic> usuario = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_DISTRITO"))
                {
                    oComando.setParamValue("sDepartamento", dep);
                    oComando.setParamValue("sProvincia", prov);
                    usuario = oMapper.BuscarTodos<dynamic>(oComando);
                }
            }
            return usuario;
        }
        public static List<dynamic> getZona(string dep, string prov, string dist)
        {
            List<dynamic> usuario = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_ZONA"))
                {
                    oComando.setParamValue("cc_dpto", dep);
                    oComando.setParamValue("cc_prov", prov);
                    oComando.setParamValue("cc_distrito", prov);
                    usuario = oMapper.BuscarTodos<dynamic>(oComando);
                }
            }
            return usuario;
        }
    }
}
