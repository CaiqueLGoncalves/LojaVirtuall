using System.Web.Mvc;
using System.Web.UI;

namespace LojaVirtuall.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class HomeController : Controller
    {
        public ActionResult Login(string redir)
        {
            ViewBag.Title = "Autenticação";
            ViewBag.Redir = redir;
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}