using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;

namespace Acetesa.TomaPedidos.Repository
{
    public class PedidoRepository : BaseRepository<LCPEDIDO_WEB>, IPedidoRepository
    {
        private readonly IDbContext _dbContext;

        public PedidoRepository(IDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<PedidoListadoModel> GetPedidosByClienteFecInicioFecFinal(
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
            var query = _dbContext.GetExecSpEnumerable<PedidoListadoModel>("usp_web_lista_pedidos", sqlParams);
            return query;
        }
        public IEnumerable<PedidoListadoModel> GetPedidosByClienteFecInicioFecFinal(
            string cliente, DateTime fechaInicio, DateTime fechaFinal, string estado, string vendedor)
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
                },
                new SqlParameter
                {
                     ParameterName = "@ct_email",
                    SqlDbType = SqlDbType.Char,
                    Size = 50,
                    Value = vendedor.Trim()
                }
            };
            var query = _dbContext.GetExecSpEnumerable<PedidoListadoModel>("usp_web_lista_pedidos", sqlParams);
            return query;
        }
        public LCPEDIDO_WEB GetLastId()
        {
            var query = _dbContext.Query<LCPEDIDO_WEB>()
                .OrderByDescending(x => x.cn_pedido)
                .FirstOrDefault();
            return query;
        }

        public void Delete(LCPEDIDO_WEB entity)
        {
            _dbContext.Delete(entity);
        }

        public void GuardarAdicional(LCPEDIDO_WEB entityMaster, string email, string Lugar, string Transporte, string Observacion, string contacto, string IdContactoEntregaDirecta, string Tienda, DateTime FechaEntrega,int IncluyeIGV, string cn_ocompra, int zonaLiberada, string ObservacionGuia)
        {
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cn_pedido",
                    SqlDbType = SqlDbType.Char,
                    Size = 8,
                    Value = entityMaster.cn_pedido
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
                    ParameterName = "@Cn_lug",
                    SqlDbType = SqlDbType.Char,
                    Size = 2,
                    Value = Lugar.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@CC_transp",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 5,
                    Value = Transporte.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@Vt_observacion",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 500,
                    Value = Observacion.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@cn_contacto",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    Value = contacto.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@IdContactoEntregaDirecta",
                    SqlDbType = SqlDbType.Char,
                    Size = 2,
                    Value = IdContactoEntregaDirecta.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@sFechaEntrega",
                    SqlDbType = SqlDbType.DateTime,
                    Value = FechaEntrega
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
                    ParameterName = "@cn_ocompra",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 20,
                    IsNullable = true,
                    Value = cn_ocompra.Trim()
                },
                 new SqlParameter
                {
                    ParameterName = "@zonaLiberada",
                    SqlDbType = SqlDbType.Int,
                    Value = zonaLiberada
                },
                new SqlParameter
                {
                    ParameterName = "@Vt_observacionGuia",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 500,
                    Value = ObservacionGuia.Trim()
                }
            };
            var query = _dbContext.GetExecSpEnumerable<PedidoListadoModel>("usp_web_graba_pedido_ext", sqlParams);
            
        }


        //******************


        //public IEnumerable<ContactoListadoModel> ListarContactoEntregaDirecta(string cc_analis)
        //{

        //    object[] sqlParams =
        //    {
        //        new SqlParameter
        //        {
        //            ParameterName = "@cc_analis",
        //            SqlDbType = SqlDbType.VarChar,
        //            Size = 11,
        //            Value = cc_analis.Trim()
        //        }
        //    };
        //    var query = _dbContext.GetExecSpEnumerable<ContactoListadoModel>("sp_listarContactoEntregaDirecta", sqlParams);
        //    return query;
        //}

        //********************************


        public LCPEDIDOADICIONAL_WEB GetAdicionalById(string cnPedido)
        {
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cnPedido",
                    SqlDbType = SqlDbType.Char,
                    Size = 8,
                    Value = cnPedido
                }
            };
            var query = _dbContext.GetExecSpEnumerable<LCPEDIDOADICIONAL_WEB>("usp_web_get_pedido_ext", sqlParams).FirstOrDefault();
            return query;
        }

        public static string getCotizaciones(string proforma)
        {
            string lista = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_PROFORMA_PEDIDO"))
                {
                    oComando.setParamValue("sproforma", proforma);
                    lista = oMapper.BuscarPrimero<string>(oComando);
                }
            }
            return lista;
        }
        public static void setCotizaciones(string proforma, string sPedido)
        {
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("SET_PROFORMA_PEDIDO"))
                {
                    oComando.setParamValue("sproforma", proforma);
                    oComando.setParamValue("spedido", sPedido);
                    oMapper.Ejecutar(oComando);
                }
            }
        }
        //public string IgvSegunEmpresa()
        //{

        //    var query = _dbContext.GetExecSpEnumerable<LCPEDIDOADICIONAL_WEB>("usp_web_igv_segun_empresa").FirstOrDefault();
        //    return query;
        //}


        public Dictionary<string, string> RegistrarDocumentoOCPorPedido(string idPedido, string usuarioRegistro, byte[] documento)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            var cn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var conexion = new SqlConnection(cn);
            using (conexion)
            {
                SqlCommand cmd = new SqlCommand("[web].[spRegistrarDocumentoOrdenCompraPorPedido]", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPedido", idPedido);
                cmd.Parameters.AddWithValue("@usuarioRegistro", usuarioRegistro);
                cmd.Parameters.AddWithValue("@documento", documento);
                cmd.CommandTimeout = 0;
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

        public Dictionary<string, string> ValidaCreditoSobregiroPorPedido(string ruc, decimal total, string moneda)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            var cn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var conexion = new SqlConnection(cn);
            using (conexion)
            {
                SqlCommand cmd = new SqlCommand("[web].[spValidaCreditoSobregiroPorPedido]", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cc_analis", ruc);
                cmd.Parameters.AddWithValue("@montoTotalPedido", total);
                cmd.Parameters.AddWithValue("@MonedaPedido", moneda);
                cmd.CommandTimeout = 0;
                conexion.Open();
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow fila in dt.Rows)
                {
                    string mensajeID = fila["mensajeID"].ToString();
                    string mensaje = fila["mensaje"].ToString();
                    string DeudaCliente = fila["DeudaCliente"].ToString();
                    string LimiteCreditoCliente = fila["LimiteCreditoCliente"].ToString();
                    string EsSobregiro = fila["EsSobregiro"].ToString();

                    diccionario.Add("mensajeID", mensajeID);
                    diccionario.Add("mensaje", mensaje);
                    diccionario.Add("DeudaCliente", DeudaCliente);
                    diccionario.Add("LimiteCreditoCliente", LimiteCreditoCliente);
                    diccionario.Add("EsSobregiro", EsSobregiro);
                }
            }
            return diccionario;
        }


        public Dictionary<string, string> RegistrarNotaPedidoVenta(LCPEDIDO_WEB Pedido, LCPEDIDOADICIONAL_WEB PedidoAdicional, string DetallePedido, string correoVendedor)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();

            var TotalPrecio2 = Pedido.LDPEDIDO_WEB.Sum(x => x.fm_precio2 * x.fq_cantidad);

            string query = "[web].[spRegistrarNotaPedidoVenta]";
            string connect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cc_moneda", Pedido.cc_moneda);
                    cmd.Parameters.AddWithValue("@cc_analiscli", Pedido.cc_analis);
                    cmd.Parameters.AddWithValue("@Cn_lug", PedidoAdicional.Cn_lug);
                    cmd.Parameters.AddWithValue("@cc_vta", Pedido.cc_vta);
                    cmd.Parameters.AddWithValue("@correoVendedor", correoVendedor);
                    cmd.Parameters.AddWithValue("@fm_tipcam", Pedido.fm_tipcam);
                    cmd.Parameters.AddWithValue("@cn_ocompra", PedidoAdicional.cn_ocompra);
                    cmd.Parameters.AddWithValue("@fm_bruto", TotalPrecio2); //fm_bruto
                    cmd.Parameters.AddWithValue("@fm_neto", Pedido.fm_valvta); //fm_neto
                    cmd.Parameters.AddWithValue("@fm_total", Pedido.fm_totvta); //@fm_total
                    cmd.Parameters.AddWithValue("@fm_igv", Pedido.fm_igv);
                    cmd.Parameters.AddWithValue("@Vt_observacion", PedidoAdicional.Vt_observacion);
                    cmd.Parameters.AddWithValue("@cn_pedido", Pedido.cn_pedido);
                    cmd.Parameters.AddWithValue("@cc_tienda", PedidoAdicional.cc_tienda);
                    cmd.Parameters.AddWithValue("@TranspIDAgencia", PedidoAdicional.CC_transp); //TranspIDAgencia
                    cmd.Parameters.AddWithValue("@ClienteTipoEntrega", Pedido.cb_recojo);//ClienteTipoEntrega
                    cmd.Parameters.AddWithValue("@FechaEntrega", PedidoAdicional.FechaEntrega);
                    cmd.Parameters.AddWithValue("@TranspContID", PedidoAdicional.ContactoTransporte); //@TranspContID
                    cmd.Parameters.AddWithValue("@DetallePedido", DetallePedido);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            diccionario.Add("mensajeID", reader["mensajeID"].ToString());
                            diccionario.Add("mensaje", reader["mensaje"].ToString());
                            //listaProvincia.Add(
                            //    new UBIGEO
                            //    {
                            //        cc_prov = reader["cc_prov"].ToString(),
                            //        cd_prov = reader["cd_prov"].ToString()
                            //    }
                            //    );
                        }
                    }
                }
            }
            return diccionario;
        }

        public string RecuperarNumeroPedidoByProformaID(string ProformaID)
        {
            string ProformaIDs = "";
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("web.spRecuperarNumeroPedidoByProformaID", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ProformaID", ProformaID);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        ProformaIDs = reader["ProformaIDs"].ToString();

                    }
                }
                sqlConnection.Close();
            }
            return ProformaIDs;
        }


        public string AsignarNumeroProformaAPedido(string ProformaID, string PedidoID)
        {
            string ProformaIDs = "";
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("web.spAsignarNumeroProformaAPedido", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@sproforma", ProformaID);
                sqlCommand.Parameters.AddWithValue("@spedido", PedidoID);
                sqlConnection.Open();

                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            return ProformaIDs;
        }

        public LCPEDIDO_WEB GetLastPedido()
        {
            LCPEDIDO_WEB pedido = new LCPEDIDO_WEB();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("[web].[spObtenerUltimoIDPedidoWeb]", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        pedido.cn_pedido = reader["cn_pedido"].ToString();
                    }
                }
                sqlConnection.Close();
            }
            return pedido;
        }

        public LCPEDIDO_WEB RecuperarDatosPedidoByID(string PedidoID)
        {
            LCPEDIDO_WEB Pedido = new LCPEDIDO_WEB();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("[web].[spRecuperarDatosPedidoByID]", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@PedidoID", PedidoID);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        Pedido.df_proceso = DateTime.Parse(reader["df_proceso"].ToString());
                        Pedido.cb_estado = reader["cb_estado"].ToString();

                    }
                }
                sqlConnection.Close();
            }
            return Pedido;
        }

        public void EliminarPedidoByID(string PedidoID)
        {
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("[web].[spEliminarPedidoByID]", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@PedidoID", PedidoID);
                sqlConnection.Open();

                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void GuardarCabeceraPedido(LCPEDIDO_WEB Pedido)
        {
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("web.spGuardarCabeceraPedido", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@cn_pedido", Pedido.cn_pedido);
                sqlCommand.Parameters.AddWithValue("@cn_proforma", Pedido.cn_proforma);
                sqlCommand.Parameters.AddWithValue("@cc_tipana", Pedido.cc_tipana);
                sqlCommand.Parameters.AddWithValue("@cc_analis", Pedido.cc_analis);
                sqlCommand.Parameters.AddWithValue("@cn_suc", Pedido.cn_suc);
                //sqlCommand.Parameters.AddWithValue("@cn_lug", Pedido.cn_lug);
                sqlCommand.Parameters.AddWithValue("@cd_razsoc", Pedido.cd_razsoc);
                sqlCommand.Parameters.AddWithValue("@cc_moneda", Pedido.cc_moneda);
                sqlCommand.Parameters.AddWithValue("@cc_vta", Pedido.cc_vta);
                sqlCommand.Parameters.AddWithValue("@df_proceso", Pedido.df_proceso);
                sqlCommand.Parameters.AddWithValue("@df_emision", Pedido.df_emision);
                sqlCommand.Parameters.AddWithValue("@fm_tipcam", Pedido.fm_tipcam);
                sqlCommand.Parameters.AddWithValue("@fm_valvta", Pedido.fm_valvta);
                sqlCommand.Parameters.AddWithValue("@fm_igv", Pedido.fm_igv);
                sqlCommand.Parameters.AddWithValue("@fm_totvta", Pedido.fm_totvta);
                sqlCommand.Parameters.AddWithValue("@cb_recojo", Pedido.cb_recojo);
                sqlCommand.Parameters.AddWithValue("@cb_estado", Pedido.cb_estado);
                //sqlCommand.Parameters.AddWithValue("@cc_analisvend", Pedido.cc_analisvend);
                //sqlCommand.Parameters.AddWithValue("@cc_transp", Pedido.cc_transp);
                //sqlCommand.Parameters.AddWithValue("@vt_observacion", Pedido.vt_observacion);
                //sqlCommand.Parameters.AddWithValue("@cn_contacto", Pedido.cn_contacto);
                //sqlCommand.Parameters.AddWithValue("@AprobacionID", Pedido.AprobacionID);
                //sqlCommand.Parameters.AddWithValue("@FechaEntrega", Pedido.FechaEntrega);
                //sqlCommand.Parameters.AddWithValue("@cc_tienda", Pedido.cc_tienda);
                //sqlCommand.Parameters.AddWithValue("@cc_locac", Pedido.cc_locac);
                //sqlCommand.Parameters.AddWithValue("@cc_artic", Pedido.cc_artic);
                //sqlCommand.Parameters.AddWithValue("@cc_lista", Pedido.cc_lista);
                //sqlCommand.Parameters.AddWithValue("@IncluyeIGV", Pedido.IncluyeIGV);
                //sqlCommand.Parameters.AddWithValue("@IdContactoEntregaDirecta", Pedido.IdContactoEntregaDirecta);
                //sqlCommand.Parameters.AddWithValue("@cn_ocompra", Pedido.cn_ocompra);
                //sqlCommand.Parameters.AddWithValue("@zonaLiberada", Pedido.zonaLiberada);

                sqlConnection.Open();
                var reader = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void GuardarDetallePedido(LDPEDIDO_WEB DetallePedido)
        {
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("web.spGuardarDetallePedido", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@cn_pedido", DetallePedido.cn_pedido);
                sqlCommand.Parameters.AddWithValue("@cn_item", DetallePedido.cn_item);
                sqlCommand.Parameters.AddWithValue("@cc_artic", DetallePedido.cc_artic);
                //sqlCommand.Parameters.AddWithValue("@descripcionArticulo", DetallePedido.descripcionArticulo);
                sqlCommand.Parameters.AddWithValue("@fq_cantidad", DetallePedido.fq_cantidad);
                sqlCommand.Parameters.AddWithValue("@fq_peso", DetallePedido.fq_peso);
                sqlCommand.Parameters.AddWithValue("@fq_stock", DetallePedido.fq_stock);
                sqlCommand.Parameters.AddWithValue("@cc_lista", DetallePedido.cc_lista);
                sqlCommand.Parameters.AddWithValue("@fm_precio", DetallePedido.fm_precio);
                sqlCommand.Parameters.AddWithValue("@fm_precio2", DetallePedido.fm_precio2);
                sqlCommand.Parameters.AddWithValue("@fm_precio_fin", DetallePedido.fm_precio_fin);
                sqlCommand.Parameters.AddWithValue("@fm_total", DetallePedido.fm_total);

                sqlConnection.Open();
                var reader = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void RegistrarDocumentoPedido(string PedidoID, byte[] Documento)
        {
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("[web].[spRegistrarDocumentoPedido]", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@PedidoID", PedidoID);
                sqlCommand.Parameters.AddWithValue("@Documento", Documento);


                sqlConnection.Open();
                var reader = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }



    }
}
