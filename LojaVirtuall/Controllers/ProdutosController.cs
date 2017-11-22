using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LojaVirtuall.Models;
using System.Web.UI;
using LojaVirtuall.Repositories;

namespace LojaVirtuall.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ProdutosController : BaseController
    {
        private Contexto db = new Contexto();

        // GET: Produtos
        public ActionResult Index()
        {
            var produto = db.Produto.Include(p => p.Categoria).Include(p => p.Fornecedor);
            return View(produto.ToList());
        }

        public ActionResult Search(string filtroBusca, string busca, int? ordenacao)
        {
            IQueryable<Produto> produto;

            switch (filtroBusca)
            {
                case "Nome":
                    {
                        if (busca.Trim().Length > 0)
                        {
                            produto = db.Produto.Where(p => p.Ativo == true && p.Nome.ToLower().Contains(busca.ToLower())).Include(p => p.Categoria).Include(p => p.Fornecedor);
                        }
                        else
                        {
                            produto = db.Produto.Where(p => p.Ativo == true).Include(p => p.Categoria).Include(p => p.Fornecedor);
                        }
                        break;
                    }
                case "Fornecedor":
                    {
                        if (busca.Trim().Length > 0)
                        {
                            produto = db.Produto.Where(p => p.Ativo == true && p.Fornecedor.Nome.ToLower().Contains(busca.ToLower())).Include(p => p.Categoria).Include(p => p.Fornecedor);
                        }
                        else
                        {
                            produto = db.Produto.Where(p => p.Ativo == true).Include(p => p.Categoria).Include(p => p.Fornecedor);
                        }
                        break;
                    }
                case "Categoria":
                    {
                        if (busca.Trim().Length > 0)
                        {
                            produto = db.Produto.Where(p => p.Ativo == true && p.Categoria.Nome.ToLower().Contains(busca.ToLower())).Include(p => p.Categoria).Include(p => p.Fornecedor);
                        }
                        else
                        {
                            produto = db.Produto.Where(p => p.Ativo == true).Include(p => p.Categoria).Include(p => p.Fornecedor);
                        }
                        break;
                    }
                default:
                    {
                        produto = db.Produto.Where(p => p.Ativo == true).Include(p => p.Categoria).Include(p => p.Fornecedor);
                        break;
                    }
            }

            switch (ordenacao)
            {
                case 1:
                    {
                        produto = produto.OrderBy(p => p.Nome);
                        break;
                    }
                case 2:
                    {
                        produto = produto.OrderByDescending(p => p.Nome);
                        break;
                    }
                case 3:
                    {
                        produto = produto.OrderBy(p => p.Preco);
                        break;
                    }
                case 4:
                    {
                        produto = produto.OrderByDescending(p => p.Preco);
                        break;
                    }
                case 5:
                    {
                        produto = produto.OrderBy(p => p.Categoria.Nome);
                        break;
                    }
                case 6:
                    {
                        produto = produto.OrderByDescending(p => p.Categoria.Nome);
                        break;
                    }
                case 7:
                    {
                        produto = produto.OrderBy(p => p.Fornecedor.Nome);
                        break;
                    }
                case 8:
                    {
                        produto = produto.OrderByDescending(p => p.Fornecedor.Nome);
                        break;
                    }
                default:
                    {
                        produto = produto.OrderBy(p => p.Nome);
                        break;
                    }
            }

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
            }
            else
            {
                produto.Categoria = db.Categoria.Find(produto.CategoriaID);
                produto.Fornecedor = db.Fornecedor.Find(produto.FornecedorID);
            }
            return View(produto);
        }

        // GET: Produtos/Create
        public ActionResult Create()
        {
            if (GestaoUsuarios.VerificarStatusAdministrador() != null)
            {
                ViewBag.CategoriaID = new SelectList(db.Categoria, "CategoriaID", "Nome");
                ViewBag.FornecedorID = new SelectList(db.Fornecedor, "FornecedorID", "Nome");
                return View();
            }
            return null;
        }

        // POST: Produtos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProdutoID, Nome, Descricao, Preco, Quantidade, Ativo, CategoriaID, FornecedorID")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                int idUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["ID"].ToString());

                produto.CriadoEm = DateTime.Now;
                produto.ModificadoEm = DateTime.Now;
                produto.CriadoPor = db.Administrador.Find(idUser);
                produto.ModificadoPor = db.Administrador.Find(idUser);

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
            if (GestaoUsuarios.VerificarStatusAdministrador() != null)
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
            return null;
        }

        // POST: Produtos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProdutoID, Nome, Descricao, Preco, Quantidade, Ativo, CategoriaID, FornecedorID")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                int idUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["ID"].ToString());

                produto.CriadoEm = DateTime.Now; // Não conseguimos ajustar isto.
                produto.ModificadoEm = DateTime.Now;
                produto.CriadoPor = db.Administrador.Find(idUser); // Não conseguimos ajustar isto.
                produto.ModificadoPor = db.Administrador.Find(idUser);

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
            if (GestaoUsuarios.VerificarStatusAdministrador() != null)
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

            return null;
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
