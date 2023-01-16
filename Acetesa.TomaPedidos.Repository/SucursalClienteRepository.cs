using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;
using System;

namespace Acetesa.TomaPedidos.Repository
{
    public class SucursalClienteRepository : BaseRepository<TSUCCLIE>, ISucursalClienteRepository
    {
        private readonly IDbContext _dbContext;

        public SucursalClienteRepository(IDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TSUCCLIE> GetByCcAnalis(string ccAnalis)
        {
            var query = _dbContext.Query<TSUCCLIE>().Where(x => x.cc_analis == ccAnalis.Trim());
            return query;
        }

        public IQueryable<TSUCCLIE> GetByCcAnalisCnSuc(string ccAnalis, string cnSuc)
        {
            var query = _dbContext.Query<TSUCCLIE>()
                .Where(x => x.cc_analis == ccAnalis.Trim() && x.cn_suc == cnSuc.Trim());
            return query;
        }

        public IEnumerable<TLUGCLIE> GetLugarEntregaByCcAnalis(string ccAnalis)
        {
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cc_analis",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 11,
                    Value = ccAnalis.Trim()
                }
            };
            var query = _dbContext.GetExecSpEnumerable<TLUGCLIE>("usp_tm_lugent_cliente", sqlParams);
            return query;
        }

        public IEnumerable<TTRASPORTE> GetTransporteByCcAnalis(string ccAnalis)
        {
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cc_analis",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 11,
                    Value = ccAnalis.Trim()
                }
            };
            var query = _dbContext.GetExecSpEnumerable<TTRASPORTE>("usp_tm_transp_cliente", sqlParams);
            return query;
        }

        public static IEnumerable<object> GetTransporteClienteTodos(string ccAnalis)
        {
            IEnumerable<object> usuario = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_LISTA_TRANSPORTE"))
                {
                    oComando.setParamValue("cc_analis", ccAnalis);
                    usuario = oMapper.BuscarTodos<object>(oComando);
                }
            }
            return usuario;
        }
        public static IEnumerable<object> GetLugarEntregaClienteTodos(string ccAnalis)
        {
            IEnumerable<object> usuario = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_LISTA_LUGARENTREGA"))
                {
                    oComando.setParamValue("cc_analis", ccAnalis);
                    usuario = oMapper.BuscarTodos<object>(oComando);
                }
            }
            return usuario;
        }
        public static void setTransporteCliente(string transporte, string estado, string cliente)
        {
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("SET_TRANSPORTE_CLIENTE"))
                {
                    oComando.setParamValue("cc_tipana", "01");
                    oComando.setParamValue("cc_analis", cliente);
                    oComando.setParamValue("cn_suc", "01");
                    oComando.setParamValue("cc_transp", transporte);
                    oComando.setParamValue("cb_activo", estado);
                    oMapper.Ejecutar(oComando);
                }
            }
        }

        public static string add(dynamic entity)
        {
            if (ExisteLugarEntrega(entity))
            {
                throw new ApplicationException("Dirección ya existente");
            }
            string codigo = "0";
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("ADD_LUGARENTREGA"))
                {
                    oComando.setParamValue("sAnalisis", entity.Analisis);
                    oComando.setParamValue("sDireccion", entity.Direccion);
                    oComando.setParamValue("sDepartamento", entity.Departamento);
                    oComando.setParamValue("sProvincia", entity.Provincia);
                    oComando.setParamValue("sDistrito", entity.Distrito);
                    oComando.setParamValue("sZona", entity.Zona);
                    oComando.setParamValue("sEntrega", entity.Entrega);
                    oComando.setParamValue("sCobranza", entity.Cobranza);

                    codigo = oMapper.BuscarPrimero<string>(oComando);
                }
            }
            return codigo;
        }

        public static bool ExisteLugarEntrega(dynamic entity)
        {
            long? item = -1;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("EXISTE_LUGARENTREGA"))
                {
                    oComando.setParamValue("sAnalisis", entity.Analisis);
                    oComando.setParamValue("sDireccion", entity.Direccion);

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
    }
}
