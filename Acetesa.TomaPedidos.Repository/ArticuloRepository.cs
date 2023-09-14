using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;
using System.Data.Linq.SqlClient;
using System.Configuration;
using System;

namespace Acetesa.TomaPedidos.Repository
{
    public class ArticuloRepository : BaseRepository<ArticuloModel>, IArticuloRepository
    {
        private readonly IDbContext _dbContext;

        public ArticuloRepository(IDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<ArticuloModel> GetAllCodDes()
        {
            //var query = _dbContext.Query<MARTICUL>().Select(x => new ArticuloModel
            //{
            //    cc_artic = x.cc_artic,
            //    cd_artic = x.cd_artic
            //});
            //return query;

            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cc_gruartec",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    Value = "%"
                },
                new SqlParameter
                {
                    ParameterName = "@cc_subgruart",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 3,
                    Value = "%"
                }
            };
            var query = _dbContext
                .GetExecSpEnumerable<PrecioModel>("[usp_tm_lista_precios_grupo]", sqlParams)
                .Select(x => new ArticuloModel
                {
                    cc_artic = x.cc_artic,
                    cd_artic = x.cd_artic
                }).ToList();
            return query;
        }

        public ArticuloModel GetByCodigo(string ccArtic)
        {
            var query = _dbContext.Query<MARTICUL>()
                .Where(x => x.cc_artic == ccArtic.Trim())
                .Select(s => new ArticuloModel
                {
                    cd_artic = s.cd_artic,
                    cc_unmed = s.cc_unmed
                })
                .FirstOrDefault();
            return query;
        }

        public IQueryable<ArticuloModel> GetNombreOrCodigo(string param)
        {
            var cboUso = new[]{"2","3"};
            var cbActivo = new[] { "1" };
            
            List<string> ArticulosPermitidos = new List<string>();
            using (var oConex = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var ocompa = new System.Data.SqlClient.SqlCommand("sp_retorno", oConex))
                {
                    ocompa.CommandType = CommandType.StoredProcedure;
                    oConex.Open();
                    SqlParameter oparae = new SqlParameter("@param",SqlDbType.VarChar);
                    oparae.Value = param;
                    ocompa.Parameters.Add(oparae);
                    var oreader = ocompa.ExecuteReader();
                    while (oreader.Read())
                    {
                        ArticulosPermitidos.Add(oreader.GetString(0));
                    }
                    oConex.Close();
                }
            }

            //ArticulosPermitidos.Add("050070029");

            var query = _dbContext.Query<MARTICUL>()
                .Where(x => ArticulosPermitidos.Contains(x.cc_artic.Trim()) 
                    //&&
                    //cbActivo.Contains( x.cb_activo) && cboUso.Contains(x.cb_uso) &&
                    //(x.cc_artic.Contains(param.Trim()) || x.cd_artic.Contains(param.Trim()))
                    )
                .Select(x => new ArticuloModel
                {
                    cc_artic = x.cc_artic,
                    cd_artic = x.cd_artic
                });
            return query;
        }

        public IEnumerable<ArticuloModel> GetArticuloNombreOrCodigo(string param)
        {
            object[] sqlParameters =
            {
                new SqlParameter
                {
                    ParameterName = "@param",
                    SqlDbType = SqlDbType.VarChar ,
                    Value = param
                }
            };

            var query = _dbContext.GetExecSpEnumerable<ArticuloModel>("usp_tm_getArticuloNombreCodigo", parameters: sqlParameters);
            return query;
        }

        public IEnumerable<ArticuloModel> GetArticuloNombreOrCodigoYGrupo(string grupo, string subGrupo, /*string param,*/ string cc_tienda)
        {
            List<ArticuloModel> listaArticulos = new List<ArticuloModel>();

            string query = "[dbo].[usp_tm_getArticuloNombreCodigoYGrupo]";
            string connect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@grupo", grupo);
                    cmd.Parameters.AddWithValue("@subGrupo", subGrupo);
                    //cmd.Parameters.AddWithValue("@param", param);
                    cmd.Parameters.AddWithValue("@cc_tienda", cc_tienda);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            listaArticulos.Add(
                                new ArticuloModel
                                {
                                    cc_artic = reader["cc_artic"].ToString(),
                                    cd_artic = reader["cd_artic"].ToString()
                                }
                                );
                        }
                    }
                }
            }
            return listaArticulos;
        }

        public List<ArticuloModel.ArticuloGS> GrupoSubgrupoSegunArtic(string idArticulo)
        {
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cc_artic",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 20,
                    Value = idArticulo
                }
            };
            List<ArticuloModel.ArticuloGS> lista = new List<ArticuloModel.ArticuloGS>();
            lista = _dbContext.GetExecSpEnumerable<ArticuloModel.ArticuloGS>("spObtenerGrupoSubgrupoArticulo", sqlParams).ToList();
            return lista;
        }
        public List<ArticuloModel.Stock> StockSegunArticulo(string idArticulo)
        {
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cc_artic",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 20,
                    Value = idArticulo
                }
            };
            List<ArticuloModel.Stock> lista = new List<ArticuloModel.Stock>();
            lista = _dbContext.GetExecSpEnumerable<ArticuloModel.Stock>("spStockPorCodigoArticulo", sqlParams).ToList();
            return lista;
        }

        public Dictionary<string, string> ValidaStockArticulo_LCPEDIDOWEB(string cc_artic, string cc_tienda, decimal StockSolicitado, bool EsProforma, string cn_proforma, string cn_pedido)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            var cn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var conexion = new SqlConnection(cn);
            using (conexion)
            {
                SqlCommand cmd = new SqlCommand("SP_ValidaStockArticulo_LCPEDIDOWEB", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cc_artic", cc_artic);
                cmd.Parameters.AddWithValue("@cc_tienda", cc_tienda);
                cmd.Parameters.AddWithValue("@StockSolicitado", StockSolicitado);
                cmd.Parameters.AddWithValue("@EsProforma", EsProforma);
                cmd.Parameters.AddWithValue("@cn_proforma", cn_proforma);
                cmd.Parameters.AddWithValue("@cn_pedido", cn_pedido);
                conexion.Open();
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow fila in dt.Rows)
                {
                    string idRespuesta = fila["IdResultado"].ToString();
                    string respuesta = fila["MsgResultado"].ToString();

                    diccionario.Add("id", idRespuesta);
                    diccionario.Add("mensaje", respuesta);
                }
            }
            return diccionario;
        }
        public Dictionary<string, string> ValidaProductosCotizacion(string cnProforma)
        {   
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            var cn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var conexion = new SqlConnection(cn);
            using (conexion)
            {
                SqlCommand cmd = new SqlCommand("spValidaProductosCotizacion", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cnProforma", cnProforma);
                conexion.Open();
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow fila in dt.Rows)
                {
                    string idRespuesta = fila["idRespuesta"].ToString();
                    string respuesta = fila["respuesta"].ToString();

                    diccionario.Add("id", idRespuesta);
                    diccionario.Add("mensaje", respuesta);
                }
            }
            return diccionario;
        }
        public Dictionary<string, string> ValidarStockPedido(string cnPedido, string listaArtic, string listaStockSolicitado, string ccTienda)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            var cn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var conexion = new SqlConnection(cn);
            using (conexion)
            {
                SqlCommand cmd = new SqlCommand("spValidarStockPedido", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                //OPCION 1
                cmd.Parameters.AddWithValue("@cn_pedido", cnPedido);
                //OPCION 2
                cmd.Parameters.AddWithValue("@lista_ccArtic", listaArtic);
                cmd.Parameters.AddWithValue("@lista_stockSolicitado", listaStockSolicitado);
                cmd.Parameters.AddWithValue("@cc_tienda", ccTienda);
                conexion.Open();
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow fila in dt.Rows)
                {
                    string idMsg = fila["idMsg"].ToString();
                    string descMsg = fila["descMsg"].ToString();

                    diccionario.Add("id", idMsg);
                    diccionario.Add("mensaje", descMsg);
                }
            }
            return diccionario;
        }

        public List<ArticuloModel.Stock> ObtenerStockTodasTiendasPorArticulo(string IdArticulo)
        {
            List<ArticuloModel.Stock> listaStockArticulo = new List<ArticuloModel.Stock>();

            string query = "[web].[spObtenerStockTodasTiendasPorArticulo]";
            string connect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdArticulo", IdArticulo);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            listaStockArticulo.Add(
                                new ArticuloModel.Stock
                                {
                                    tienda = reader["tienda"].ToString(),
                                    stockActual = decimal.Parse(reader["stock"].ToString())
                                }
                                );
                        }
                    }
                }
            }
            return listaStockArticulo;
        }


    }
}
