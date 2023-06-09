using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;

namespace CapaPresentacionAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> oLista = new List<Usuario>();
            oLista = new CN_Usuarios().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario usuario)
        {
            object resultado;
            string mensaje = string.Empty;

            if(usuario.IdUsuario == 0)
            {
                resultado = new CN_Usuarios().Registrar(usuario, out mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().Editar(usuario, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Usuarios().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ListaDashboard()
        {
            Dashboard objeto = new CN_Dashboard().VerDashboard();

            return Json(new { resultado = objeto }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListaReporte(string fechaInicio,string fechaFin, string idTransaccion)
        {
            List<Reporte> oLista = new List<Reporte>();

            oLista = new CN_Dashboard().Ventas(fechaInicio, fechaFin, idTransaccion);

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult ExportarVentas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<Reporte> oLista = new List<Reporte>();
            oLista = new CN_Dashboard().Ventas(fechaInicio, fechaFin, idTransaccion);

            DataTable dt = new DataTable();

            dt.Columns.Add("Fecha Venta",typeof(string));
            dt.Columns.Add("Cliente",typeof(string));
            dt.Columns.Add("Producto",typeof(string));
            dt.Columns.Add("Precio",typeof(decimal));
            dt.Columns.Add("Cantidad",typeof(int));
            dt.Columns.Add("Total",typeof(decimal));
            dt.Columns.Add("Id Transaccion",typeof(string));

            foreach(var rp in oLista)
            {
                dt.Rows.Add(new object[]
                {
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion
                }) ;
            }
            dt.TableName = "Datos";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","Reporte Venta" + DateTime.Now.ToString() + ".xlsx");
                }
            } ;
        }

    }
}