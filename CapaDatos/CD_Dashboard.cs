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
    }
}
