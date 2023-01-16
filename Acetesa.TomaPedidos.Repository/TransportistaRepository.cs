using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace Acetesa.TomaPedidos.Repository
{
    public class TransportistaRepository
    {
        public static long add(dynamic entity)
        {
            if (ExisteTransportista(entity))
            {
                throw new ApplicationException("Ruc o Razón Social ya existente");
            }
            long codigo = 0;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("ADD_TRANSPORTISTA"))
                {
                    oComando.setParamValue("sRuc", entity.Ruc);
                    oComando.setParamValue("sRazonSocial", entity.RazonSocial);
                    oComando.setParamValue("sDomicilio", entity.Domicilio);
                    oComando.setParamValue("sDepartamento", entity.Departamento);
                    oComando.setParamValue("sProvincia", entity.Provincia);
                    oComando.setParamValue("sDistrito", entity.Distrito);
                    oComando.setParamValue("sCliente", entity.Cliente);

                    codigo = oMapper.BuscarPrimero<long>(oComando);
                }
            }
            return codigo;
        }
        public static bool ExisteTransportista(dynamic entity)
        {
            long? item = -1;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("EXISTE_TRANSPORTISTA"))
                {
                    oComando.setParamValue("sRuc", entity.Ruc);
                    oComando.setParamValue("sRazonSocial", entity.RazonSocial);

                    item = oMapper.BuscarPrimero<long>(oComando);
                }
            }
            if (item !=null && item.Value > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
