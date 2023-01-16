using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;

namespace Acetesa.TomaPedidos.Repository
{
    public class ListaPrecioRepository : BaseRepository<ListaPrecioModel>, IListaPrecioRepository
    {
        private readonly IDbContext _dbContext;

        public ListaPrecioRepository(IDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public ListaPrecioModel GetPreciosByCodArticulo(string ccArtic, string ct_email, string cc_tienda, string cc_lista, int igv_bo)
        {
            if (string.IsNullOrEmpty(ccArtic) || string.IsNullOrWhiteSpace(ccArtic))
            {
                throw new ArgumentNullException("ccArtic");
            }

            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cc_artic",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 10,
                    Value = ccArtic.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@ct_email",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = ct_email.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@cc_tienda",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    IsNullable = true,
                    Value = cc_tienda == null ? "0" : cc_tienda.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@cc_lista",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    IsNullable = true,
                    Value = cc_lista == null ? "0" : cc_lista.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@igv_bo",
                    SqlDbType = SqlDbType.Bit,
                    IsNullable = true,
                    Value = igv_bo
                }
            };
            var query = _dbContext
                .GetExecSpEnumerable<ListaPrecioModel>("usp_tm_precio_articulo", sqlParams)
                .FirstOrDefault();
            return query;
        }

        public ListaPrecioModel GetCcListaByCcArtic(string ccArtic)
        {
            var query = _dbContext.Query<TLISTAPRECIO>()
                .Where(x => x.cc_artic == ccArtic.Trim())
                .Select(s => new ListaPrecioModel
                {
                    cc_lista = s.cc_lista
                })
                .FirstOrDefault();
            return query;
        }
    }
}
