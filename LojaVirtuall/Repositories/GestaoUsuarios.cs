using LojaVirtuall.Models;
using System;
using System.Linq;

namespace LojaVirtuall.Repositories
{
    public class GestaoUsuarios
    {
        public static bool VerificarClienteBD(string login, string senha)
        {
            try
            {
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
    }
}