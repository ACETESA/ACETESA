using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;

namespace Acetesa.TomaPedidos.Repository
{
    public class CondicionesVentasRepository : BaseRepository<CondicionVentaModel>, ICondicionesVentasRepository
    {
        private readonly IDbContext _dbContext;

        public CondicionesVentasRepository(IDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<CondicionVentaModel> GetAll()
        {
            var query = _dbContext.GetExecSpEnumerable<CondicionVentaModel>("sp_ventas_ay_tcondvta");
            return query;
        }

        public IEnumerable<CondicionVentaModel> GetAll(string ccAnalis)
        {
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cc_tipana",
                    SqlDbType = SqlDbType.Char,
                    Size = 2,
                    Value = "01"
                },
                new SqlParameter
                {
                    ParameterName = "@cc_analis",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 11,
                    Value = ccAnalis.Trim()
                }
            };
            var query = _dbContext.GetExecSpEnumerable<CondicionVentaModel>("sp_ventas_cliente_acondvta", sqlParams);
            return query;
        }


        public List<CondicionVentaModel> RecuperarCondicionVentaPorClienteID(string cc_analis)
        {
            List<CondicionVentaModel> ListaCondicion = new List<CondicionVentaModel>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("[web].[spRecuperarCondicionVentaPorClienteID]", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cc_analis", cc_analis);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        CondicionVentaModel Condicion = new CondicionVentaModel();
                        Condicion.cc_vta = reader["cc_vta"].ToString();
                        Condicion.cd_vta = reader["cd_vta"].ToString();
                        ListaCondicion.Add(Condicion);
                    }
                }
                sqlConnection.Close();
            }
            return ListaCondicion;
        }

    }
}
