using LojaVirtuall.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LojaVirtuall.Repositories
{
    public class GestaoUsuarios
    {
        public static bool VerificarClienteBD(string login, string senha)
        {
            try
            {
                senha = CalculateMD5String(senha);

                Contexto db = new Contexto();
                var cliente = db.Cliente.Where(c => c.Login == login && c.Senha == senha && c.Ativo == true).SingleOrDefault();

                if (cliente != null)
                {
                    GestaoCookies.CriarCookie(cliente.UsuarioID, false);
                    GestaoSessao.AbrirSessao(cliente.UsuarioID, false);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool VerificarAdministradorBD(string login, string senha)
        {
            try
            {
                senha = CalculateMD5String(senha);

                Contexto db = new Contexto();
                var administrador = db.Administrador.Where(a => a.Login == login && a.Senha == senha && a.Ativo == true).SingleOrDefault();

                if (administrador != null)
                {
                    GestaoCookies.CriarCookie(administrador.UsuarioID, true);
                    GestaoSessao.AbrirSessao(administrador.UsuarioID, true);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Usuario RecuperaUsuario(int UsuarioID, string tipo)
        {
            try
            {
                Contexto db = new Contexto();
                if (tipo == "Cliente")
                {
                    var usuario = db.Cliente.Where(c => c.UsuarioID == UsuarioID).SingleOrDefault();
                    return usuario;
                }
                else
                {
                    var usuario = db.Administrador.Where(c => c.UsuarioID == UsuarioID).SingleOrDefault();
                    return usuario;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Cliente VerificarStatusCliente()
        {
            try
            {
                var cookie = HttpContext.Current.Request.Cookies["CookieUsuario"];
                var sessao = HttpContext.Current.Session;

                if (cookie != null && sessao != null)
                {
                    if (cookie.Values["ID"].Equals(sessao["ID"]) &&
                        cookie.Values["Login"].Equals(sessao["Login"]) &&
                        cookie.Values["Nivel"].Equals(sessao["Nivel"]))
                    {
                        if (cookie.Values["Nivel"].Equals("Cliente"))
                        {

                            int id = Convert.ToInt32(cookie.Values["ID"]);
                            var usuarioRegistrado = RecuperaUsuario(id, cookie.Values["Nivel"]);
                            return usuarioRegistrado as Cliente;
                        }
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Administrador VerificarStatusAdministrador()
        {
            try
            {
                var cookie = HttpContext.Current.Request.Cookies["CookieUsuario"];
                var sessao = HttpContext.Current.Session;

                if (cookie != null && sessao != null)
                {
                    if (cookie.Values["ID"].Equals(sessao["ID"]) &&
                        cookie.Values["Login"].Equals(sessao["Login"]) &&
                        cookie.Values["Nivel"].Equals(sessao["Nivel"]))
                    {
                        if (cookie.Values["Nivel"].Equals("Administrador"))
                        {
                            int id = Convert.ToInt32(cookie.Values["ID"]);
                            var usuarioRegistrado = RecuperaUsuario(id, cookie.Values["Nivel"]);
                            return usuarioRegistrado as Administrador;
                        }
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string CalculateMD5String(string entrada)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(entrada);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}