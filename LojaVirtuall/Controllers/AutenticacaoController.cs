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
                return Json(new { OK = true, Nivel = "Cliente", Mensagem = "Dados corretos. Redirecionando..." },
                    JsonRequestBehavior.AllowGet);
            }
            else if (GestaoUsuarios.VerificarAdministradorBD(login, senha))
            {
                return Json(new { OK = true, Nivel = "Administrador", Mensagem = "Dados corretos. Redirecionando..." },
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { OK = false, Mensagem = "Dados incorretos." },
                    JsonRequestBehavior.AllowGet);
            }
        }
    }
}