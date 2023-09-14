using Acetesa.TomaPedidos.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acetesa.TomaPedidos.Repository
{
    public class ContactoSucursalRepository
    {
        public List<TCONTACLIE> ListarContactoPorSucursal(string cn_suc, string cc_analis)
        {
            List<TCONTACLIE> lista = new List<TCONTACLIE>();
            var connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connection))
            {
                SqlCommand sqlCommand = new SqlCommand("spContactoPorSucursal", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@cn_suc", cn_suc);
                sqlCommand.Parameters.AddWithValue("@cc_analis", cc_analis);
                sqlConnection.Open();

                var reader = sqlCommand.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        TCONTACLIE Contacto = new TCONTACLIE();
                        Contacto.cn_contacto = reader["cn_contacto"].ToString();
                        Contacto.cd_contacto = reader["cd_contacto"].ToString();

                        lista.Add(Contacto);

                    }
                }
                sqlConnection.Close();
            }
            return lista;
        }

        public List<TCONTACLIE> GetContactoParaEditar(string cc_analis, string cn_suc, string cn_contacto)
        {
            List<TCONTACLIE> lista = new List<TCONTACLIE>();
            var cn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var conexion = new SqlConnection(cn);
            using (conexion)
            {
                SqlCommand cmd = new SqlCommand("spObtenerDatosContacto", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cc_analis", cc_analis);
                cmd.Parameters.AddWithValue("@cn_suc", cn_suc);
                cmd.Parameters.AddWithValue("@cn_contacto", cn_contacto);
                conexion.Open();
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow fila in dt.Rows)
                {
                    TCONTACLIE c = new TCONTACLIE();
                    c.ct_nombres = fila["ct_nombres"].ToString();
                    //c.ct_appaterno = fila["ct_appaterno"].ToString();
                    //c.ct_apmaterno = fila["ct_apmaterno"].ToString();
                    c.cd_cargo_laboral = fila["CargoLaboral"].ToString();
                    c.cn_telf1 = fila["cn_telf1"].ToString();
                    c.cn_telf2 = fila["cn_telf2"].ToString();
                    c.cd_email = fila["cd_email"].ToString();
                    c.cbContVenta = Convert.ToBoolean(Convert.ToInt32(fila["ContVenta"].ToString()));
                    c.cbContCobranza = Convert.ToBoolean(Convert.ToInt32(fila["ContCobranza"].ToString()));
                    c.cbContAlmacen = Convert.ToBoolean(Convert.ToInt32(fila["ContAlmacen"].ToString()));
                    c.cb_envio_docum = fila["EnvioDocs"].ToString();
                    lista.Add(c);
                }
            }
            return lista;
        }
        public void ActualizarContacto(List<TCONTACLIE> listaContacto)
        {
            List<TCONTACLIE> lc = listaContacto;
            var cn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var conexion = new SqlConnection(cn);
            using (conexion)
            {
                SqlCommand cmd = new SqlCommand("spActualizarDatosContacto", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cc_analis", listaContacto[0].cc_analis);
                cmd.Parameters.AddWithValue("@cn_suc", listaContacto[0].cn_suc);
                cmd.Parameters.AddWithValue("@cn_contacto", listaContacto[0].cn_contacto);
                cmd.Parameters.AddWithValue("@ct_nombres", listaContacto[0].ct_nombres);
                //cmd.Parameters.AddWithValue("@ct_appaterno", listaContacto[0].ct_appaterno);
                //cmd.Parameters.AddWithValue("@ct_apmaterno", listaContacto[0].ct_apmaterno);
                cmd.Parameters.AddWithValue("@CargoLaboral", listaContacto[0].cd_cargo_laboral);
                cmd.Parameters.AddWithValue("@cn_telf1", listaContacto[0].cn_telf1);
                cmd.Parameters.AddWithValue("@cn_telf2", listaContacto[0].cn_telf2);
                cmd.Parameters.AddWithValue("@cd_email", listaContacto[0].cd_email);
                cmd.Parameters.AddWithValue("@ContAlmacen", listaContacto[0].cbContAlmacen);
                cmd.Parameters.AddWithValue("@ContCobranza", listaContacto[0].cbContCobranza);
                cmd.Parameters.AddWithValue("@ContVenta", listaContacto[0].cbContVenta);
                cmd.Parameters.AddWithValue("@EnvioDocs", listaContacto[0].cb_envio_docum);
                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
