using LojaVirtuall.Models;
using System;
using System.Web;

namespace LojaVirtuall.Repositories
{
    public class GestaoCookies
    {
        public static void CriarCookie(int usuarioID, bool adm)
        {
            ApagarCookie(); // Apaga o cookie de autenticação antes de criar um novo

            Contexto db = new Contexto();

            HttpCookie cookieUsuario = new HttpCookie("CookieUsuario");
            cookieUsuario.Values["ID"] = usuarioID.ToString();

            if (adm)
            {
                cookieUsuario.Values["Login"] = db.Administrador.Find(usuarioID).Login;
                cookieUsuario.Values["Nivel"] = "Administrador";
            }
            else
            {
                cookieUsuario.Values["Login"] = db.Cliente.Find(usuarioID).Login;
                cookieUsuario.Values["Nivel"] = "Cliente";
            }

            cookieUsuario.Expires = DateTime.Now.AddMinutes(5);
            HttpContext.Current.Response.Cookies.Add(cookieUsuario);
        }

        public static bool ApagarCookie()
        {
            var cookie = HttpContext.Current.Request.Cookies["CookieUsuario"];

            if (cookie == null)
            {
                return false;
            }
            else
            {
                HttpContext.Current.Response.Cookies[cookie.Name].Expires = DateTime.Now.AddDays(-1);
                return true;
            }
        }
    }
}