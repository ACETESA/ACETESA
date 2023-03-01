using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;

namespace Acetesa.TomaPedidos.Repository
{
    public class CotizacionRepository : BaseRepository<LCPROF_WEB>, ICotizacionRepository
    {
        private readonly IDbContext _dbContext;

        public CotizacionRepository(IDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<CotizacionListadoModel> GetCotizacionesByClienteFecInicioFecFinal(
            string cliente, DateTime fechaInicio, DateTime fechaFinal, string estado)
        {
            if (string.IsNullOrEmpty(cliente) || string.IsNullOrWhiteSpace(cliente))
            {
                cliente = "%";
            }
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cc_analiscli",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 11,
                    Value = cliente.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@df_desde",
                    SqlDbType = SqlDbType.DateTime,
                    Value = fechaInicio
                },
                new SqlParameter
                {
                    ParameterName = "@df_hasta",
                    SqlDbType = SqlDbType.DateTime,
                    Value = fechaFinal
                },
                new SqlParameter
                {
                     ParameterName = "@cb_estado",
                    SqlDbType = SqlDbType.Char,
                    Size = 1,
                    Value = estado
                }
            };
            var query = _dbContext.GetExecSpEnumerable<CotizacionListadoModel>("[web].[usp_web_lista_cotizaciones]", sqlParams);
            return query;
        }

        public IEnumerable<CotizacionListadoModel> GetCotizacionesByClienteFecInicioFecFinal(
            string cliente, DateTime fechaInicio, DateTime fechaFinal, string vendedor, string estado)
        {
            if (string.IsNullOrEmpty(cliente) || string.IsNullOrWhiteSpace(cliente))
            {
                cliente = "%";
            }
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cc_analiscli",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 11,
                    Value = cliente.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@df_desde",
                    SqlDbType = SqlDbType.DateTime,
                    Value = fechaInicio
                },
                new SqlParameter
                {
                    ParameterName = "@df_hasta",
                    SqlDbType = SqlDbType.DateTime,
                    Value = fechaFinal
                },
                new SqlParameter
                {
                     ParameterName = "@ct_email",
                    SqlDbType = SqlDbType.Char,
                    Size = 50,
                    Value = vendedor.Trim()
                },
                new SqlParameter
                {
                     ParameterName = "@cb_estado",
                    SqlDbType = SqlDbType.Char,
                    Size = 1,
                    Value = estado
                }
            };
            var query = _dbContext.GetExecSpEnumerable<CotizacionListadoModel>("[web].[usp_web_lista_cotizaciones]", sqlParams);
            return query;
        }

        public LCPROF_WEB GetLastCotizacion()
        {
            //var query = _dbContext.Query<LCPROF_WEB>()
            //    .OrderByDescending(x=>x.cn_proforma)
            //    .FirstOrDefault();
            //return query;
            LCPROF_WEB proforma = new LCPROF_WEB();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("spObteneUltimoIDProforma", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        proforma.cn_proforma = reader["cn_proforma"].ToString();
                    }
                }
            }
            return proforma;


        }

        public void Delete(LCPROF_WEB entity)
        {
            _dbContext.Delete(entity);
        }

        public void GuardarAdicional(LCPROF_WEB entityMaster, string email, string Tienda, int IncluyeIGV, string cn_suc, string cn_contacto, bool imprimirPrecioTN, string observacion, int zonaLiberada)
        {
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cn_proforma",
                    SqlDbType = SqlDbType.Char,
                    Size = 8,
                    Value = entityMaster.cn_proforma
                },
                new SqlParameter
                {
                    ParameterName = "@ct_email",
                    SqlDbType = SqlDbType.Char,
                    Size = 50,
                    Value = email.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@Tienda",
                    SqlDbType = SqlDbType.Char,
                    Size = 2,
                    Value = Tienda.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@IncluyeIGV",
                    SqlDbType = SqlDbType.Int,
                    Value = IncluyeIGV
                },
                new SqlParameter
                {
                    ParameterName = "@cn_suc",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    Value = cn_suc
                },
                new SqlParameter
                {
                    ParameterName = "@cn_contacto",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    Value = cn_contacto
                },
                new SqlParameter
                {
                    ParameterName = "@imprimirPrecioTN",
                    SqlDbType = SqlDbType.Bit,
                    Value = imprimirPrecioTN
                },
                new SqlParameter
                {
                    ParameterName = "@observacion",
                    SqlDbType = SqlDbType.VarChar,
                    Value = observacion
                },
                new SqlParameter
                {
                    ParameterName = "@zonaLiberada",
                    SqlDbType = SqlDbType.Int,
                    Value = zonaLiberada
                }
            };
            var query = _dbContext.GetExecSpEnumerable<CotizacionListadoModel>("usp_web_graba_cotizacion_ext", sqlParams);
        }
        public LCPROFADICIONAL_WEB GetAdicionalById(string cnCotizacion)
        {
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cnCotizacion",
                    SqlDbType = SqlDbType.Char,
                    Size = 10,
                    Value = cnCotizacion
                }
            };
            var query = _dbContext.GetExecSpEnumerable<LCPROFADICIONAL_WEB>("usp_web_get_cotizacion_ext", sqlParams).FirstOrDefault();
            return query;
        }
        //public static  LCPROFADICIONAL_WEB CotizacionAdicional(string cnCotizacion) {
        //    CotizacionRepository repository = new CotizacionRepository();
        //    return x;
        //}
        public string AnularRestablecerProforma(string cn_proforma)
        {
            string mensaje="";
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("spAnularRestablecerProforma", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cn_proforma", cn_proforma);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        mensaje = reader["msg"].ToString();
                    }
                }
            }
            return mensaje;
        }

        public List<Entity.Consultas.DetalleCotizacion> DatosDetalleProformaParaPedido(string cn_proforma)
        {
            List<Entity.Consultas.DetalleCotizacion> listaDetalleProforma = new List<Entity.Consultas.DetalleCotizacion>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("spRecuperaDetalleCotizacionParaPedido", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cn_proforma", cn_proforma);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        var cdArtic = reader["cd_artic"].ToString();

                        Entity.Consultas.DetalleCotizacion detalleProforma = new Entity.Consultas.DetalleCotizacion();
                        detalleProforma.cc_artic = reader["cc_artic"].ToString();
                        detalleProforma.cd_artic = cdArtic;
                        detalleProforma.cc_unmed = reader["cc_unmed"].ToString();
                        detalleProforma.cc_lista = reader["cc_lista"].ToString();
                        detalleProforma.fq_cantidad = (decimal) reader["fq_cantidad"];
                        detalleProforma.fq_peso_teorico = (double) reader["fq_peso_teorico"];
                        detalleProforma.fm_precio = (decimal) reader["fm_precio"];
                        detalleProforma.fm_precio2 = (decimal) reader["fm_precio2"];
                        detalleProforma.fm_precio_fin = (decimal) reader["fm_precio_fin"];
                        detalleProforma.fm_total = (decimal) reader["fm_total"];
                        listaDetalleProforma.Add(detalleProforma);
                    }
                }
            }
            return listaDetalleProforma;
        }

        public List<CotizacionMotivoRechazo> ListaMotivosRechazoCotizacion()
        {
            List<CotizacionMotivoRechazo> listaCotizacionMotivoRechazo = new List<CotizacionMotivoRechazo>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("spListaMotivoRechazoCotizacion", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        CotizacionMotivoRechazo mr = new CotizacionMotivoRechazo();
                        mr.idMotivo = (int)reader["idMotivo"];
                        mr.descripcion = reader["descripcion"].ToString();
                        listaCotizacionMotivoRechazo.Add(mr);

                    }
                }
            }
            return listaCotizacionMotivoRechazo;
        }
        public Dictionary<string,string> RegistrarRechazoCotizacion(string cn_proforma, int idMotivo, string mensajeRechazo)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("spRegistrarRechazoCotizacion", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cn_proforma", cn_proforma);
                sqlCommand.Parameters.AddWithValue("@idMotivo", idMotivo);
                sqlCommand.Parameters.AddWithValue("@mensajeRechazo", mensajeRechazo);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        string idMensaje = reader["idMensaje"].ToString();
                        string mensaje = reader["mensaje"].ToString();

                        diccionario.Add("idMensaje", idMensaje);
                        diccionario.Add("mensaje", mensaje);

                    }
                }
            }
            return diccionario;
        }

        public Dictionary<string, string> RegistrarCierreCotizacionParcial(string cn_proforma, int idMotivo, string mensajeRechazo)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("spRegistrarCierreCotizacionParcial", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cn_proforma", cn_proforma);
                sqlCommand.Parameters.AddWithValue("@idMotivo", idMotivo);
                sqlCommand.Parameters.AddWithValue("@mensajeRechazo", mensajeRechazo);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        string idMensaje = reader["idMensaje"].ToString();
                        string mensaje = reader["mensaje"].ToString();

                        diccionario.Add("idMensaje", idMensaje);
                        diccionario.Add("mensaje", mensaje);

                    }
                }
            }
            return diccionario;
        }

        public List<Tuple<string,string>> ValidarTransformacionCotizacionAPedido(string CotizacionID)
        {
            //Dictionary<string, string> diccionario = new Dictionary<string, string>();
            List<Tuple<string, string>> listado = new List<Tuple<string, string>>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("[web].[uspValidarProductosPorIDCotizacion]", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@CotizacionID", CotizacionID);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        string idMensaje = reader["mensajeID"].ToString();
                        string mensaje = reader["mensaje"].ToString();

                        listado.Add(new Tuple<string, string>(idMensaje,mensaje));

                    }
                }
            }
            return listado;
        }
    }
}
