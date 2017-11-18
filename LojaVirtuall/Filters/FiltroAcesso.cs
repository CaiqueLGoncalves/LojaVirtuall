using LojaVirtuall.Repositories;
using System.Web.Mvc;

namespace LojaVirtuall.Filters
{
    public class FiltroAcesso : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filtroContexto)
        {
            var controller = filtroContexto.ActionDescriptor.ControllerDescriptor.ControllerName;
            var action = filtroContexto.ActionDescriptor.ActionName;

            if ((controller != "Home") &&
                (controller != "Clientes" || action != "Register") &&
                (controller != "Administradores" || action != "Register"))
            {

                if ((controller == "Clientes" && action == "ManageAccount"))
                {
                    if (GestaoUsuarios.VerificarStatusCliente() == null)
                    {
                        filtroContexto.RequestContext.HttpContext.Response.Redirect("/Home/Login");
                    }
                }
                else
                {
                    if (GestaoUsuarios.VerificarStatusAdministrador() == null)
                    {
                        filtroContexto.RequestContext.HttpContext.Response.Redirect("/Home/Login");
                    }
                }
            }
        }
    }
}