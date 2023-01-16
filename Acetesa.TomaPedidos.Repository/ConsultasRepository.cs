using System;
using System.Collections.Generic;
using Acetesa.TomaPedidos.Entity.Consultas;
using Dapper;
using System.Linq;


namespace Acetesa.TomaPedidos.Repository
{
    public class ConsultasRepository
    {
        public static List<Contacto> getContactos(string sUsuario)
        {
            List<Contacto> items = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_CONTACTOS_X_USUARIO"))
                {
                    oComando.setParamValue("usuario", sUsuario);
                    items = oMapper.BuscarTodos<Contacto>(oComando);
                }
            }
            return items;
        }
        public static List<Cliente> getClientes(string sUsuario)
        {
            List<Cliente> items = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_CLIENTES_X_USUARIO"))
                {
                    oComando.setParamValue("usuario", sUsuario);
                    var dicMiembro = new Dictionary<string, Cliente>();
                    items = oMapper.Conexion.Query<Cliente, Sucursal, Cliente>(oComando.CommandText,
                            (cli, suc) =>
                            {
                                Cliente entrada;

                                if (!dicMiembro.TryGetValue(cli.Ruc, out entrada))
                                {
                                    entrada = cli;
                                    dicMiembro.Add(cli.Ruc, entrada);
                                }
                                entrada.Sucursales.Add(suc);
                                return entrada;
                            }, oComando.getParametros(), null, true, "CodigoSucursal", null, System.Data.CommandType.StoredProcedure).Distinct().ToList();
                }
            }
            return items;
        }
        public static Cliente getCliente(string sRUC)
        {
            Cliente items = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_CLIENTE"))
                {
                    oComando.setParamValue("codigo", sRUC);
                    var dicMiembro = new Dictionary<string, Cliente>();
                    items = oMapper.Conexion.Query<Cliente, Sucursal, Cliente>(oComando.CommandText,
                            (cli, suc) =>
                            {
                                Cliente entrada;

                                if (!dicMiembro.TryGetValue(cli.Ruc, out entrada))
                                {
                                    entrada = cli;
                                    dicMiembro.Add(cli.Ruc, entrada);
                                }
                                entrada.Sucursales.Add(suc);
                                return entrada;
                            }, oComando.getParametros(), null, true, "CodigoSucursal", null, System.Data.CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            return items;
        }
        public static Contacto getContacto(string sRuc, string sSucursal, string sContacto)
        {
            Contacto items = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_CONTACTO"))
                {
                    oComando.setParamValue("ruc", sRuc);
                    oComando.setParamValue("suc", sSucursal);
                    oComando.setParamValue("codigo", sContacto);
                    items = oMapper.BuscarPrimero<Contacto>(oComando);
                }
            }
            return items;
        }
        public static Usuario login(string sUsuario, string sClave)
        {
            Usuario items = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("LOGIN"))
                {
                    oComando.setParamValue("sUsuario", sUsuario);
                    oComando.setParamValue("sClave", sClave);
                    items = oMapper.BuscarPrimero<Usuario>(oComando);
                }
            }
            return items;
        }
        public static void setContacto(string codigo, string cliente, string sucursal, string nombres, string telefono1, string telefono2, string anexo, string mail, string tipo, string activo, string sUsuario, string Cargo, string EnvioDocs)
        {
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("SET_CONTACTO"))
                {
                    oComando.setParamValue("codigo", codigo);
                    oComando.setParamValue("cliente", cliente);
                    oComando.setParamValue("sucursal", sucursal);
                    oComando.setParamValue("nombres", nombres);
                    oComando.setParamValue("telefono1", telefono1);
                    oComando.setParamValue("telefono2", telefono2);
                    oComando.setParamValue("anexo", anexo);
                    oComando.setParamValue("mail", mail);
                    oComando.setParamValue("tipo", tipo);
                    oComando.setParamValue("activo", activo);
                    oComando.setParamValue("sCargo", Cargo);
                    oComando.setParamValue("sEnvioDocs", EnvioDocs);
                    oComando.setParamValue("sUsuario", sUsuario);
                    oMapper.Ejecutar(oComando);
                }
            }
        }
    }
}
