using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Acetesa.TomaPedidos.Domain;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;

namespace Acetesa.TomaPedidos.Repository
{
    public class ProductoRepository : BaseRepository<MARTICUL>, IProductoRepository
    {
        private readonly IDbContext _dbContext;

        public ProductoRepository(IDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Tlistaprec> GetListaPreciosSp()
        {
            var query = _dbContext.GetExecSpEnumerable<Tlistaprec>("sp_mostrarListaPrecios");
            return query;
        }

        public List<Tlistaprec> ListarPreciosArticulosPorEmpresa(string empresa)
        {
            List<Tlistaprec> listaPrecio = new List<Tlistaprec>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("web.spListarPreciosArticulo", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@empresa", empresa);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        Tlistaprec precio = new Tlistaprec();
                        precio.cc_lista = reader["cc_lista"].ToString();
                        precio.cd_lista = reader["cd_lista"].ToString();

                        listaPrecio.Add(precio);
                    }
                }
            }
            return listaPrecio;
        }

        public IEnumerable<Tgruartec> GetFamiliasSp()
        {
            var query = _dbContext.GetExecSpEnumerable<Tgruartec>("sp_cat_mat_ay_tgruartec");
            return query;
        }
        public List<Tgruartec> ListarFamiliasArticulosPorEmpresa(string empresa)
        {
            List<Tgruartec> listaGrupo = new List<Tgruartec>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("web.spListarFamiliasArticulo", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@empresa", empresa);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        Tgruartec grupo = new Tgruartec();
                        grupo.cc_gruartec = reader["cc_gruartec"].ToString();
                        grupo.cd_gruart = reader["cd_gruart"].ToString();

                        listaGrupo.Add(grupo);
                    }
                }
            }
            return listaGrupo;
        }


        public IEnumerable<SubFamiliaModel> GetSubFamiliasByCodFamiliaSp(string codFamilia)
        {
            if (string.IsNullOrEmpty(codFamilia) || string.IsNullOrWhiteSpace(codFamilia))
            {
                throw new ArgumentNullException("codFamilia");
            }
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cc_gruart",
                    SqlDbType = SqlDbType.VarChar,
                    //Size = 3,
                    Value = codFamilia.Trim()
                }
            };
            var query = _dbContext.GetExecSpEnumerable<SubFamiliaModel>("sp_cat_mat_ay_tsubgruartec", sqlParams);
            return query;
        }

        public List<SubFamiliaModel> ListarSubFamiliasArticulosPorEmpresa(string empresa, string cc_gruart)
        {
            List<SubFamiliaModel> listaSubGrupo = new List<SubFamiliaModel>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("web.spListarSubFamiliasArticulo", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@empresa", empresa);
                sqlCommand.Parameters.AddWithValue("@cc_gruart", cc_gruart);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        SubFamiliaModel subgrupo = new SubFamiliaModel();
                        subgrupo.cc_subgruart = reader["cc_subgruart"].ToString();
                        subgrupo.cd_subgruart = reader["cd_subgruart"].ToString();

                        listaSubGrupo.Add(subgrupo);
                    }
                }
            }
            return listaSubGrupo;
        }

        public IEnumerable<Ttipoartic> GetTiposSp()
        {
            var query = _dbContext.GetExecSpEnumerable<Ttipoartic>("sp_cat_mat_ay_ttipoartic");
            return query;
        }

        public List<Ttipoartic> ListarTipoArticulosPorEmpresa(string empresa)
        {
            List<Ttipoartic> listaTipo = new List<Ttipoartic>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("web.spListarTipoArticulo", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@empresa", empresa);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        Ttipoartic tipo = new Ttipoartic();
                        tipo.cc_tipoartic = reader["cc_tipoartic"].ToString();
                        tipo.cd_tipoartic = reader["cd_tipoartic"].ToString();

                        listaTipo.Add(tipo);
                    }
                }
            }
            return listaTipo;
        }

        public IEnumerable<StockModel> GetStockSp(DateTime fechaDelDia, string codFamilia, string codSubFamilia, string codigoTipoArticulo)
        {
            if (string.IsNullOrEmpty(codFamilia) || string.IsNullOrWhiteSpace(codFamilia))
            {
                throw new ArgumentNullException("codFamilia");
            }
            if (string.IsNullOrEmpty(codSubFamilia) || string.IsNullOrWhiteSpace(codSubFamilia))
            {
                throw new ArgumentNullException("codSubFamilia");
            }
            //if (string.IsNullOrEmpty(codigoTipoArticulo) || string.IsNullOrWhiteSpace(codigoTipoArticulo))
            //{
            //    throw new ArgumentNullException("codigoTipoArticulo");
            //}
            object[] sqlParameters =
            {
                new SqlParameter
                {
                    ParameterName = "@fecha_fin",
                    SqlDbType = SqlDbType.DateTime,
                    Value = fechaDelDia
                }, 


                new SqlParameter
                {
                    ParameterName = "@cc_artic_desde",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 10,
                    Value = "0"
                }, 
                new SqlParameter
                {
                    ParameterName = "@cc_artic_hasta",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 10,
                    Value = "ZZZZZZZZZZ"
                }, 
                new SqlParameter
                {
                    ParameterName = "@cc_almac_desde",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 3,
                    Value = "0"
                }, 
                new SqlParameter
                {
                    ParameterName = "@cc_almac_hasta",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 3,
                    Value = "999"
                }, 
                new SqlParameter
                {
                    ParameterName = "@cc_famiart_desde",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    Value = "0"
                }, 
                new SqlParameter
                {
                    ParameterName = "@cc_famiart_hasta",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    Value = "zz"
                }, 


                new SqlParameter
                {
                    ParameterName = "@cc_gruartec_desde",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    Value = codFamilia
                }, 
                new SqlParameter
                {
                    ParameterName = "@cc_gruartec_hasta",
                    SqlDbType =  SqlDbType.VarChar,
                    Size = 2,
                    Value = codFamilia
                }, 
                new SqlParameter
                {
                    ParameterName = "@cc_subgruart_desde",
                    SqlDbType = SqlDbType.VarChar,
                    //Size = 2,
                    Value = codSubFamilia.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@cc_subgruart_hasta",
                    SqlDbType =  SqlDbType.VarChar,
                    //Size = 2,
                    Value = codSubFamilia.Trim()
                }, 
                new SqlParameter
                {
                    ParameterName = "@cc_tipoartic_desde",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    Value = "****"//codigoTipoArticulo.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@cc_tipoartic_hasta",
                    SqlDbType =  SqlDbType.VarChar,
                    Size = 2,
                    Value = "****"//codigoTipoArticulo.Trim()
                }
            };
            
            
            var query = _dbContext.GetExecSpEnumerable<StockModel>("usp_rep_alm_stocks_articulos_new", parameters: sqlParameters);
            return query;
        }

        public Dictionary<string, object> GetStockStoreProcedure(DateTime fechaDelDia, string codFamilia, string codSubFamilia, string codigoTipoArticulo)
        {
            if (string.IsNullOrEmpty(codFamilia) || string.IsNullOrWhiteSpace(codFamilia))
            {
                throw new ArgumentNullException("codFamilia");
            }
            if (string.IsNullOrEmpty(codSubFamilia) || string.IsNullOrWhiteSpace(codSubFamilia))
            {
                throw new ArgumentNullException("codSubFamilia");
            }
            object[] sqlParameters =
            {
                new SqlParameter
                {
                    ParameterName = "@cc_gruartec",
                    SqlDbType =  SqlDbType.VarChar,
                    Size = 2,
                    Value = codFamilia
                },
                new SqlParameter
                {
                    ParameterName = "@cc_subgruart",
                    SqlDbType =  SqlDbType.VarChar,
                    //Size = 2,
                    Value = codSubFamilia.Trim()
                }
            };
            var Resultados = new Dictionary<string, object>();
            var query = new List<Dictionary<string, object>>();
            var cabeceras = new List<string>();
            using (var oConex = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var ocompa = new System.Data.SqlClient.SqlCommand("usp_almacen_rep_stock_tm", oConex))
                {
                    ocompa.CommandType = CommandType.StoredProcedure;
                    oConex.Open();
                    ocompa.Parameters.AddRange(sqlParameters);
                    using (var oTabla = new DataTable())
                    {
                        oTabla.Load(ocompa.ExecuteReader());

                        foreach (DataColumn oColumna in oTabla.Columns)
                        {
                            if (!cabeceras.Contains(oColumna.ColumnName))
                            {
                                cabeceras.Add(oColumna.ColumnName);
                            }
                        }

                        foreach (DataRow oRow in oTabla.Rows)
                        {
                            var oDatoRow = new Dictionary<string,object>();

                            foreach (DataColumn oColumna in oTabla.Columns)
                            {    
                                if (!oDatoRow.ContainsKey(oColumna.ColumnName))
                                {
                                    oDatoRow.Add(oColumna.ColumnName, oRow[oColumna.ColumnName]);
                                }
                            }
                            query.Add(oDatoRow);
                        }
                    }
                    
                    oConex.Close();
                }
            }
            
            Resultados.Add("cabeceras", cabeceras);
            Resultados.Add("datos", query);

            return Resultados;
        }

        public IEnumerable<PrecioModel> GetPreciosSp(string codListaPrecio, string codFamilia, string codSubFamilia, int Stocks)
        {
            if (string.IsNullOrEmpty(codListaPrecio) || string.IsNullOrWhiteSpace(codListaPrecio))
            {
                throw new ArgumentNullException("codListaPrecio");
            }
            if (string.IsNullOrEmpty(codFamilia) || string.IsNullOrWhiteSpace(codFamilia))
            {
                throw new ArgumentNullException("codFamilia");
            }
            if (string.IsNullOrEmpty(codSubFamilia) || string.IsNullOrWhiteSpace(codSubFamilia))
            {
                throw new ArgumentNullException("codSubFamilia");
            }

            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cc_lista",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 5,
                    Value = codListaPrecio.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@cc_gruartec",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    Value = codFamilia.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@cc_subgruart",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 3,
                    Value = codSubFamilia.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@Stocks",
                    SqlDbType = SqlDbType.Int,
                    Value = Stocks
                }
            };
            var query = _dbContext.GetExecSpEnumerable<PrecioModel>("usp_tm_lista_precios_grupo", parameters: sqlParams);
            return query;
        }

        public List<PrecioModel> ListarPreciosArticulosPorGrupoEmpresa(string codListaPrecio, string codFamilia, string codSubFamilia, int Stocks, string empresa)
        {
            if (string.IsNullOrEmpty(codListaPrecio) || string.IsNullOrWhiteSpace(codListaPrecio))
            {
                throw new ArgumentNullException("codListaPrecio");
            }
            if (string.IsNullOrEmpty(codFamilia) || string.IsNullOrWhiteSpace(codFamilia))
            {
                throw new ArgumentNullException("codFamilia");
            }
            if (string.IsNullOrEmpty(codSubFamilia) || string.IsNullOrWhiteSpace(codSubFamilia))
            {
                throw new ArgumentNullException("codSubFamilia");
            }

            List<PrecioModel> listaPrecio = new List<PrecioModel>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("web.spListarPreciosArticuloPorGrupo", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cc_lista", codListaPrecio);
                sqlCommand.Parameters.AddWithValue("@cc_gruartec", codFamilia);
                sqlCommand.Parameters.AddWithValue("@cc_subgruart", codSubFamilia);
                sqlCommand.Parameters.AddWithValue("@Stocks", Stocks);
                sqlCommand.Parameters.AddWithValue("@empresa", empresa);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        PrecioModel precio = new PrecioModel();
                        precio.cc_artic = reader["cc_artic"].ToString();
                        precio.cd_artic = reader["cd_artic"].ToString();
                        precio.cc_unmed = reader["cc_unmed"].ToString();
                        precio.fq_peso_teorico = reader["fq_peso_teorico"].ToString();
                        precio.fq_unid_tm = reader["fq_unid_tm"].ToString();
                        precio.cd_moneda = reader["cd_moneda"].ToString();
                        precio.fm_valorunit = reader["fm_valorunit"].ToString();
                        precio.fm_valorunit2 = reader["fm_valorunit2"].ToString();
                        precio.fm_valorvta_tn = reader["fm_valorvta_tn"].ToString();
                        precio.fm_valorvta_tn2 = reader["fm_valorvta_tn2"].ToString();
                        precio.ni_tipcam = (decimal) reader["ni_tipcam"];
                        precio.ArticStk = (decimal) reader["ArticStk"];
                        listaPrecio.Add(precio);
                    }
                }
            }
            return listaPrecio;
        }

        public List<object> GetEstadisticaVentas(DateTime sFechaInicio, DateTime sFechaFinal, string sVendedor = "", string sEmpresa = "", string sTienda="")
        {
            object[] sqlParameters =
            {
                new SqlParameter
                {
                    ParameterName ="@sEmpresa",
                    SqlDbType = SqlDbType.Char,
                    Size = 7,
                    Value = sEmpresa.Trim()
                },

                new SqlParameter
                {
                    ParameterName = "@dFechaInicio",
                    SqlDbType =  SqlDbType.DateTime,
                    Value = sFechaInicio
                },
                new SqlParameter
                {
                    ParameterName = "@dFechaFinal",
                    SqlDbType =  SqlDbType.DateTime,
                    Value = sFechaFinal
                },
                new SqlParameter
                {
                    ParameterName = "@sTienda_id",
                    SqlDbType =  SqlDbType.Char,
                    Size = 2,
                    Value = sTienda.Trim()
                },
                new SqlParameter
                {
                    ParameterName = "@sVendedor_id",
                    SqlDbType =  SqlDbType.Char,
                    Size = 11,
                    Value = sVendedor.Trim()
                }
            };
            var Resultados = new List<dynamic>();

            using (var oConex = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var ocompa = new System.Data.SqlClient.SqlCommand("usp_EstadisticaVentasPorEmpresa", oConex))
                {
                    ocompa.CommandType = CommandType.StoredProcedure;
                    oConex.Open();
                    ocompa.Parameters.AddRange(sqlParameters);

                    using (var oReader = ocompa.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            dynamic objecto = new
                            {
                                Tienda = ValorCelda(oReader, "Tienda"),
                                Tienda_ID = ValorCelda(oReader, "Tienda_id"),
                                ID = oReader["ID"],
                                Nombre = oReader["Nombre"],
                                VtaSoles = oReader["VtaSoles"],
                                CtoSoles = oReader["CtoSoles"],
                                VtaPeso = oReader["VtaPeso"],
                                MargenPorc = oReader["MargenPorc"]
                            };
                            Resultados.Add(objecto);
                        }
                    }

                    oConex.Close();
                }
            }

            return Resultados;
        }

        private object ValorCelda(SqlDataReader oReader, string campo)
        {
            try
            {
                return oReader[campo].ToString().Trim();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public IEnumerable<StockPorGrupoModel> GetStockPorGrupoSp(string sEmpresa, string sTienda, string codFamilia, string codSubGrupo , string codigoTipoArticulo, DateTime fecha)
        {

            object[] sqlParams2 =
            {
                new SqlParameter
                {
                    ParameterName = "@sEmpresa",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 1,
                    Value = sEmpresa
                },

                new SqlParameter
                {
                    ParameterName = "@fecha_fin",
                    SqlDbType = SqlDbType.DateTime,
                    Value = fecha
                },

                new SqlParameter
                {
                    ParameterName = "@cc_tienda_desde",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 3,
                    Value = sTienda
                },

                new SqlParameter
                {
                    ParameterName = "@cc_tienda_hasta",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 3,
                    Value = sTienda
                },

                new SqlParameter
                {
                    ParameterName = "@cc_almac_desde",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 3,
                    Value = "0"
                },

                 new SqlParameter
                {
                    ParameterName = "@cc_almac_hasta",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 3,
                    Value = "999"
                },

                  new SqlParameter
                {
                    ParameterName = "@cc_artic_desde",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 10,
                    Value = "0"
                },

                   new SqlParameter
                {
                    ParameterName = "@cc_artic_hasta",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 10,
                    Value = "999999999"
                },

                    new SqlParameter
                {
                    ParameterName = "@cc_famiart_desde",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    Value = "0"
                },


                        new SqlParameter
                {
                    ParameterName = "@cc_famiart_hasta",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    Value = "99"
                },


                     new SqlParameter
                {
                    ParameterName = "@cc_gruartec_p",
                    SqlDbType = SqlDbType.VarChar,
                    Value = codFamilia
                },
                         new SqlParameter
                {
                    ParameterName = "@cc_subgruart_p",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 6,
                    Value = codSubGrupo
                },

                             new SqlParameter
                {
                    ParameterName = "@cc_material_p",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    Value = "%"
                },

                                            new SqlParameter
                {
                    ParameterName = "@cc_tipoartic_p",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    Value = codigoTipoArticulo
                },


                new SqlParameter
                {
                    ParameterName = "@cb_reporte",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 1,
                    Value = "N"
                }
            };
            if (sEmpresa == "1") /*Acetesa*/
            {
                var query = _dbContext.GetExecSpEnumerable<StockPorGrupoModel>("ZICO_ERP01.DBO.usp_web_stock_por_grupo", parameters: sqlParams2);
                return query;
            }
            else
            {
                var query = _dbContext.GetExecSpEnumerable<StockPorGrupoModel>("ZICO_ERP04.DBO.usp_web_stock_por_grupo", parameters: sqlParams2);
                return query;
            }
        }



        public MARTICUL RecuperarArticuloPorID(string ArticuloID)
        {
            MARTICUL Articulo = new MARTICUL();

            string query = "[web].[spRecuperarArticuloByID]";
            string connect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cc_artic", ArticuloID);
                    conn.Open();
                    //CommandBehavior.CloseConnection
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Articulo.cc_gruart = reader["cc_gruart"].ToString();
                            Articulo.cc_artic = reader["cc_artic"].ToString();
                            Articulo.cc_famiart = reader["cc_famiart"].ToString();
                            Articulo.cc_marca = reader["cc_marca"].ToString();
                            Articulo.cc_gruartec = reader["cc_gruartec"].ToString();
                            Articulo.cc_subgruart = reader["cc_subgruart"].ToString();
                            Articulo.cc_modelo = reader["cc_modelo"].ToString();
                            Articulo.cd_artic = reader["cd_artic"].ToString();
                            Articulo.cn_parara = reader["cn_parara"].ToString();
                            Articulo.cd_artictc = reader["cd_artictc"].ToString();
                            Articulo.cc_unmed = reader["cc_unmed"].ToString();
                            Articulo.df_ultcom = DateTime.Parse(reader["df_ultcom"].ToString());
                            Articulo.fm_ultcom = double.Parse(reader["fm_ultcom"].ToString());
                            Articulo.df_ultven = DateTime.Parse(reader["df_ultven"].ToString());
                            Articulo.fm_ultven = double.Parse(reader["fm_ultven"].ToString());
                            Articulo.cb_undalt = reader["cb_undalt"].ToString();
                            Articulo.cb_activo = reader["cb_activo"].ToString();
                            Articulo.cb_stocks = reader["cb_stocks"].ToString();
                            Articulo.fq_smin = double.Parse(reader["fq_smin"].ToString());
                            Articulo.fq_smax = double.Parse(reader["fq_smax"].ToString());
                            Articulo.fq_nivrepos = double.Parse(reader["fq_nivrepos"].ToString());
                            Articulo.cb_standard = reader["cb_standard"].ToString();
                            Articulo.ci_consig = reader["ci_consig"].ToString();
                            Articulo.cb_nacional = reader["cb_nacional"].ToString();
                            Articulo.cb_critico = reader["cb_critico"].ToString();
                            Articulo.cb_obsleto = reader["cb_obsleto"].ToString();
                            Articulo.cc_barras = reader["cc_barras"].ToString();
                            Articulo.fm_precprom = double.Parse(reader["fm_precprom"].ToString());
                            Articulo.ct_graf = reader["ct_graf"].ToString();
                            Articulo.fm_ulco_d = double.Parse(reader["fm_ulco_d"].ToString());
                            Articulo.fm_ulve_d = double.Parse(reader["fm_ulve_d"].ToString());
                            Articulo.fm_precprom_d = double.Parse(reader["fm_precprom_d"].ToString());
                            Articulo.ci_abc = reader["ci_abc"].ToString();
                            Articulo.cb_catart = reader["cb_catart"].ToString();
                            Articulo.fm_ultpu = double.Parse(reader["fm_ultpu"].ToString());
                            Articulo.fm_ultpu_d = double.Parse(reader["fm_ultpu_d"].ToString());
                            Articulo.fd_stock_cero = DateTime.Parse(reader["fd_stock_cero"].ToString());
                            Articulo.fm_consumo = double.Parse(reader["fm_consumo"].ToString());
                            Articulo.fq_sinicial = double.Parse(reader["fq_sinicial"].ToString());
                            Articulo.cb_estado = reader["cb_estado"].ToString();
                            Articulo.cb_rotacion = reader["cb_rotacion"].ToString();
                            Articulo.cn_partnumber = reader["cn_partnumber"].ToString();
                            Articulo.cb_uso = reader["cb_uso"].ToString();
                            Articulo.cn_item = reader["cn_item"].ToString();
                            Articulo.fq_embalaje = double.Parse(reader["fq_embalaje"].ToString());
                            Articulo.cc_catalogo = reader["cc_catalogo"].ToString();
                            Articulo.cc_tipoartic = reader["cc_tipoartic"].ToString();
                            Articulo.fq_espesor = double.Parse(reader["fq_espesor"].ToString());
                            Articulo.fq_ancho = double.Parse(reader["fq_ancho"].ToString());
                            Articulo.fq_largo = double.Parse(reader["fq_largo"].ToString());
                            Articulo.cc_costeo = reader["cc_costeo"].ToString();
                            Articulo.cb_eval_precio = reader["cb_eval_precio"].ToString();
                            Articulo.cc_tipArt = reader["cc_tipArt"].ToString();
                            Articulo.cc_costo_kardexpaqbobi = reader["cc_costo_kardexpaqbobi"].ToString();
                            Articulo.df_creacion = DateTime.Parse(reader["df_creacion"].ToString());
                            Articulo.fq_sku = double.Parse(reader["fq_sku"].ToString());
                            Articulo.cc_articant = reader["cc_articant"].ToString();
                            Articulo.cc_color = reader["cc_color"].ToString();
                            Articulo.cb_peso_pt = reader["cb_peso_pt"].ToString();
                            Articulo.cb_mprima = reader["cb_mprima"].ToString();
                            Articulo.cc_simbolo = reader["cc_simbolo"].ToString();
                            Articulo.cc_costeo_pocl = reader["cc_costeo_pocl"].ToString();
                            Articulo.cc_codpeso = reader["cc_codpeso"].ToString();
                            Articulo.cc_gruartecduf = reader["cc_gruartecduf"].ToString();
                            Articulo.cc_subgruartduf = reader["cc_subgruartduf"].ToString();
                            Articulo.c_fl_afecto_percepcion = reader["c_fl_afecto_percepcion"].ToString();
                            Articulo.fq_peso_teorico = double.Parse(reader["fq_peso_teorico"].ToString());

                        }
                    }
                    conn.Close();
                }
            }
            return Articulo;
        }

        //public MARTICUL RecuperarArticuloPorID(string ArticuloID)
        //{
        //    MARTICUL Articulo = new MARTICUL();

        //    // Assuming you have access to a shared IDbContext
        //    using (var scope = new TransactionScope(TransactionScopeOption.Required))
        //    {
        //        try
        //        {
        //            // Your transactional code here

        //            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
        //            {
        //                using (SqlCommand cmd = new SqlCommand("[web].[spRecuperarArticuloByID]", conn))
        //                {
        //                    // ... your data retrieval code here ...
        //                }
        //            }

        //            // Commit the transaction
        //            scope.Complete();
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }

        //    return Articulo;
        //}



        public MARTICUL RecuperarDatosArticuloByID(string ArticuloID)
        {
            MARTICUL Articulo = new MARTICUL();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("web.spRecuperarDatosArticuloByID", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ArticuloID", ArticuloID);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        Articulo.fq_peso_teorico = double.Parse(reader["fq_peso_teorico"].ToString());
                    }
                }
                sqlConnection.Close();
            }
            return Articulo;
        }

    }
}
