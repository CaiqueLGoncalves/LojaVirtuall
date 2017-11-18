using LojaVirtuall.Filters;
using System.Web.Mvc;

namespace LojaVirtuall.Controllers
{
    [FiltroAcesso]
    public class BaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filtroContexto)
        {
            base.OnActionExecuted(filtroContexto);
        }
    }
}