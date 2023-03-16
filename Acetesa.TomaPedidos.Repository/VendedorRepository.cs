using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;
using Acetesa.TomaPedidos.Domain;
using System.Configuration;

namespace Acetesa.TomaPedidos.Repository
{
    public class VendedorRepository : IVendedorRepository
    {
        public static MVENDEDOR getVendedorCotizacion(LCPROF_WEB entityMaster)
        {
            MVENDEDOR oVendedor = new MVENDEDOR();
            using (var oConex = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var ocompa = new System.Data.SqlClient.SqlCommand("usp_web_getVendedor_cotizacion", oConex))
                {
                    ocompa.CommandType = CommandType.StoredProcedure;
                    oConex.Open();
                    ocompa.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@cn_proforma",
                        SqlDbType = SqlDbType.Char,
                        Size = 8,
                        Value = entityMaster.cn_proforma
                    });
                    using (var dr = ocompa.ExecuteReader()){
                        if (dr.Read())
                        {
                            oVendedor.cc_analisvend = dr["cc_analisvend"].ToString();
                            oVendedor.ct_nombres = dr["ct_nombres"].ToString();
                            oVendedor.ct_appaterno = dr["ct_appaterno"].ToString();
                            oVendedor.cn_telf = dr["cn_telf"].ToString();
                            oVendedor.cn_telfref = dr["cn_telfref"].ToString();
                            oVendedor.ct_email = dr["ct_email"].ToString();
                        }
                    }

                    oConex.Close();
                }
            }
            return oVendedor;
        }

        public static VendedorModel getVendedorPedido(string idPedido)
        {
            VendedorModel usuario = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_VENDEDORES_PEDIDO"))
                {
                    oComando.setParamValue("idPedido", idPedido);
                    usuario = oMapper.BuscarPrimero<VendedorModel>(oComando);
                }
            }
            return usuario;
        }
        public IEnumerable<VendedorModel> GetAll()
        {
            IEnumerable<VendedorModel> usuario = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_VENDEDORES"))
                {
                    usuario = oMapper.BuscarTodos<VendedorModel>(oComando);
                }
            }
            return usuario;
        }

        public VendedorModel GetByEmail(string ct_email)
        {
            VendedorModel usuario = null;
            using (var oMapper = new Mapper())
            {
                using (var oComando = Mapper.getComando("GET_VENDEDORES_MAIL"))
                {
                    oComando.setParamValue("ct_email", ct_email);
                    usuario = oMapper.BuscarPrimero<VendedorModel>(oComando);
                }
            }
            return usuario;
        }
        public Dictionary<string,string> ValidarVendedorJefe(string correoUsuario)
        {
            Dictionary<string, string> resultados = new Dictionary<string, string>();
            string resultado;
            var cn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var conexion = new SqlConnection(cn);
            using (conexion)
            {
                SqlCommand cmd = new SqlCommand("spValidarVendedorJefe", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@correo", correoUsuario);
                conexion.Open();
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow fila in dt.Rows)
                {
                    resultado =fila["resultado"].ToString();
                    resultados.Add("resultado", resultado);
                }
            }
            return resultados;
        }

        public VendedorModel.CorreoVendedor ObtenerCredencialesCorreoVendedor(string correoVendedor)
        {
            VendedorModel.CorreoVendedor CorreoVendedor = new VendedorModel.CorreoVendedor();

            string query = "[web].[uspObtenerCredencialesCorreoVendedor]";
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
                            CorreoVendedor.correo = reader["correo"].ToString();
                            CorreoVendedor.clave = reader["clave"].ToString();
                            CorreoVendedor.llave = reader["llave"].ToString();
                        }
                    }
                }
            }
            return CorreoVendedor;
        }

        public Dictionary<string, string> RegistrarCredencialesCorreoVendedor(string correoVendedor, string claveCorreo, string llaveClave)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();

            string query = "[web].[uspRegistrarCredencialesCorreoVendedor]";
            string connect = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@correoVendedor", correoVendedor);
                    cmd.Parameters.AddWithValue("@claveCorreo", claveCorreo);
                    cmd.Parameters.AddWithValue("@llaveClave", llaveClave);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            diccionario.Add("mensajeID", reader["mensajeID"].ToString());
                            diccionario.Add("mensaje", reader["mensaje"].ToString());
                        }
                    }
                }
            }
            return diccionario;
        }
    }
}
