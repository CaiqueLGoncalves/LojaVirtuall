using LojaVirtuall.Repositories;
using System.Web.Mvc;

namespace LojaVirtuall.Controllers
{
    public class AutenticacaoController : Controller
    {
        public JsonResult AutenticarUsuario(string login, string senha)
        {
            if (GestaoUsuarios.VerificarClienteBD(login, senha))
            {
                return Json(new { OK = true, Nivel = "Cliente", Mensagem = "Redirecionando..." },
                    JsonRequestBehavior.AllowGet);
            }
            else if (GestaoUsuarios.VerificarAdministradorBD(login, senha))
            {
                return Json(new { OK = true, Nivel = "Administrador", Mensagem = "Redirecionando..." },
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { OK = false, Mensagem = "Dados incorretos." },
                    JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DesautenticarUsuario()
        {
            if (GestaoCookies.ApagarCookie() && GestaoSessao.FecharSessao())
            {
                return Json(new { OK = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { OK = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult VerificarAutenticacao()
        {
            if (GestaoUsuarios.VerificarStatusCliente() != null)
            {
                return Json(new { OK = true, Nivel = "Cliente" }, JsonRequestBehavior.AllowGet);
            }
            else if (GestaoUsuarios.VerificarStatusAdministrador() != null)
            {
                return Json(new { OK = true, Nivel = "Administrador" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { OK = false, Nivel = "Visitante" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}