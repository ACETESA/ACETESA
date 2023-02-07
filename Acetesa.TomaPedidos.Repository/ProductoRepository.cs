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
    }
}
