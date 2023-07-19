using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios objCapaDato = new CD_Usuarios();

        public List<Usuario> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
                Mensaje = "El nombre no es valido";
            else if (string.IsNullOrEmpty(obj.Apellido) || string.IsNullOrWhiteSpace(obj.Apellido))
                Mensaje = "El apellido no es valido";
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
                Mensaje = "El correo no es valido";

            if (string.IsNullOrEmpty(Mensaje))
            {
                string clave = CN_Recursos.GenerarClave();

                string asunto = "Creacion de cuenta";
                string mensajeCorreo = "<h3>Su cuenta fue creada correctamente</h3><br><p>Su contraseña para acceder es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", clave);

                bool respuesta = CN_Recursos.EnviarCorreo(obj.Correo, asunto, mensajeCorreo);

                if(respuesta)
                {
                    obj.Clave = CN_Recursos.CovertirSHA256(clave);
                    return objCapaDato.Registrar(obj, out Mensaje);
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return 0;
                }  
            }                
            else
                return 0;
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
                Mensaje = "El nombre no es valido";
            else if (string.IsNullOrEmpty(obj.Apellido) || string.IsNullOrWhiteSpace(obj.Apellido))
                Mensaje = "El apellido no es valido";
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
                Mensaje = "El correo no es valido";

            if (string.IsNullOrEmpty(Mensaje))
            {
                string clave = CN_Recursos.GenerarClave();
                obj.Clave = CN_Recursos.CovertirSHA256(clave);

                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
                return false;
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }

        public bool CambiarContraseña(int idUsuario, string nuevaClave, out string Mensaje)
        {
            return objCapaDato.CambiarContraseña(idUsuario,nuevaClave, out Mensaje);
        }

        public bool ReestablecerContraseña(int idUsuario, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaClave = CN_Recursos.GenerarClave();

            bool resultado = objCapaDato.ReestablecerContraseña(idUsuario,CN_Recursos.CovertirSHA256(nuevaClave), out Mensaje);

            if (resultado)
            {
                string asunto = "Contraseña reestablecida";
                string mensajeCorreo = "<h3>Su clave fue reestablecida correctamente</h3><br><p>Su nueva contraseña es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", nuevaClave);

                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensajeCorreo);

                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo reestablecer la contraseña";
                return false;
            }
               
        }

    }
}
