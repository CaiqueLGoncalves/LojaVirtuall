using LojaVirtuall.Repositories;
using System.Web;
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
                (controller != "Administradores" || action != "Register") &&
                (controller != "Produtos" || action != "Search"))
            {

                if ((controller == "Clientes" && action == "ManageAccount") ||
                    (controller == "Clientes" && action == "ChangePassword") ||
                    (controller == "Pedidos" && action == "EmitirPedido") ||
                    (controller == "Pedidos" && action == "ExibirPedido") ||
                    (controller == "Pedidos" && action == "MeusPedidos") ||
                    (controller == "Pedidos" && action == "CancelarPedido"))
                {
                    if (GestaoUsuarios.VerificarStatusCliente() == null)
                    {
                        if (controller == "Pedidos" && action == "EmitirPedido")
                        {
                            HttpContext.Current.Response.Redirect("/Home/Login?redir=/Carrinho");
                        }
                        else if ((controller == "Pedidos" && action == "ExibirPedido") || (controller == "Pedidos" && action == "CancelarPedido"))
                        {
                            HttpContext.Current.Response.Redirect("/Home/Login?redir=/Pedidos/MeusPedidos");
                        }
                        else
                        {
                            HttpContext.Current.Response.Redirect("/Home/Login?redir=/" + controller + "/" + action);
                        }
                    }
                }
                else
                {
                    if (GestaoUsuarios.VerificarStatusAdministrador() == null)
                    {
                        HttpContext.Current.Response.Redirect("/Home/Login?redir=/" + controller + "/" + action);
                    }
                }
            }
        }
    }
}