using LojaVirtuall.Models;
using System;
using System.Web;

namespace LojaVirtuall.Repositories
{
    public class GestaoCookies
    {
        public static void CriarCookie(int usuarioID, bool adm)
        {
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

            cookieUsuario.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies.Add(cookieUsuario);
        }
    }
}