using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Web.Security;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CambiarContraseña()
        {
            return View();
        }

        public ActionResult ReestablecerContraseña()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.CovertirSHA256(clave)).FirstOrDefault();

            if(oUsuario == null)
            {
                ViewBag.Error = "La combinacion correo-contraseña no es correcta";
                return View();
            }
            else
            {
                if(oUsuario.Reestablecer)
                {
                    TempData["IdUsuario"] = oUsuario.IdUsuario;
                    return RedirectToAction("CambiarContraseña");
                }

                FormsAuthentication.SetAuthCookie(oUsuario.Correo, false);

                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult CambiarContraseña(string idUsuario, string claveActual, string nuevaClave, string confirmarClave)
        {
            Usuario oUsuario = new Usuario();
            oUsuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(idUsuario)).FirstOrDefault();

            if(oUsuario.Clave != CN_Recursos.CovertirSHA256(claveActual))
            {
                TempData["IdUsuario"] = idUsuario;
                ViewData["vClave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if(nuevaClave != confirmarClave)
            {
                TempData["IdUsuario"] = idUsuario;
                ViewData["vClave"] = claveActual;
                ViewBag.Error = "Las nuevas contraseñas no coinciden";
                return View();
            }
            TempData["IdUsuario"] = idUsuario;
            ViewData["vClave"] = "";

            nuevaClave = CN_Recursos.CovertirSHA256(nuevaClave);

            string mensaje = "";

            bool respuesta = new CN_Usuarios().CambiarContraseña(int.Parse(idUsuario), nuevaClave, out mensaje);

            if(respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdUsuario"] = idUsuario;
                ViewBag.Error = mensaje;
                return View();
            }            
        }

        [HttpPost]
        public ActionResult ReestablecerContraseña(string correo)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if(oUsuario == null)
            {
                ViewBag.Error = "No se encontro un usuario relacionado a ese correo";
                return View();
            }

            string mensaje = "";
            bool respuesta = new CN_Usuarios().ReestablecerContraseña(oUsuario.IdUsuario, correo, out mensaje);

            if(respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }
    }
}