using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Dashboard
    {
        private CD_Dashboard objCapaDato = new CD_Dashboard();

        public List<Reporte> Ventas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            return objCapaDato.Ventas(fechaInicio, fechaFin, idTransaccion);
        }

        public Dashboard VerDashboard()
        {
            return objCapaDato.VerDashboard();
        }
    }
}
