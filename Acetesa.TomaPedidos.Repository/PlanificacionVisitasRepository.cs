
using System.Collections.Generic;
using Acetesa.TomaPedidos.IRepository;
using Acetesa.TomaPedidos.Entity;
using System.Data;
using System.Data.SqlClient;
using Acetesa.TomaPedidos.Transversal;
using System;

namespace Acetesa.TomaPedidos.Repository
{
    public class PlanificacionVisitasRepository : IPlanificacionVisitasRepository
    {
        public List<PlanificacionVisita> getListaVisitasClientes(string correo, string ClienteID, string fechaInicio, string fechaFin, string estadoVisita,string planificacionID)
        {
            List<PlanificacionVisita> ListaPlanificacionVisita = new List<PlanificacionVisita>();
            using (var oConex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var sqlCommand = new SqlCommand("[dbo].[spListaVisitasClientes]", oConex))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    oConex.Open();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@correo", correo);
                    sqlCommand.Parameters.AddWithValue("@ClienteID", ClienteID);
                    sqlCommand.Parameters.AddWithValue("@fechaInicio", Convert.ToDateTime(fechaInicio));
                    sqlCommand.Parameters.AddWithValue("@fechaFin", Convert.ToDateTime(fechaFin));
                    sqlCommand.Parameters.AddWithValue("@estadoVisita", estadoVisita);
                    sqlCommand.Parameters.AddWithValue("@planificacionID", planificacionID);

                    var dr = sqlCommand.ExecuteReader();
                    using (dr)
                    {
                        while (dr.Read())
                        {
                            PlanificacionVisita planificacionVisita = new PlanificacionVisita();

                            planificacionVisita.PlanificacionID = int.Parse(dr["PlanificacionID"].ToString());
                            planificacionVisita.VisitaClienteID = int.Parse(dr["VisitaClienteID"].ToString());
                            planificacionVisita.Descripcion = dr["DescripcionPlanificacion"].ToString();
                            planificacionVisita.Cliente = dr["Cliente"].ToString();
                            planificacionVisita.FechaVisita = dr["FechaVisita"].ToString();
                            planificacionVisita.Estado = dr["Estado"].ToString();

                            ListaPlanificacionVisita.Add(planificacionVisita);

                        }
                    }

                    oConex.Close();
                }
            }
            return ListaPlanificacionVisita;
        }

        public List<PlanificacionVisita> ObtenerNumerosPlanificacionActivas()
        {
            List<PlanificacionVisita> listaPlanificacionActivas = new List<PlanificacionVisita>();
            using (var oConex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var sqlCommand = new SqlCommand("[dbo].[spObtenerNumerosPlanificacionActivas]", oConex))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    oConex.Open();

                    var reader = sqlCommand.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            PlanificacionVisita planificacion = new PlanificacionVisita();

                            planificacion.PlanificacionID = int.Parse(reader["PlanificacionID"].ToString());
                            planificacion.Descripcion = reader["Descripcion"].ToString();

                            listaPlanificacionActivas.Add(planificacion);

                        }
                    }

                    oConex.Close();
                }
            }
            return listaPlanificacionActivas;
        }


        public List<PlanificacionVisita> ObtenerNumerosPlanificacionNoActivas()
        {
            List<PlanificacionVisita> listaPlanificacionActivas = new List<PlanificacionVisita>();
            using (var oConex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var sqlCommand = new SqlCommand("[dbo].[spObtenerNumerosPlanificacionNoActivas]", oConex))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    oConex.Open();

                    var reader = sqlCommand.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            PlanificacionVisita planificacion = new PlanificacionVisita();

                            planificacion.PlanificacionID = int.Parse(reader["PlanificacionID"].ToString());
                            planificacion.Descripcion = reader["Descripcion"].ToString();

                            listaPlanificacionActivas.Add(planificacion);

                        }
                    }

                    oConex.Close();
                }
            }
            return listaPlanificacionActivas;
        }


        public List<PlanificacionVisita> ObtenerNumerosPlanificacionTodos()
        {
            List<PlanificacionVisita> listaPlanificacionActivas = new List<PlanificacionVisita>();
            using (var oConex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var sqlCommand = new SqlCommand("[dbo].[spObtenerPlanificacionTodos]", oConex))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    oConex.Open();

                    var reader = sqlCommand.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            PlanificacionVisita planificacion = new PlanificacionVisita();

                            planificacion.PlanificacionID = int.Parse(reader["PlanificacionID"].ToString());
                            planificacion.Descripcion = reader["Descripcion"].ToString();

                            listaPlanificacionActivas.Add(planificacion);

                        }
                    }

                    oConex.Close();
                }
            }
            return listaPlanificacionActivas;
        }

        public List<PlanificacionVisita.MotivoVisitaCancelacion> ObtenerMotivosVisita()
        {
            List<PlanificacionVisita.MotivoVisitaCancelacion> listaMotivosVisita = new List<PlanificacionVisita.MotivoVisitaCancelacion>();
            using (var oConex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var sqlCommand = new SqlCommand("[dbo].[spObtenerMotivosVisita]", oConex))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    oConex.Open();

                    var reader = sqlCommand.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            PlanificacionVisita planificacion = new PlanificacionVisita();
                            planificacion.motivoVisitaCancelacion = new PlanificacionVisita.MotivoVisitaCancelacion();

                            planificacion.motivoVisitaCancelacion.MotivoID = int.Parse(reader["MotivoID"].ToString());
                            planificacion.motivoVisitaCancelacion.Motivo = reader["Motivo"].ToString();

                            listaMotivosVisita.Add(planificacion.motivoVisitaCancelacion);

                        }
                    }

                    oConex.Close();
                }
            }
            return listaMotivosVisita;
        }

        public List<PlanificacionVisita.ContactoCliente> ObtenerContactoCliente(string ClienteID)
        {
            List<PlanificacionVisita.ContactoCliente> listaContactoCliente = new List<PlanificacionVisita.ContactoCliente>();
            using (var oConex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var sqlCommand = new SqlCommand("[dbo].[spObtenerContactoCliente]", oConex))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@ClienteID", ClienteID);
                    oConex.Open();

                    var reader = sqlCommand.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            PlanificacionVisita planificacion = new PlanificacionVisita();
                            planificacion.contactoCliente = new PlanificacionVisita.ContactoCliente();

                            planificacion.contactoCliente.ContactoID = reader["ContactoID"].ToString();
                            planificacion.contactoCliente.Contacto = reader["Contacto"].ToString();

                            listaContactoCliente.Add(planificacion.contactoCliente);

                        }
                    }

                    oConex.Close();
                }
            }
            return listaContactoCliente;
        }


        public List<PlanificacionVisita.Mensajes> RegistrarVisitaClientes(string correo, int PlanificacionID, string ClienteID, string ContactoID, string FechaVisita, int MotivoVisitaID, string ObservacionPlanificacion, int EsVisitaPlanificada, string FechaReal, string ObservacionVisita, string UbicacionLatitud, string UbicacionLongitud, int EsUbicado)
        {

            DateTime? dateFechaReal = string.IsNullOrEmpty(FechaReal) ? (DateTime?)null : DateTime.Parse(FechaReal);


            List<PlanificacionVisita.Mensajes> listaMensajes = new List<PlanificacionVisita.Mensajes>();
            using (var oConex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var sqlCommand = new SqlCommand("[dbo].[spInsertarVisitasClientes]", oConex))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@correo", correo);
                    sqlCommand.Parameters.AddWithValue("@PlanificacionID", PlanificacionID);
                    sqlCommand.Parameters.AddWithValue("@ClienteID", ClienteID);
                    sqlCommand.Parameters.AddWithValue("@ContactoID", ContactoID);
                    sqlCommand.Parameters.AddWithValue("@FechaVisita", Convert.ToDateTime(FechaVisita));
                    sqlCommand.Parameters.AddWithValue("@MotivoVisitaID", MotivoVisitaID);
                    sqlCommand.Parameters.AddWithValue("@ObservacionPlanificacion", ObservacionPlanificacion);
                    sqlCommand.Parameters.AddWithValue("@EsVisitaPlanificada", EsVisitaPlanificada);
                    sqlCommand.Parameters.AddWithValue("@FechaReal", dateFechaReal);
                    sqlCommand.Parameters.AddWithValue("@ObservacionVisita", ObservacionVisita);
                    sqlCommand.Parameters.AddWithValue("@UbicacionLatitud", UbicacionLatitud);
                    sqlCommand.Parameters.AddWithValue("@UbicacionLongitud", UbicacionLongitud);
                    sqlCommand.Parameters.AddWithValue("@EsUbicado", EsUbicado);
                    oConex.Open();

                    var reader = sqlCommand.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            PlanificacionVisita planificacion = new PlanificacionVisita();
                            planificacion.mensajes = new PlanificacionVisita.Mensajes();

                            planificacion.mensajes.MensajeID = int.Parse(reader["MensajeID"].ToString());
                            planificacion.mensajes.Mensaje = reader["Mensaje"].ToString();

                            listaMensajes.Add(planificacion.mensajes);

                        }
                    }

                    oConex.Close();
                }
            }
            return listaMensajes;
        }


        public List<PlanificacionVisita.MotivoVisitaCancelacion> ObtenerMotivosCancelacion()
        {
            List<PlanificacionVisita.MotivoVisitaCancelacion> listaMotivosVisita = new List<PlanificacionVisita.MotivoVisitaCancelacion>();
            using (var oConex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var sqlCommand = new SqlCommand("[dbo].[spObtenerMotivosCancelacion]", oConex))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    oConex.Open();

                    var reader = sqlCommand.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            PlanificacionVisita planificacion = new PlanificacionVisita();
                            planificacion.motivoVisitaCancelacion = new PlanificacionVisita.MotivoVisitaCancelacion();

                            planificacion.motivoVisitaCancelacion.MotivoID = int.Parse(reader["MotivoID"].ToString());
                            planificacion.motivoVisitaCancelacion.Motivo = reader["Motivo"].ToString();

                            listaMotivosVisita.Add(planificacion.motivoVisitaCancelacion);

                        }
                    }

                    oConex.Close();
                }
            }
            return listaMotivosVisita;
        }

        public List<PlanificacionVisita.Mensajes> CancelarVisitaClientes(int VisitaClienteID, int MotivoCancelacionID, string ObservacionCancelacion)
        {

            List<PlanificacionVisita.Mensajes> listaMensajes = new List<PlanificacionVisita.Mensajes>();
            using (var oConex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var sqlCommand = new SqlCommand("[dbo].[spCancelarVisitaCliente]", oConex))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@VisitaClienteID", VisitaClienteID);
                    sqlCommand.Parameters.AddWithValue("@MotivoCancelacionID", MotivoCancelacionID);
                    sqlCommand.Parameters.AddWithValue("@ObservacionCancelacion", ObservacionCancelacion);

                    oConex.Open();

                    var reader = sqlCommand.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            PlanificacionVisita planificacion = new PlanificacionVisita();
                            planificacion.mensajes = new PlanificacionVisita.Mensajes();

                            planificacion.mensajes.MensajeID = int.Parse(reader["MensajeID"].ToString());
                            planificacion.mensajes.Mensaje = reader["Mensaje"].ToString();

                            listaMensajes.Add(planificacion.mensajes);

                        }
                    }

                    oConex.Close();
                }
            }
            return listaMensajes;
        }

        public List<PlanificacionVisita> RecuperarVisitaClienteByID(int VisitaClienteID)
        {
            List<PlanificacionVisita> ListaPlanificacionVisita = new List<PlanificacionVisita>();
            using (var oConex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var sqlCommand = new SqlCommand("[dbo].[spRecuperarVisitaClienteByID]", oConex))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    oConex.Open();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@VisitaClienteID", VisitaClienteID);

                    var dr = sqlCommand.ExecuteReader();
                    using (dr)
                    {
                        while (dr.Read())
                        {
                            PlanificacionVisita planificacionVisita = new PlanificacionVisita();

                            planificacionVisita.VisitaClienteID = int.Parse(dr["VisitaClienteID"].ToString());
                            planificacionVisita.PlanificacionID = int.Parse(dr["PlanificacionID"].ToString());
                            planificacionVisita.ClienteID = dr["ClienteID"].ToString();
                            planificacionVisita.ContactoID = dr["ContactoID"].ToString();
                            planificacionVisita.FechaVisitaDT = DateTime.Parse(dr["FechaVisita"].ToString());
                            planificacionVisita.MotivoVisitaID = int.Parse(dr["MotivoVisitaID"].ToString());
                            planificacionVisita.ObservacionPlanificacion = dr["ObservacionPlanificacion"].ToString();
                            planificacionVisita.EsVisitaPlanificada = int.Parse(dr["EsVisitaPlanificada"].ToString());
                            planificacionVisita.EstadoVisita = int.Parse(dr["EstadoVisita"].ToString());
                            planificacionVisita.FechaReal = dr["FechaReal"].ToString();
                            planificacionVisita.ObservacionVisita = dr["ObservacionVisita"].ToString();
                            planificacionVisita.UbicacionLatitud = dr["UbicacionLatitud"].ToString();
                            planificacionVisita.UbicacionLongitud = dr["UbicacionLongitud"].ToString();
                            planificacionVisita.DepartamentoId = dr["departamentoId"].ToString();
                            planificacionVisita.ProvinciaId = dr["provinciaId"].ToString();
                            planificacionVisita.DistritoId = dr["distritoId"].ToString();
                            //planificacionVisita.MotivoCancelacionID = int.Parse(dr["MotivoCancelacionID"].ToString());
                            //planificacionVisita.ObservacionCancelacion = dr["ObservacionCancelacion"].ToString();

                            ListaPlanificacionVisita.Add(planificacionVisita);

                        }
                    }

                    oConex.Close();
                }
            }
            return ListaPlanificacionVisita;
        }


        public List<PlanificacionVisita.Mensajes> EditarVisitaCliente(string correo, int VisitaClienteID, int PlanificacionID, string ClienteID, string ContactoID, string FechaVisita, int MotivoVisitaID, string ObservacionPlanificacion, int EsVisitaPlanificada, string FechaReal, string ObservacionVisita, string UbicacionLatitud, string UbicacionLongitud, int EsUbicado)
        {

            DateTime? dateFechaReal = string.IsNullOrEmpty(FechaReal) ? (DateTime?)null : DateTime.Parse(FechaReal);


            List<PlanificacionVisita.Mensajes> listaMensajes = new List<PlanificacionVisita.Mensajes>();
            using (var oConex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var sqlCommand = new SqlCommand("[dbo].[spEditarVisitaCliente]", oConex))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@correo", correo);
                    sqlCommand.Parameters.AddWithValue("@VisitaClienteID", VisitaClienteID);
                    sqlCommand.Parameters.AddWithValue("@PlanificacionID", PlanificacionID);
                    sqlCommand.Parameters.AddWithValue("@ClienteID", ClienteID);
                    sqlCommand.Parameters.AddWithValue("@ContactoID", ContactoID);
                    sqlCommand.Parameters.AddWithValue("@FechaVisita", Convert.ToDateTime(FechaVisita));
                    sqlCommand.Parameters.AddWithValue("@MotivoVisitaID", MotivoVisitaID);
                    sqlCommand.Parameters.AddWithValue("@ObservacionPlanificacion", ObservacionPlanificacion);
                    sqlCommand.Parameters.AddWithValue("@EsVisitaPlanificada", EsVisitaPlanificada);
                    sqlCommand.Parameters.AddWithValue("@FechaReal", dateFechaReal);
                    sqlCommand.Parameters.AddWithValue("@ObservacionVisita", ObservacionVisita);
                    sqlCommand.Parameters.AddWithValue("@UbicacionLatitud", UbicacionLatitud);
                    sqlCommand.Parameters.AddWithValue("@UbicacionLongitud", UbicacionLongitud);
                    sqlCommand.Parameters.AddWithValue("@EsUbicado", EsUbicado);
                    oConex.Open();

                    var reader = sqlCommand.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            PlanificacionVisita planificacion = new PlanificacionVisita();
                            planificacion.mensajes = new PlanificacionVisita.Mensajes();

                            planificacion.mensajes.MensajeID = int.Parse(reader["MensajeID"].ToString());
                            planificacion.mensajes.Mensaje = reader["Mensaje"].ToString();

                            listaMensajes.Add(planificacion.mensajes);

                        }
                    }

                    oConex.Close();
                }
            }
            return listaMensajes;
        }

        public List<PlanificacionVisita> getSelectVisitasClientes(string ClienteID, string fechaEmision)
        {
            List<PlanificacionVisita> ListaPlanificacionVisita = new List<PlanificacionVisita>();
            using (var oConex = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var sqlCommand = new SqlCommand("[dbo].[spObtenerPlanificacionVisitasPorCliente]", oConex))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    oConex.Open();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@ClienteID", ClienteID);
                    sqlCommand.Parameters.AddWithValue("@FechaEmision", Convert.ToDateTime(fechaEmision));
                    var dr = sqlCommand.ExecuteReader();
                    using (dr)
                    {
                        while (dr.Read())
                        {
                            PlanificacionVisita planificacionVisita = new PlanificacionVisita();

                            planificacionVisita.VisitaClienteID = int.Parse(dr["VisitaClienteID"].ToString());
                            planificacionVisita.Descripcion = dr["VisitaCliente"].ToString();

                            ListaPlanificacionVisita.Add(planificacionVisita);

                        }
                    }

                    oConex.Close();
                }
            }
            return ListaPlanificacionVisita;
        }



    }
}
