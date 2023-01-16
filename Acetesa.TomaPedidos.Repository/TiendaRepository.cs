using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Acetesa.TomaPedidos.Repository
{
    public class TiendaRepository: ITiendaRepository
    {
        private readonly IDbContext _dbContext;

        public TiendaRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public static IEnumerable<dynamic> getTiendas()
        {
            IEnumerable<dynamic> lista = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_TIENDAS"))
                {
                    lista = oMapper.BuscarTodos<dynamic>(oComando);
                }
            }
            return lista;
        }
        public static IEnumerable<dynamic> getTiendasByParam(string empresa)
        {
            IEnumerable<dynamic> lista = null;
            if (empresa.Contains("acetesa"))
            {
                using (var oMapper = new Mapper())
                {
                    using (var oComando = Mapper.getComando("GET_TIENDAS_ACETESA"))
                    {
                        lista = oMapper.BuscarTodos<dynamic>(oComando);
                    }
                }
            }
            else
            {
                using (var oMapper = new Mapper())
                {
                    using (var oComando = Mapper.getComando("GET_TIENDAS_GALPESA"))
                    {
                        lista = oMapper.BuscarTodos<dynamic>(oComando);
                    }
                }
            }

            return lista;
        }
        //public TTIENDA GetTiendaSegunVendedor(string ct_email)
        //{
        //    object[] sqlParams =
        //    {
        //        new SqlParameter
        //        {
        //            ParameterName = "@ct_email",
        //            SqlDbType = SqlDbType.Char,
        //            Size = 100,
        //            Value = ct_email
        //        }
        //    };
        //    var query = _dbContext.GetExecSpEnumerable<TTIENDA>("spTiendaSegunVendedor", sqlParams).FirstOrDefault();
        //    return query;
        //}
    }
}
