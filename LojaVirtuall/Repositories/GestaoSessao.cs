using LojaVirtuall.Models;
using System.Web;

namespace LojaVirtuall.Repositories
{
    public class GestaoSessao
    {
        public static void AbrirSessao(int usuarioID, bool adm)
        {
            Contexto db = new Contexto();

            HttpContext.Current.Session["ID"] = usuarioID.ToString();

            Usuario usuario;

            if (adm)
            {
                usuario = db.Administrador.Find(usuarioID);
                HttpContext.Current.Session["Nivel"] = "Administrador";
            }
            else
            {
                usuario = db.Cliente.Find(usuarioID);
                HttpContext.Current.Session["Nivel"] = "Cliente";
            }

            HttpContext.Current.Session["Nome"] = usuario.Nome;
            HttpContext.Current.Session["Login"] = usuario.Login;
            HttpContext.Current.Session["Email"] = usuario.Email;
        }

        public static bool FecharSessao()
        {
            var sessao = HttpContext.Current.Session;

            if (sessao == null)
            {
                return false;
            }
            else
            {
                HttpContext.Current.Session["ID"] = null;
                HttpContext.Current.Session["Nome"] = null;
                HttpContext.Current.Session["Login"] = null;
                HttpContext.Current.Session["Email"] = null;
                HttpContext.Current.Session["Nivel"] = null;
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();

                return true;
            }
        }
    }
}