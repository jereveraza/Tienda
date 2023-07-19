using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class MantenimientoController : Controller
    {
        // GET: Mantenimiento
        public ActionResult Categoria()
        {
            return View();
        }

        public ActionResult Marca()
        {
            return View();
        }

        public ActionResult Producto()
        {
            return View();
        }

        #region Categoria

        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = new CN_Categoria().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria categoria)
        {
            object resultado;
            string mensaje = string.Empty;

            if (categoria.IdCategoria == 0)
            {
                resultado = new CN_Categoria().Registrar(categoria, out mensaje);
            }
            else
            {
                resultado = new CN_Categoria().Editar(categoria, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Categoria().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Marca

        [HttpGet]
        public JsonResult ListarMarcas()
        {
            List<Marca> oLista = new List<Marca>();
            oLista = new CN_Marca().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca marca)
        {
            object resultado;
            string mensaje = string.Empty;

            if (marca.IdMarca == 0)
            {
                resultado = new CN_Marca().Registrar(marca, out mensaje);
            }
            else
            {
                resultado = new CN_Marca().Editar(marca, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Marca().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Producto

        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> oLista = new List<Producto>();
            oLista = new CN_Producto().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(string producto, HttpPostedFileBase archivoImagen)
        {
            string mensaje = string.Empty;
            bool operacionExitosa = true;
            bool guardarImagenExitosamente = true;

            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(producto);

            decimal precio;

            if(decimal.TryParse(oProducto.PrecioTexto,System.Globalization.NumberStyles.AllowDecimalPoint,new CultureInfo("es-AR"),out precio))
            {
                oProducto.Precio = precio;
            }
            else
            {
                return Json(new { operacionExitosa = false, mensaje = "El formato del precio no es correcto" },JsonRequestBehavior.AllowGet);
            }

            if (oProducto.IdProducto == 0)
            {
                int idGenerado = new CN_Producto().Registrar(oProducto, out mensaje);

                if (idGenerado != 0)
                    oProducto.IdProducto = idGenerado;
                else
                    operacionExitosa = false;
            }
            else
            {
                operacionExitosa = new CN_Producto().Editar(oProducto, out mensaje);
            }

            if(operacionExitosa)
            {
                if(archivoImagen != null)
                {
                    string pathImagenes = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombreImagen = string.Concat(oProducto.IdProducto.ToString(),extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(pathImagenes, nombreImagen));
                    }
                    catch(Exception e)
                    {
                        string msg = e.Message;
                        guardarImagenExitosamente = false;
                    }

                    if(guardarImagenExitosamente)
                    {
                        oProducto.RutaImagen = pathImagenes;
                        oProducto.NombreImagen = nombreImagen;
                        bool resp = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "No se pudo guardar la imagen del producto";
                    }
                }
            }

            return Json(new { operacionExitosa = operacionExitosa,idGenerado = oProducto.IdProducto ,mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            Producto oProducto = new CN_Producto().Listar().Where(p => p.IdProducto == id).FirstOrDefault();

            string textoBase64 = CN_Recursos.ConvertirImagenBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen),out conversion);

            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(oProducto.NombreImagen)
            },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Producto().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}