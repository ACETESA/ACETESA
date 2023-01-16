using System.Collections.Generic;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Entity;
using System.Data;
using System.Data.SqlClient;

namespace Acetesa.TomaPedidos.Repository
{
    public class LiquidacionGastosRepository : ILiquidacionGastosRepository
    {

        public List<LiquidacionGastos> getListaLiquidacionGastos(string correo)
        {
            LiquidacionGastos liquidacionGastos = new LiquidacionGastos();
            List<LiquidacionGastos> ListaLiquidacionGastos = new List<LiquidacionGastos>();
            using (var oConex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var ocompa = new SqlCommand("[web].[spListarLiquidacionGatos]", oConex))
                {
                    ocompa.CommandType = CommandType.StoredProcedure;
                    oConex.Open();
                    ocompa.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@correo",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = correo
                    });
                    using (var dr = ocompa.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            liquidacionGastos.LiquidacionGastosID = int.Parse(dr["LiquidacionGastosID"].ToString());
                            liquidacionGastos.AlternoID = dr["AlternoID"].ToString();
                            liquidacionGastos.Asunto = dr["Asunto"].ToString();
                            liquidacionGastos.Destino = dr["Destino"].ToString();
                            liquidacionGastos.FechaInicioViaje = dr["FechaInicioViaje"].ToString();
                            liquidacionGastos.GastoTotal = decimal.Parse(dr["GastoTotal"].ToString());

                            ListaLiquidacionGastos.Add(liquidacionGastos);
                        }
                    }

                    oConex.Close();
                }
            }
            return ListaLiquidacionGastos;
        }

        public List<DetalleLiquidacionGastos> getRecuperarDetalleLiquidacionGastos(int id)
        {
            DetalleLiquidacionGastos detalleLiquidacionGastos = new DetalleLiquidacionGastos();
            List<DetalleLiquidacionGastos> ListaDetalleLiquidacionGastos = new List<DetalleLiquidacionGastos>();
            using (var oConex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var ocompa = new SqlCommand("[web].[spRecuperarDetalleLiquidacionGastos]", oConex))
                {
                    ocompa.CommandType = CommandType.StoredProcedure;
                    oConex.Open();
                    ocompa.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@LiquidacionGastosID",
                        SqlDbType = SqlDbType.Int,
                        Value = id
                    });
                    using (var dr = ocompa.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            detalleLiquidacionGastos.fechaEvento = dr["FechaEvento"].ToString();
                            detalleLiquidacionGastos.descripcion = dr["Descripcion"].ToString();
                            detalleLiquidacionGastos.ruc = dr["RUC"].ToString();
                            detalleLiquidacionGastos.razonSocial = dr["RazonSocial"].ToString();
                            detalleLiquidacionGastos.numeroFactura = dr["NumeroFactura"].ToString();
                            detalleLiquidacionGastos.tipoGastoID = (int)dr["TipoGastoID"];
                            detalleLiquidacionGastos.montoFactura = (decimal)dr["MontoFactura"];
                            detalleLiquidacionGastos.documento = (byte[])dr["Documento"];

                            ListaDetalleLiquidacionGastos.Add(detalleLiquidacionGastos);
                        }
                    }

                    oConex.Close();
                }
            }
            return ListaDetalleLiquidacionGastos;
        }

        public List<LiquidacionGastos> getRecuperarLiquidacionGastos(int id)
        {
            LiquidacionGastos liquidacionGastos = new LiquidacionGastos();
            List<LiquidacionGastos> ListaLiquidacionGastos = new List<LiquidacionGastos>();
            using (var oConex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var ocompa = new SqlCommand("[web].[spRecuperarLiquidacionGastos]", oConex))
                {
                    ocompa.CommandType = CommandType.StoredProcedure;
                    oConex.Open();
                    ocompa.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@LiquidacionGastosID",
                        SqlDbType = SqlDbType.Int,
                        Value = id
                    });
                    using (var dr = ocompa.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            liquidacionGastos.Asunto = dr["Asunto"].ToString();
                            liquidacionGastos.Destino = dr["Destino"].ToString();
                            liquidacionGastos.Observaciones = dr["Observaciones"].ToString();
                            liquidacionGastos.FechaInicioViaje = dr["FechaInicioViaje"].ToString();
                            liquidacionGastos.FechaFinViaje = dr["FechaFinViaje"].ToString();
                            liquidacionGastos.Viaticos = (int)dr["MontoViaticosEntregado"];

                            ListaLiquidacionGastos.Add(liquidacionGastos);
                        }
                    }

                    oConex.Close();
                }
            }
            return ListaLiquidacionGastos;
        }


    }
}
