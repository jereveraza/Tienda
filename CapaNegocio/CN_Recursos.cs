using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace CapaNegocio
{
    public class CN_Recursos
    {
        //Encriptar un texto en SHA256
        public static string CovertirSHA256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);
            return clave;
        }

        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("jv.asp.net@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("jv.asp.net@gmail.com", "yhotuqphgtpjezrz"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };

                smtp.Send(mail);
                resultado = true;
            }
            catch(Exception e)
            {
                resultado = false;
            }
            return resultado;
        }

        public static string ConvertirImagenBase64(string ruta,out bool conversionExitosa)
        {
            string textoBase64 = string.Empty;
            conversionExitosa = true;

            try
            {
                byte[] bytesImagen = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytesImagen);
            }
            catch(Exception e)
            {
                conversionExitosa = false;
            }

            return textoBase64;
        }
    }
}
