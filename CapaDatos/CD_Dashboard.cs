using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Dashboard
    {
        public Dashboard VerDashboard()
        {
            Dashboard objeto = new Dashboard();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ReporteDashboard", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            objeto = new Dashboard()
                            {
                                TotalCliente = Convert.ToInt32(reader["TotalCliente"]),
                                TotalProducto = Convert.ToInt32(reader["TotalProducto"]),
                                TotalVenta = Convert.ToInt32(reader["TotalVenta"]),
                            };
                            
                        }
                    }
                }
            }
            catch
            {
                objeto = new Dashboard();
            }
            return objeto;
        }

        public List<Reporte> Ventas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<Reporte> lista = new List<Reporte>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {


                    SqlCommand cmd = new SqlCommand("sp_reporteVentas", oConexion);
                    cmd.Parameters.AddWithValue("fechainicio", fechaInicio);
                    cmd.Parameters.AddWithValue("fechafin", fechaFin);
                    cmd.Parameters.AddWithValue("idtransaccion", idTransaccion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(
                                new Reporte()
                                {
                                    FechaVenta = reader["FechaVenta"].ToString(),
                                    Cliente = reader["Cliente"].ToString(),
                                    Producto = reader["Producto"].ToString(),
                                    Precio = Convert.ToDecimal(reader["Precio"]),
                                    Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                    Total = Convert.ToDecimal(reader["Total"]),
                                    IdTransaccion = reader["IdTransaccion"].ToString()
                                });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Reporte>();
            }
            return lista;
        }
    }
}
