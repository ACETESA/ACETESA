using System;
using System.Collections.Generic;
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
    public class ClienteRepository : BaseRepository<MCLIENTE>, IClienteRepository
    {
        private readonly IDbContext _dbContext;

        public ClienteRepository(IDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<ClienteModel> GetAll()
        {
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cc_zona",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    Value = "%"
                },
                new SqlParameter
                {
                    ParameterName = "@cc_dpto",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    Value = "%"
                },
                new SqlParameter
                {
                    ParameterName = "@cc_prov",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    Value = "%"
                },
                new SqlParameter
                {
                    ParameterName = "@cc_distrito",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 2,
                    Value = "%"
                }
            };
            var query = _dbContext.GetExecSpEnumerable<ClienteModel>("sp_creditos_ay_clientes_x_zona", sqlParams);
            return query;
        }

        public ClienteModel GetByCodigo(string ccAnalis)
        {
            var query = _dbContext.Query<MCLIENTE>()
                .Select(s => new ClienteModel
                {
                    cc_tipana = s.cc_tipana,
                    cc_analis = s.cc_analis,
                    cd_razsoc = s.cd_razsoc,
                    cd_direc = s.cd_direc,
                }).FirstOrDefault(x => x.cc_analis == ccAnalis.Trim());
            return query;
        }

        public ClienteModel GetByRazSoc(string cdRazSoc)
        {
            var query = _dbContext.Query<MCLIENTE>()
               .Select(s => new ClienteModel
               {
                   cc_tipana = s.cc_tipana,
                   cc_analis = s.cc_analis,
                   cd_razsoc = s.cd_razsoc
               }).FirstOrDefault(x => x.cd_razsoc == cdRazSoc.Trim());
            return query;
        }

        public ClienteModel GetEmailByCodigo(int tipoMail, string ccAnalis, string cn_contacto, string cn_suc)
        {
            ClienteModel cliente = new ClienteModel();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("spObtenerMailPorCliente", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@tipoMail", tipoMail);
                sqlCommand.Parameters.AddWithValue("@cc_analis", ccAnalis);
                sqlCommand.Parameters.AddWithValue("@cn_contacto", cn_contacto);
                sqlCommand.Parameters.AddWithValue("@cn_suc", cn_suc);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        cliente.cc_analis = reader["cc_analis"].ToString();
                        cliente.ct_email = reader["ct_email"].ToString();
                        cliente.cd_razsoc = reader["cd_razsoc"].ToString();
                    }
                }
            }
            return cliente;

        }

        
        public void ActualizarMailContacto(int tipoMail, string id, string emailPara)
        {
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("spActualizarMailContacto", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@tipoMail", tipoMail);
                sqlCommand.Parameters.AddWithValue("@id", id);
                sqlCommand.Parameters.AddWithValue("@emailPara", emailPara);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteNonQuery();
            }
        }

        public IQueryable<ClienteModel> GetByRazSocOrRuc(string param)
        {
            var query = _dbContext.Query<MCLIENTE>()
                .Where(x => (x.cd_razsoc.Contains(param.Trim()) || x.cc_analis.Contains(param.Trim())) && x.cb_activo == "1")
               .Select(s => new ClienteModel
               {
                   cc_tipana = s.cc_tipana,
                   cc_analis = s.cc_analis,
                   cd_razsoc = s.cd_razsoc
               });
            return query;
        }
        public IEnumerable<ContactoListadoModel> GetContactoEntregaDirectaByccAnalis(string ccAnalis)
        {
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@cc_analis",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 11,
                    Value = ccAnalis.Trim()
                }
            };
            var query = _dbContext.GetExecSpEnumerable<ContactoListadoModel>("sp_listarContactoEntregaDirecta", sqlParams);
            return query;
        }
        public static IEnumerable<object> GetContactoEntregaDirectaClienteTodos(string ccAnalis)
        {
            IEnumerable<object> usuario = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_LISTA_CONTACTOENTREGADIRECTA"))
                {
                    oComando.setParamValue("cc_analis", ccAnalis);
                    usuario = oMapper.BuscarTodos<object>(oComando);
                }
            }
            return usuario;
        }
        public static string addContactoEntregaDirecta(dynamic entity)
        {
            string codigo = "0";
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("ADD_CONTACTOENTREGADIRECTA"))
                {
                    oComando.setParamValue("cc_analis", entity.Analisis);
                    oComando.setParamValue("cn_suc", entity.Surcursal);
                    oComando.setParamValue("ct_nombres", entity.Nombres);
                    oComando.setParamValue("cn_telf1", entity.Telefono);
                    oComando.setParamValue("cn_telf2", entity.Telefono2);
                    oComando.setParamValue("cd_email", entity.Email);
                    oComando.setParamValue("ContVenta", entity.ContVenta);
                    oComando.setParamValue("ContCobranza", entity.ContCobranza);
                    oComando.setParamValue("ContAlmacen", entity.ContAlmacen);
                    oComando.setParamValue("CargoLaboral", entity.CargoLaboral);
                    oComando.setParamValue("EnvioDocs", entity.EnvioDocs);

                    codigo = oMapper.BuscarPrimero<string>(oComando);
                }
            }
            return codigo;
        }

        public List<ClienteModel.VendedorCliente> VendedorAsignadoPorCliente(string ClienteID)
        {
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@ClienteID",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = ClienteID
                }
            };
            List<ClienteModel.VendedorCliente> lista = new List<ClienteModel.VendedorCliente>();
            lista = _dbContext.GetExecSpEnumerable<ClienteModel.VendedorCliente>("[web].[uspVendedorAsignadoPorClienteID]", sqlParams).ToList();
            return lista;
        }
        public List<ClienteModel> ClientesActivos()
        {
            List<ClienteModel> lista = new List<ClienteModel>();
            lista = _dbContext.GetExecSpEnumerable<ClienteModel>("[web].[uspSelectTodosClientesActivos]").ToList();
            return lista;
        }

        public Dictionary<int, string> NuevoCliente(MCLIENTE cliente, string emailUsuario) {
            Dictionary<int, string> diccionario1 = new Dictionary<int, string>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("spRegistrarNuevoCliente", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cc_analis", cliente.cc_analis);
                sqlCommand.Parameters.AddWithValue("@cd_razsoc", cliente.cd_razsoc);
                sqlCommand.Parameters.AddWithValue("@cd_direc", cliente.cd_direc);
                sqlCommand.Parameters.AddWithValue("@cn_telf1", cliente.cn_telf1);
                sqlCommand.Parameters.AddWithValue("@ct_email", cliente.ct_email);
                sqlCommand.Parameters.AddWithValue("@emailUsuario", emailUsuario);
                sqlCommand.Parameters.AddWithValue("@cc_sector", cliente.cc_sector);
                sqlCommand.Parameters.AddWithValue("@cc_dpto", cliente.cc_dpto);
                sqlCommand.Parameters.AddWithValue("@cc_prov", cliente.cc_prov);
                sqlCommand.Parameters.AddWithValue("@cc_distrito", cliente.cc_distrito);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read()) {
                        diccionario1.Add(reader.GetInt32(1), reader.GetString(0));
                    }
                }
            }
            return diccionario1;
        }
        public List<TSECTOR> ListarSector()
        {
            List<TSECTOR> listaSector = new List<TSECTOR>();

            string query = "[dbo].[sp_cat_cli_tsector]";
            string connect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            listaSector.Add(
                                new TSECTOR
                                {
                                    cc_sector = reader["cc_sector"].ToString(),
                                    cd_sector = reader["cd_sector"].ToString()
                                }
                                );
                        }
                    }
                }
            }
            return listaSector;
        }

        public List<UBIGEO> ListarDepartamento()
        {
            List<UBIGEO> listaDepartamentos = new List<UBIGEO>();

            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("dbo.sp_cat_cli_ay_dpto", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        UBIGEO Ubigeo = new UBIGEO();
                        Ubigeo.cc_dpto = reader["cc_dpto"].ToString();
                        Ubigeo.cd_dpto = reader["cd_dpto"].ToString();
                        listaDepartamentos.Add(Ubigeo);
                    }
                }
                sqlConnection.Close();
            }
            return listaDepartamentos;
        }

        public List<UBIGEO> ListarProvincia(string cc_dpto)
        {
            List<UBIGEO> listaUbigeo = new List<UBIGEO>();

            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("dbo.sp_cat_cli_ay_prov", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cc_dpto", cc_dpto);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        UBIGEO Ubigeo = new UBIGEO();
                        Ubigeo.cc_prov = reader["cc_prov"].ToString();
                        Ubigeo.cd_prov = reader["cd_prov"].ToString();
                        listaUbigeo.Add(Ubigeo);
                    }
                }
                sqlConnection.Close();
            }
            return listaUbigeo;
        }

        public List<UBIGEO> ListarDistrito(string cc_dpto, string cc_prov)
        {
            List<UBIGEO> listaDistrito = new List<UBIGEO>();

            string query = "[dbo].[sp_cat_cli_ay_distrito]";
            string connect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cc_dpto", cc_dpto);
                    cmd.Parameters.AddWithValue("@cc_prov", cc_prov);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            listaDistrito.Add(
                                new UBIGEO
                                {
                                    cc_distrito = reader["cc_distrito"].ToString(),
                                    cd_distrito = reader["cd_distrito"].ToString()
                                }
                                );
                        }
                    }
                }
            }
            return listaDistrito;
        }

        public Dictionary<string,string> ValidarRelacionVendedorCliente(string cc_analis, string emailUsuario)
        {
            Dictionary<string, string> diccionario1 = new Dictionary<string, string>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("spValidarRelacionVendedorCliente", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cc_analis", cc_analis);
                sqlCommand.Parameters.AddWithValue("@emailUsuario", emailUsuario);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        diccionario1.Add(reader["mostrar"].ToString(), reader["mensaje"].ToString());
                    }
                }
            }
            return diccionario1;
        }

        public Dictionary<string, string> ValidarExistenciaClientePorRUC(string cc_analis)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("spValidarExistenciaClientePorRuc", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cc_analis", cc_analis);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        diccionario.Add("error", reader["error"].ToString());
                        diccionario.Add("mensaje", reader["mensaje"].ToString());
                    }
                }
            }
            return diccionario;
        }

        public List<CarteraCliente> ClientesAsignadosLibres(string correoVendedor, string EsAsignado)
        {
            List<CarteraCliente> listaCartera = new List<CarteraCliente>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("spCarteraClientesAsignadosLibres", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@CorreoVendedor", correoVendedor);
                sqlCommand.Parameters.AddWithValue("@EsAsignado", EsAsignado);
                sqlCommand.CommandTimeout=0;
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        CarteraCliente cc = new CarteraCliente();
                        cc.Ruc = reader["Ruc"].ToString();
                        cc.RazonSocial = reader["RazonSocial"].ToString();
                        cc.Sector = reader["Sector"].ToString();
                        cc.Departamento = reader["Departamento"].ToString();
                        cc.Provincia = reader["Provincia"].ToString();
                        cc.Distrito = reader["Distrito"].ToString();
                        cc.Asignado = (reader["Asignado"].ToString().Equals("1") ? true : false);
                        cc.MontoTotalLimite = (decimal)reader["MontoTotalLimite"];
                        cc.Aseguradora = reader["Aseguradora"].ToString();

                        listaCartera.Add(cc);
                    }
                }
            }
            return listaCartera;
        }

        public List<ClienteModel> SelectClientesSegunCarteraVendedor(string correoVendedor)
        {
            List<ClienteModel> ListaClientes = new List<ClienteModel>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("web.spSelectClientesSegunCarteraVendedor", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@correoVendedor", correoVendedor);
                sqlCommand.CommandTimeout = 0;
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        ClienteModel c = new ClienteModel();
                        c.cc_analis = reader["Ruc"].ToString();
                        c.cd_razsoc = reader["RazonSocial"].ToString();

                        ListaClientes.Add(c);
                    }
                }
            }
            return ListaClientes;
        }

        public List<CarteraCliente> CarteraClientesAsignados(string correoVendedor ,string departamentoId, string provinciaId, string distritoId)
        {

            List<CarteraCliente> listaCartera = new List<CarteraCliente>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("spCarteraClientesAsignados", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@CorreoVendedor", correoVendedor);
                sqlCommand.Parameters.AddWithValue("@departamentoId", departamentoId);
                sqlCommand.Parameters.AddWithValue("@provinciaId", provinciaId);
                sqlCommand.Parameters.AddWithValue("@distritoId", distritoId);
                sqlCommand.CommandTimeout = 0;
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        CarteraCliente cc = new CarteraCliente();
                        cc.Ruc = reader["Ruc"].ToString();
                        cc.RazonSocial = reader["RazonSocial"].ToString();

                        listaCartera.Add(cc);
                    }
                }
            }
            return listaCartera;
        }


        public Dictionary<string, string> ActualizarAsignacionClienteVendedor(string rucCliente, string correoVendedor, bool asignar)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("spActualizarAsignacionClienteVendedor", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@rucCliente", rucCliente);
                sqlCommand.Parameters.AddWithValue("@CorreoVendedor", correoVendedor);
                sqlCommand.Parameters.AddWithValue("@asignar", asignar);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        diccionario.Add("error", reader["error"].ToString());
                        diccionario.Add("mensaje", reader["mensaje"].ToString());
                    }
                }
            }
            return diccionario;
        }

        public string ValidarClienteEnZonaLiberada(string ruc)
        {
            string zonaLiberada="";
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("spValidarClienteZonaLiberada", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cc_analis", ruc);
                sqlCommand.CommandTimeout = 0;
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        zonaLiberada = reader["zonaLiberada"].ToString();
                    }
                }
            }
            return zonaLiberada;
        }

        public List<ClienteModel> SelectClientesSegunCarteraVendedorYLibres(string correoVendedor)
        {
            List<ClienteModel> listaClientes = new List<ClienteModel>();

            string query = "[web].[uspSelectClientesSegunCarteraVendedorYLibres]";
            string connect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@correoVendedor", correoVendedor);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            listaClientes.Add(
                                new ClienteModel
                                {
                                    cc_analis = reader["Ruc"].ToString(),
                                    cd_razsoc = reader["RazonSocial"].ToString()
                                }
                                );
                        }
                    }
                }
            }
            return listaClientes;
        }

        public List<TDOCUMENTOIDENTIDAD> SelectTipoDocumentoIdentidad()
        {
            List<TDOCUMENTOIDENTIDAD> listaTipoDocumento = new List<TDOCUMENTOIDENTIDAD>();

            string query = "web.spSelectTipoDocumentoIdentidad";
            string connect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            listaTipoDocumento.Add(
                                new TDOCUMENTOIDENTIDAD
                                {
                                    TipoDocumentoID = reader["TipoDocumentoID"].ToString(),
                                    TipoDocumento = reader["TipoDocumento"].ToString()
                                }
                                );
                        }
                    }
                }
            }
            return listaTipoDocumento;
        }

        public List<UBIGEO> ListarPaises()
        {
            List<UBIGEO> listaPaises = new List<UBIGEO>();

            string query = "[web].[spListaSelectPaises]";
            string connect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            listaPaises.Add(new UBIGEO
                            {
                                cc_pais = reader["paisID"].ToString(),
                                cd_pais = reader["pais"].ToString()
                            });
                        }
                    }
                }
            }
            return listaPaises;
        }

        public List<TZONA> ListarZonas(string cc_distrito, string cc_dpto, string cc_prov)
        {
            List<TZONA> listaZona = new List<TZONA>();

            string query = "[web].[spListaSelectZonas]";
            string connect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cc_distrito", cc_distrito);
                    cmd.Parameters.AddWithValue("@cc_dpto", cc_dpto);
                    cmd.Parameters.AddWithValue("@cc_prov", cc_prov);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            listaZona.Add(new TZONA
                            {
                                cc_zona = reader["cc_zona"].ToString(),
                                cd_zona = reader["cd_zona"].ToString()
                            });
                        }
                    }
                }
            }
            return listaZona;
        }
        public List<TCATCLIE> ListarCategorias()
        {
            List<TCATCLIE> listaCategoria = new List<TCATCLIE>();

            string query = "[web].[spListaSelectCategoria]";
            string connect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            listaCategoria.Add(new TCATCLIE
                            {
                                cc_catclie = reader["cc_catclie"].ToString(),
                                cd_catclie = reader["cd_catclie"].ToString()
                            });
                        }
                    }
                }
            }
            return listaCategoria;
        }

        public List<Dictionary<string,string>> ListarEstadosCliente()
        {
            List<Dictionary<string, string>> listaCategoria = new List<Dictionary<string, string>>();

            string query = "[web].[spListaSelectEstadoCliente]";
            string connect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            listaCategoria.Add(new Dictionary<string, string>
                            {
                                {"estadoID" , reader["estadoID"].ToString()},
                                {"estado" , reader["estado"].ToString() }
                            });
                        }
                    }
                }
            }
            return listaCategoria;
        }

        public Dictionary<string,string> RegistrarCliente(MCLIENTE cliente)
        {
            Dictionary<string, string> respuesta = new Dictionary<string, string>();

            string query = "[web].[spRegistrarNuevoCliente]";
            string connect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cc_analis", cliente.cc_analis);
                    cmd.Parameters.AddWithValue("@cc_pais", cliente.cc_pais);
                    cmd.Parameters.AddWithValue("@cc_dpto", cliente.cc_dpto);
                    cmd.Parameters.AddWithValue("@cc_prov", cliente.cc_prov);
                    cmd.Parameters.AddWithValue("@cc_sector", cliente.cc_sector);
                    cmd.Parameters.AddWithValue("@cc_catclie", cliente.cc_catclie);
                    cmd.Parameters.AddWithValue("@cc_distrito", cliente.cc_distrito);
                    cmd.Parameters.AddWithValue("@cd_razsoc", cliente.cd_razsoc);
                    cmd.Parameters.AddWithValue("@cc_zona", cliente.cc_zona);
                    cmd.Parameters.AddWithValue("@cb_proced", cliente.cb_proced);
                    cmd.Parameters.AddWithValue("@cd_direc", cliente.cd_direc);
                    cmd.Parameters.AddWithValue("@ct_giro", cliente.ct_giro);
                    cmd.Parameters.AddWithValue("@dt_constit", cliente.dt_constit);
                    cmd.Parameters.AddWithValue("@cb_monfac", cliente.cb_monfac);
                    cmd.Parameters.AddWithValue("@cb_sucursal", cliente.cb_sucursal);
                    cmd.Parameters.AddWithValue("@cb_sector", cliente.cb_sector);
                    cmd.Parameters.AddWithValue("@cb_activo", cliente.cb_activo);
                    cmd.Parameters.AddWithValue("@cd_nomcom", cliente.cd_nomcom);
                    cmd.Parameters.AddWithValue("@cd_appaterno", cliente.cd_appaterno);
                    cmd.Parameters.AddWithValue("@cd_apmaterno", cliente.cd_apmaterno);
                    cmd.Parameters.AddWithValue("@cd_nombre1", cliente.cd_nombre1);
                    cmd.Parameters.AddWithValue("@cd_nombre2", cliente.cd_nombre2);
                    cmd.Parameters.AddWithValue("@c_fl_agente_percepcion", cliente.c_fl_agente_percepcion);
                    cmd.Parameters.AddWithValue("@c_cod_documento_identidad", cliente.c_cod_documento_identidad);
                    cmd.Parameters.AddWithValue("@c_fl_vinculacion", cliente.c_fl_vinculacion);
                    cmd.Parameters.AddWithValue("@Corporativo", cliente.Corporativo);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            respuesta.Add("mensajeID", reader["mensajeID"].ToString());
                            respuesta.Add("mensaje", reader["mensaje"].ToString());
                        }
                    }
                }
            }
            return respuesta;
        }

    }
}
