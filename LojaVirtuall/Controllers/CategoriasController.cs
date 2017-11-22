using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LojaVirtuall.Models;
using System;
using System.Web.UI;
using LojaVirtuall.Repositories;

namespace LojaVirtuall.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class CategoriasController : BaseController
    {
        private Contexto db = new Contexto();

        // GET: Categorias
        public ActionResult Index()
        {
            return View(db.Categoria.ToList());
        }

        // GET: Categorias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categoria.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }

        // GET: Categorias/Create
        public ActionResult Create()
        {
            if (GestaoUsuarios.VerificarStatusAdministrador() != null)
            {
                return View();
            }
            return null;
        }

        // POST: Categorias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoriaID, Nome, Descricao")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                int idUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["ID"].ToString());

                categoria.CriadoEm = DateTime.Now;
                categoria.ModificadoEm = DateTime.Now;
                categoria.CriadoPor = db.Administrador.Find(idUser);
                categoria.ModificadoPor = db.Administrador.Find(idUser);

                db.Categoria.Add(categoria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoria);
        }

        // GET: Categorias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (GestaoUsuarios.VerificarStatusAdministrador() != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Categoria categoria = db.Categoria.Find(id);
                if (categoria == null)
                {
                    return HttpNotFound();
                }
                return View(categoria);
            }
            return null;
        }

        // POST: Categorias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoriaID, Nome, Descricao")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                int idUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["ID"].ToString());

                categoria.CriadoEm = DateTime.Now; // Não conseguimos ajustar isto.
                categoria.ModificadoEm = DateTime.Now;
                categoria.CriadoPor = db.Administrador.Find(idUser); // Não conseguimos ajustar isto.
                categoria.ModificadoPor = db.Administrador.Find(idUser);

                db.Entry(categoria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (GestaoUsuarios.VerificarStatusAdministrador() != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Categoria categoria = db.Categoria.Find(id);
                if (categoria == null)
                {
                    return HttpNotFound();
                }
                return View(categoria);
            }

            return null;
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categoria categoria = db.Categoria.Find(id);
            db.Categoria.Remove(categoria);
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