using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acetesa.TomaPedidos.Entity;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Transversal;
using System.Data;
using System.Data.SqlClient;
using Acetesa.TomaPedidos.Domain;
using System.Configuration;

namespace Acetesa.TomaPedidos.Repository
{
    public class VentaRepository : BaseRepository<LCPEDIDO_WEB>, IVentaRepository
    {
        private readonly IDbContext _dbContext;

        public VentaRepository(IDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Dictionary<string, object> GetVentaStoreProcedure(DateTime fechaInicio, DateTime fechaFinal, string cliente, string vendedor)
        {
            if (string.IsNullOrEmpty(cliente) || string.IsNullOrWhiteSpace(cliente))
            {
                cliente = "%";
            }
            object[] sqlParams =
            {
                new SqlParameter
                {
                    ParameterName = "@FechaInicio",
                    SqlDbType = SqlDbType.DateTime,
                    Value = fechaInicio
                },
                new SqlParameter
                {
                    ParameterName = "@FechaFin",
                    SqlDbType = SqlDbType.DateTime,
                    Value = fechaFinal
                },
                new SqlParameter
                {
                    ParameterName = "@cliente",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 11,
                    Value = cliente.Trim()
                },
                new SqlParameter
                {
                     ParameterName = "@Vendedor",
                    SqlDbType = SqlDbType.Char,
                    Size = 50,
                    Value = vendedor.Trim()
                }
            };

            var Resultados = new Dictionary<string, object>();
            var query = new List<Dictionary<string, object>>();
            var cabeceras = new List<string>();
            using (var oConex = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var ocompa = new System.Data.SqlClient.SqlCommand("usp_VentaPorVendedorCliente", oConex))
                {
                    ocompa.CommandType = CommandType.StoredProcedure;
                    oConex.Open();
                    ocompa.Parameters.AddRange(sqlParams);
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
                            var oDatoRow = new Dictionary<string, object>();

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

        public Dictionary<string,byte[]> RecuperarDocumentosPorComprobante(DocumentosModel documentosModel)
        {
            Dictionary<string, byte[]> diccionario = new Dictionary<string, byte[]>();

            var cn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var conexion = new SqlConnection(cn);
            using (conexion)
            {
                SqlCommand cmd = new SqlCommand("spRecuperarDocumentosPorComprobante", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@empresaEmisora", documentosModel.empresaEmisora);
                cmd.Parameters.AddWithValue("@idTipoDocumento", documentosModel.idTipoDocumento);
                cmd.Parameters.AddWithValue("@tipoDocRelacionado", documentosModel.tipoDocRelacionado);
                cmd.Parameters.AddWithValue("@serieDocRelacionado", documentosModel.serieDocRelacionado);
                cmd.Parameters.AddWithValue("@correlativoDocRelacionado", documentosModel.correlativoDocRelacionado);
                conexion.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nombre = (string)reader["nombre"];
                        byte[] archivo = (byte[])reader["documento"];
                        diccionario.Add(nombre, archivo);
                    }
                }

            }
            return diccionario;
        }
    }
}