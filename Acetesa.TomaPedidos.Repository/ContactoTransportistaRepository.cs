using System;
using System.Collections.Generic;

namespace Acetesa.TomaPedidos.Repository
{
    public class ContactoTransportistaRepository
    {
        public static long add(dynamic entity)
        {
            if (ExisteContactoTransportista(entity))
            {
                throw new ApplicationException("Nombre ya existente");
            }
            long codigo = 0;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("ADD_CONTACTO_TRANSPORTISTA"))
                {
                    oComando.setParamValue("sTransportista", entity.Transportista);
                    oComando.setParamValue("sNombres", entity.Nombres);
                    oComando.setParamValue("sTelefono1", entity.Telefono1);
                    oComando.setParamValue("sTelefono2", entity.Telefono2);
                    oComando.setParamValue("sEmail", entity.Email);

                    codigo = oMapper.BuscarPrimero<long>(oComando);
                }
            }
            return codigo;
        }
        public static bool ExisteContactoTransportista(dynamic entity)
        {
            long? item = -1;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("EXISTE_CONTACTO_TRANSPORTISTA"))
                {
                    oComando.setParamValue("sTransportista", entity.Transportista);
                    oComando.setParamValue("sNombres", entity.Nombres);

                    item = oMapper.BuscarPrimero<long>(oComando);
                }
            }
            if (item != null && item.Value > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void ActivaContactoTransportista(dynamic entity)
        {
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("ACTIVA_CONTACTO_TRANSPORTISTA"))
                {
                    oComando.setParamValue("sTransportista", entity.Transportista);
                    oComando.setParamValue("sContacto", entity.Contacto);
                    oComando.setParamValue("sActivo", entity.Activo);

                    oMapper.Ejecutar(oComando);
                }
            }
        }
        public static IEnumerable<object> getByTransportista(string transporte)
        {
            IEnumerable<object> usuario = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_CONTACTO_TRANSPORTE"))
                {
                    oComando.setParamValue("sTransportista", transporte);
                    usuario = oMapper.BuscarTodos<object>(oComando);
                }
            }
            return usuario;
        }
        public static IEnumerable<dynamic> getByTransportistaActivos(string transporte)
        {
            IEnumerable<dynamic> usuario = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_CONTACTO_TRANSPORTE_ACTIVOS"))
                {
                    oComando.setParamValue("sTransportista", transporte);
                    usuario = oMapper.BuscarTodos<dynamic>(oComando);
                }
            }
            return usuario;
        }
    }
}
