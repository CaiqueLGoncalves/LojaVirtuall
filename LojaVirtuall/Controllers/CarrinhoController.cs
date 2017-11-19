using LojaVirtuall.Repositories;
using System.Web.Mvc;

namespace LojaVirtuall.Controllers
{
    public class CarrinhoController : Controller
    {
        // private static bool estaNoCarrinho = false;

        public ActionResult Index()
        {
            ViewBag.Carrinho = GestaoCarrinho.RetornarCarrinho();
            ViewBag.TotalCarrinho = GestaoCarrinho.RetornarTotalCarrinho();
            // ViewBag.EstaNoCarrinho = estaNoCarrinho;
            return View();
        }

        public ActionResult AdicionarProdutoCarrinho(int produtoID, int quantidade)
        {
            if (!GestaoCarrinho.EstaNoCarrinho(produtoID))
            {
                // estaNoCarrinho = false;
                GestaoCarrinho.AdicionarProdutoCarrinho(produtoID, quantidade);
            }
            /*
            else
            {
                estaNoCarrinho = true;
            }
            */

            return RedirectToAction("Index");
        }

        public ActionResult RemoverProdutoCarrinho(int produtoID)
        {
            GestaoCarrinho.RemoverProdutoCarrinho(produtoID);
            return RedirectToAction("Index");
        }

        public ActionResult AtualizarProdutoCarrinho(int produtoID, int novaQuantidade)
        {
            GestaoCarrinho.AtualizarProdutoCarrinho(produtoID, novaQuantidade);
            return RedirectToAction("Index");
        }

        public ActionResult IncrementarProdutoCarrinho(int produtoID)
        {
            GestaoCarrinho.IncrementarProdutoCarrinho(produtoID);
            return RedirectToAction("Index");
        }

        public ActionResult DecrementarProdutoCarrinho(int produtoID)
        {
            GestaoCarrinho.DecrementarProdutoCarrinho(produtoID);
            return RedirectToAction("Index");
        }

        public ActionResult LimparCarrinho()
        {
            GestaoCarrinho.LimparCarrinho();
            return RedirectToAction("Index");
        }
    }
}