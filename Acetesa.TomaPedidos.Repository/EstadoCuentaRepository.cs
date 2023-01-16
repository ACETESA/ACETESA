using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;

namespace Acetesa.TomaPedidos.Repository
{
    public class EstadoCuentaRepository : BaseRepository<EstadoCuentaModel>, IEstadoCuentaRepository
    {
        private readonly IDbContext _dbContext;

        public EstadoCuentaRepository(IDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public EstadoCuentaResumenModel GetResumenByRuc(string ruc)
        {
            if (string.IsNullOrEmpty(ruc) || string.IsNullOrWhiteSpace(ruc))
            {
                throw new ArgumentNullException("ruc");
            }
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cc_analiscli",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 11,
                    Value = ruc.Trim()
                }
            };
            var query = _dbContext.GetExecSpEnumerable<EstadoCuentaResumenModel>("usp_tm_estado_cuenta_res", sqlParams).FirstOrDefault();
            return query;
        }

        public IEnumerable<EstadoCuentaDetalleModel> GetDetalleByRuc(string ruc)
        {
            if (string.IsNullOrEmpty(ruc) || string.IsNullOrWhiteSpace(ruc))
            {
                throw new ArgumentNullException("ruc");
            }
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cc_analiscli",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 11,
                    Value = ruc.Trim()
                }
            };
            var query = _dbContext.GetExecSpEnumerable<EstadoCuentaDetalleModel>("usp_tm_estado_cuenta_det", sqlParams);
            return query;
        }
    }
}
