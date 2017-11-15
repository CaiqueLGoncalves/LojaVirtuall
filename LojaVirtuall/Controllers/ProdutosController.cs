using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LojaVirtuall.Models;
using System.Web.UI;

namespace LojaVirtuall.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ProdutosController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Produtos
        public ActionResult Index()
        {
            var produto = db.Produto.Include(p => p.Categoria).Include(p => p.Fornecedor);
            return View(produto.ToList());
        }

        // GET: Produtos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);

            if (produto == null)
            {
                return HttpNotFound();
            } else
            {
                produto.Categoria = db.Categoria.Find(produto.CategoriaID);
                produto.Fornecedor = db.Fornecedor.Find(produto.FornecedorID);
            }
            return View(produto);
        }

        // GET: Produtos/Create
        public ActionResult Create()
        {
            ViewBag.CategoriaID = new SelectList(db.Categoria, "CategoriaID", "Nome");
            ViewBag.FornecedorID = new SelectList(db.Fornecedor, "FornecedorID", "Nome");
            return View();
        }

        // POST: Produtos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProdutoID, Nome, Descricao, Preco, Quantidade, Ativo, CategoriaID, FornecedorID")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                produto.CriadoEm = DateTime.Now;
                produto.ModificadoEm = DateTime.Now;

                db.Produto.Add(produto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriaID = new SelectList(db.Categoria, "CategoriaID", "Nome", produto.CategoriaID);
            ViewBag.FornecedorID = new SelectList(db.Fornecedor, "FornecedorID", "Nome", produto.FornecedorID);
            return View(produto);
        }

        // GET: Produtos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriaID = new SelectList(db.Categoria, "CategoriaID", "Nome", produto.CategoriaID);
            ViewBag.FornecedorID = new SelectList(db.Fornecedor, "FornecedorID", "Nome", produto.FornecedorID);
            return View(produto);
        }

        // POST: Produtos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProdutoID, Nome, Descricao, Preco, Quantidade, Ativo, CategoriaID, FornecedorID")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                produto.CriadoEm = DateTime.Now;
                produto.ModificadoEm = DateTime.Now;

                db.Entry(produto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoriaID = new SelectList(db.Categoria, "CategoriaID", "Nome", produto.CategoriaID);
            ViewBag.FornecedorID = new SelectList(db.Fornecedor, "FornecedorID", "Nome", produto.FornecedorID);
            return View(produto);
        }

        // GET: Produtos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            else
            {
                produto.Categoria = db.Categoria.Find(produto.CategoriaID);
                produto.Fornecedor = db.Fornecedor.Find(produto.FornecedorID);
            }
            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Produto produto = db.Produto.Find(id);
            db.Produto.Remove(produto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
