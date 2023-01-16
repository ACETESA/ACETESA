﻿using System;
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
                    //ct_email = s.ct_email,
                    //cn_telf1 = s.cn_telf1
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

            //var query = _dbContext.Query<MCLIENTE>()
            //    .Select(s => new ClienteModel
            //    {
            //        cc_analis = s.cc_analis,
            //        ct_email = s.ct_email,
            //        cd_razsoc = s.cd_razsoc
            //    }).FirstOrDefault(x => x.cc_analis == ccAnalis.Trim());
            //return query;
        }

        //public void UpdateEmailByCodigo(string ccAnalis, string email)
        //{
        //    var cliente = _dbContext.Query<MCLIENTE>()
        //        .FirstOrDefault(x => x.cc_analis == ccAnalis.Trim());
        //    if (cliente == null) return;
        //    if (string.IsNullOrEmpty(cliente.ct_email) || string.IsNullOrWhiteSpace(cliente.ct_email))
        //    {
        //        cliente.ct_email = email;
        //    }
        //}

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
            //if (ExisteContactoEntregaDirecta(entity))
            //{
            //    throw new ApplicationException("Dirección ya existente");
            //}
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
        //public static bool ExisteContactoEntregaDirecta(dynamic entity)
        //{
        //    long? item = -1;
        //    using (var oMapper = new Mapper())
        //    {
        //        using (var oComando = Mapper.getComando("EXISTE_LUGARENTREGA"))
        //        {
        //            oComando.setParamValue("sAnalisis", entity.Analisis);
        //            oComando.setParamValue("sDireccion", entity.Direccion);

        //            item = oMapper.BuscarPrimero<long>(oComando);
        //        }
        //    }
        //    if (item != null && item.Value > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
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
            lista = _dbContext.GetExecSpEnumerable<ClienteModel.VendedorCliente>("spClientesAsignadosAlVendedor", sqlParams).ToList();
            return lista;
        }
        public List<ClienteModel> ClientesActivos()
        {

            List<ClienteModel> lista = new List<ClienteModel>();
            lista = _dbContext.GetExecSpEnumerable<ClienteModel>("spTodosClientesActivos").ToList();
            return lista;
        }
        //NUEVA OPCION ALTERNATIVA NO FUNCIONAL - ANTIGUO "vparco
        //public string NuevoCliente(MCLIENTE cliente)
        //{
        //    object[] sqlParams =
        //    {
        //        new SqlParameter
        //        {
        //            ParameterName = "@cc_analis",
        //            SqlDbType = SqlDbType.VarChar,
        //            Size = 50,
        //            Value = cliente.cc_analis
        //        },
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cc_pais",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 2,
        //        //    Value = cliente.cc_pais
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cc_dpto",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 2,
        //        //    Value = cliente.cc_dpto
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cc_prov",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 2,
        //        //    Value = cliente.cc_prov
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cc_sector",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 2,
        //        //    Value = cliente.cc_sector
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cc_catclie",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 2,
        //        //    Value = cliente.cc_catclie
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cc_distrito",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 2,
        //        //    Value = cliente.cc_distrito
        //        //},
        //        new SqlParameter
        //        {
        //            ParameterName = "@cd_razsoc",
        //            SqlDbType = SqlDbType.VarChar,
        //            Size = 250,
        //            Value = cliente.cd_razsoc
        //        },
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cc_zona",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 2,
        //        //    Value = cliente.cc_zona
        //        //},
        //        new SqlParameter
        //        {
        //            ParameterName = "@cd_direc",
        //            SqlDbType = SqlDbType.VarChar,
        //            Size = 45,
        //            Value = cliente.cd_direc
        //        },
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cn_regind",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 15,
        //        //    Value = cliente.cn_regind
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cn_sanit",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 15,
        //        //    Value = cliente.cn_sanit
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cn_regmerc",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 15,
        //        //    Value = cliente.cn_regmerc
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@ct_giro",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 85,
        //        //    Value = cliente.ct_giro
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@dt_constit",
        //        //    SqlDbType = SqlDbType.DateTime,
        //        //    Value = cliente.dt_constit
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@dt_registro",
        //        //    SqlDbType = SqlDbType.DateTime,
        //        //    Value = cliente.dt_registro
        //        //},
        //        new SqlParameter
        //        {
        //            ParameterName = "@cn_telf1",
        //            SqlDbType = SqlDbType.VarChar,
        //            Size = 15,
        //            Value = cliente.cn_telf1
        //        },
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cn_telf2",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 15,
        //        //    Value = cliente.cn_telf2
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cn_telf3",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 15,
        //        //    Value = cliente.cn_telf3
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cb_cheque",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 1,
        //        //    Value = cliente.cb_cheque
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cb_sector",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 1,
        //        //    Value = cliente.cb_sector
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@dt_ultcomp",
        //            //SqlDbType = SqlDbType.DateTime,
        //            //Value = cliente.dt_ultcomp
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@ct_legal",
        //            //SqlDbType = SqlDbType.VarChar,
        //            //Size = 45,
        //            //Value = cliente.ct_legal
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@cn_rucleg",
        //            //SqlDbType = SqlDbType.VarChar,
        //            //Size = 11,
        //            //Value = cliente.cn_rucleg
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@dt_ultdeuda",
        //            //SqlDbType = SqlDbType.DateTime,
        //            //Value = cliente.dt_ultdeuda
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@fm_compmn",
        //            //SqlDbType = SqlDbType.Float,
        //            //Value = cliente.fm_compmn
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@cd_direcleg",
        //            //SqlDbType = SqlDbType.VarChar,
        //            //Size = 45,
        //            //Value = cliente.cd_direcleg
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@fm_compme",
        //            //SqlDbType = SqlDbType.Float,
        //            //Value = cliente.fm_compme
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@cn_telfleg",
        //            //SqlDbType = SqlDbType.VarChar,
        //            //Size = 10,
        //            //Value = cliente.cn_telfleg
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@fm_acummn",
        //            //SqlDbType = SqlDbType.Float,
        //            //Value = cliente.fm_acummn
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@cn_faxleg",
        //            //SqlDbType = SqlDbType.VarChar,
        //            //Size = 10,
        //            //Value = cliente.cn_faxleg
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@fm_acumme",
        //            //SqlDbType = SqlDbType.Float,
        //            //Value = cliente.fm_acumme
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@fm_saldomn",
        //            //SqlDbType = SqlDbType.Float,
        //            //Value = cliente.fm_saldomn
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@fm_saldome",
        //            //SqlDbType = SqlDbType.Float,
        //            //Value = cliente.fm_saldome
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@fq_descto",
        //            //SqlDbType = SqlDbType.Float,
        //            //Value = cliente.fq_descto
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@fm_cantporc",
        //            //SqlDbType = SqlDbType.Float,
        //            //Value = cliente.fm_cantporc
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@fm_desctomn",
        //            //SqlDbType = SqlDbType.Float,
        //            //Value = cliente.fm_desctomn
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@fm_montporcmn",
        //            //SqlDbType = SqlDbType.Float,
        //            //Value = cliente.fm_montporcmn
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@fm_desctome",
        //            //SqlDbType = SqlDbType.Float,
        //            //Value = cliente.fm_desctome
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@fm_montporcme",
        //            //SqlDbType = SqlDbType.Float,
        //            //Value = cliente.fm_montporcme
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@cb_cheqdif",
        //            //SqlDbType = SqlDbType.VarChar,
        //            //Size = 1,
        //            //Value = cliente.cb_cheqdif
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@fm_diferidomn",
        //            //SqlDbType = SqlDbType.Float,
        //            //Value = cliente.fm_diferidomn
        //        //},
        //        //new SqlParameter
        //        //{
        //            //ParameterName = "@fm_diferidome",
        //            //SqlDbType = SqlDbType.Float,
        //            //Value = cliente.fm_diferidome
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@fm_limcred",
        //        //    SqlDbType = SqlDbType.Money,
        //        //    Value = cliente.fm_limcred
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cd_nomcom",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 45,
        //        //    Value = cliente.cd_nomcom
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cd_appaterno",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 30,
        //        //    Value = cliente.cd_appaterno
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cd_apmaterno",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 330,
        //        //    Value = cliente.cd_apmaterno
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cd_nombre1",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 30,
        //        //    Value = cliente.cd_nombre1
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cd_nombre2",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 30,
        //        //    Value = cliente.cd_nombre2
        //        //},
        //        //new SqlParameter
        //        //{
        //        //    ParameterName = "@cc_proyecto",
        //        //    SqlDbType = SqlDbType.VarChar,
        //        //    Size = 1,
        //        //    Value = cliente.cc_proyecto
        //        //},
        //        new SqlParameter
        //        {
        //            ParameterName = "@ct_email",
        //            SqlDbType = SqlDbType.VarChar,
        //            Size = 225,
        //            Value = cliente.ct_email
        //        }

        //    };
        //    var resultado = _dbContext.GetExecSpEnumerable("spRegistrarNuevoCliente", sqlParams).ToList();
        //    return "ok";
        //}
        public Dictionary<int, string> NuevoCliente(MCLIENTE cliente, string emailUsuario) {
            Dictionary<int, string> diccionario1 = new Dictionary<int, string>();
            //int resultado=0;
            //string texto="";
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
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("sp_cat_cli_tsector", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        TSECTOR sector = new TSECTOR();
                        sector.cc_sector = reader["cc_sector"].ToString();
                        sector.cd_sector = reader["cd_sector"].ToString();
                        listaSector.Add(sector);
                    }
                }
            }
            return listaSector;
        }
        public List<UBIGEO> ListarDepartamento()
        {
            List<UBIGEO> listaDepartamento = new List<UBIGEO>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("sp_cat_cli_ay_dpto", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        UBIGEO departamento = new UBIGEO();
                        departamento.cc_dpto = reader["cc_dpto"].ToString();
                        departamento.cd_dpto = reader["cd_dpto"].ToString();
                        listaDepartamento.Add(departamento);
                    }
                }
            }
            return listaDepartamento;
        }
        public List<UBIGEO> ListarProvincia(string cc_dpto)
        {
            List<UBIGEO> listaProvincia = new List<UBIGEO>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("sp_cat_cli_ay_prov", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cc_dpto", cc_dpto);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        UBIGEO provincia = new UBIGEO();
                        provincia.cc_prov = reader["cc_prov"].ToString();
                        provincia.cd_prov = reader["cd_prov"].ToString();
                        listaProvincia.Add(provincia);
                    }
                }
            }
            return listaProvincia;
        }
        public List<UBIGEO> ListarDistrito(string cc_dpto, string cc_prov)
        {
            List<UBIGEO> listaDistrito = new List<UBIGEO>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("sp_cat_cli_ay_distrito", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cc_dpto", cc_dpto);
                sqlCommand.Parameters.AddWithValue("@cc_prov", cc_prov);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        UBIGEO distrito = new UBIGEO();
                        distrito.cc_distrito = reader["cc_distrito"].ToString();
                        distrito.cd_distrito = reader["cd_distrito"].ToString();
                        listaDistrito.Add(distrito);
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

        public List<CarteraCliente> ClientesAsignadosLibres(string correoVendedor)
        {
            List<CarteraCliente> listaCartera = new List<CarteraCliente>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("spCarteraClientesAsignadosLibres", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@CorreoVendedor", correoVendedor);
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
                        //cc.MontoLimiteCoberturado =  (decimal)reader["MontoLimiteCoberturado"];
                        //cc.MontoLimiteDiscrecional = (decimal)reader["MontoLimiteDiscrecional"];
                        //cc.MontoLimiteInterno = (decimal)reader["MontoLimiteInterno"];
                        cc.MontoTotalLimite = (decimal)reader["MontoTotalLimite"];
                        //cc.FechaUltVenta = (string)reader["FechaUltVenta"];
                        //cc.MontoDeuda = (decimal)reader["MontoDeuda"];
                        cc.Aseguradora = reader["Aseguradora"].ToString();

                        listaCartera.Add(cc);
                    }
                }
            }
            return listaCartera;
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

    }
}
