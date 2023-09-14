using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
                sqlConnection.Close();
            }
            return proforma;
        }

        public void Delete(LCPROF_WEB entity)
        {
            _dbContext.Delete(entity);
        }

        public void GuardarAdicional(LCPROF_WEB entityMaster, string email, string Tienda, int IncluyeIGV, string cn_suc, string cn_contacto, bool imprimirPrecioTN, string observacion, int zonaLiberada)
        {
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("usp_web_graba_cotizacion_ext", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cn_proforma", entityMaster.cn_proforma);
                sqlCommand.Parameters.AddWithValue("@ct_email", email.Trim());
                sqlCommand.Parameters.AddWithValue("@Tienda", Tienda.Trim());
                sqlCommand.Parameters.AddWithValue("@IncluyeIGV", IncluyeIGV);
                sqlCommand.Parameters.AddWithValue("@cn_suc", cn_suc);
                sqlCommand.Parameters.AddWithValue("@cn_contacto", cn_contacto);
                sqlCommand.Parameters.AddWithValue("@imprimirPrecioTN", imprimirPrecioTN);
                sqlCommand.Parameters.AddWithValue("@observacion", observacion);
                sqlCommand.Parameters.AddWithValue("@zonaLiberada", zonaLiberada);
                sqlConnection.Open();

                sqlCommand.ExecuteNonQuery();

                sqlConnection.Close();
            }
        }


        public LCPROFADICIONAL_WEB GetAdicionalById(string cnCotizacion)
        {
            LCPROFADICIONAL_WEB Proforma = new LCPROFADICIONAL_WEB();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("usp_web_get_cotizacion_ext", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cnCotizacion", cnCotizacion);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        Proforma.cc_tienda = reader["cc_tienda"].ToString();
                        Proforma.igv_bo = int.Parse(reader["igv_bo"].ToString());
                        Proforma.cn_suc = reader["cn_suc"].ToString();
                        Proforma.cn_contacto = reader["cn_contacto"].ToString();
                        Proforma.nombreContacto = reader["nombreContacto"].ToString();
                        Proforma.emailContacto = reader["emailContacto"].ToString();
                        Proforma.telefonoContacto = reader["telefonoContacto"].ToString();
                        Proforma.imprimirPrecioTN = int.Parse(reader["imprimirPrecioTN"].ToString());
                        Proforma.observacion = reader["observacion"].ToString();
                        Proforma.zonaLiberada = int.Parse(reader["zonaLiberada"].ToString());
                    }
                }
            }
            return Proforma;
        }

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

        public LCPROF_WEB RecuperarDatosProformaByID(string ProformaID)
        {
            LCPROF_WEB Proforma = new LCPROF_WEB();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("web.spRecuperarDatosProformaByID", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ProformaID", ProformaID);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        Proforma.df_proceso = DateTime.Parse(reader["df_proceso"].ToString());
                        Proforma.cb_estado = reader["cb_estado"].ToString();

                    }
                }
                sqlConnection.Close();
            }
            return Proforma;
        }

        public void EliminarProformaByID(string ProformaID)
        {
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("web.spEliminarProformaByID", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ProformaID", ProformaID);
                sqlConnection.Open();

                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void GuardarCabeceraProforma(LCPROF_WEB Proforma)
        {
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("web.spGuardarCabeceraProforma", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cn_proforma", Proforma.cn_proforma);
                sqlCommand.Parameters.AddWithValue("@cc_tipana", Proforma.cc_tipana);
                sqlCommand.Parameters.AddWithValue("@cc_analis", Proforma.cc_analis);
                sqlCommand.Parameters.AddWithValue("@cd_razsoc", Proforma.cd_razsoc);
                sqlCommand.Parameters.AddWithValue("@cc_moneda", Proforma.cc_moneda);
                sqlCommand.Parameters.AddWithValue("@cc_vta", Proforma.cc_vta);
                sqlCommand.Parameters.AddWithValue("@df_proceso", Proforma.df_proceso);
                sqlCommand.Parameters.AddWithValue("@df_emision", Proforma.df_emision);
                sqlCommand.Parameters.AddWithValue("@fm_tipcam", Proforma.fm_tipcam);
                sqlCommand.Parameters.AddWithValue("@fm_valvta", Proforma.fm_valvta);
                sqlCommand.Parameters.AddWithValue("@fm_igv", Proforma.fm_igv);
                sqlCommand.Parameters.AddWithValue("@fm_totvta", Proforma.fm_totvta);
                sqlCommand.Parameters.AddWithValue("@cb_estado", Proforma.cb_estado);
                //sqlCommand.Parameters.AddWithValue("@cd_atencion", Proforma.cd_atencion);
                //sqlCommand.Parameters.AddWithValue("@cn_pedido", Proforma.cn_pedido);
                //sqlCommand.Parameters.AddWithValue("@cc_analisvend", Proforma.cc_analisvend);
                //sqlCommand.Parameters.AddWithValue("@cc_tienda", Proforma.cc_tienda);
                //sqlCommand.Parameters.AddWithValue("@cc_locac", Proforma.cc_locac);
                //sqlCommand.Parameters.AddWithValue("@cc_artic", Proforma.cc_artic);
                //sqlCommand.Parameters.AddWithValue("@cc_lista", Proforma.cc_lista);
                //sqlCommand.Parameters.AddWithValue("@IncluyeIGV", Proforma.IncluyeIGV);
                //sqlCommand.Parameters.AddWithValue("@cn_suc", Proforma.cn_suc);
                //sqlCommand.Parameters.AddWithValue("@cn_contacto", Proforma.cn_contacto);
                //sqlCommand.Parameters.AddWithValue("@imprimirPrecioTN", Proforma.imprimirPrecioTN);
                //sqlCommand.Parameters.AddWithValue("@observacion", Proforma.observacion);
                //sqlCommand.Parameters.AddWithValue("@idMotivoRechazo", Proforma.idMotivoRechazo);
                //sqlCommand.Parameters.AddWithValue("@mensajeRechazo", Proforma.mensajeRechazo);
                //sqlCommand.Parameters.AddWithValue("@zonaLiberada", Proforma.zonaLiberada);
                sqlCommand.Parameters.AddWithValue("@VisitaClienteID", Proforma.VisitaClienteID);

                sqlConnection.Open();
                var reader = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void GuardarDetalleProforma(LDPROF_WEB DetalleProforma)
        {
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("web.spGuardarDetalleProforma", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cn_proforma", DetalleProforma.cn_proforma);
                sqlCommand.Parameters.AddWithValue("@cn_item", DetalleProforma.cn_item);
                sqlCommand.Parameters.AddWithValue("@cc_artic", DetalleProforma.cc_artic);
                //sqlCommand.Parameters.AddWithValue("@descripcionArticulo", DetalleProforma.descripcionArticulo);
                sqlCommand.Parameters.AddWithValue("@fq_cantidad", DetalleProforma.fq_cantidad);
                sqlCommand.Parameters.AddWithValue("@fq_stock", DetalleProforma.fq_stock);
                sqlCommand.Parameters.AddWithValue("@cc_lista", DetalleProforma.cc_lista);
                sqlCommand.Parameters.AddWithValue("@fm_precio", DetalleProforma.fm_precio);
                sqlCommand.Parameters.AddWithValue("@fm_precio2", DetalleProforma.fm_precio2);
                sqlCommand.Parameters.AddWithValue("@fm_precio_fin", DetalleProforma.fm_precio_fin);
                sqlCommand.Parameters.AddWithValue("@fm_total", DetalleProforma.fm_total);
                sqlCommand.Parameters.AddWithValue("@fq_peso", DetalleProforma.fq_peso);


                sqlConnection.Open();
                var reader = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void RegistrarDocumentoProforma(string ProformaID, byte[] Documento)
        {
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("web.spRegistrarDocumentoProforma", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ProformaID", ProformaID);
                sqlCommand.Parameters.AddWithValue("@Documento", Documento);


                sqlConnection.Open();
                var reader = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
