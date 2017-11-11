using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LojaVirtuall.Models;
using System;

namespace LojaVirtuall.Controllers
{
    public class FornecedoresController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Fornecedores
        public ActionResult Index()
        {
            return View(db.Fornecedor.ToList());
        }

        // GET: Fornecedores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fornecedor fornecedor = db.Fornecedor.Find(id);
            if (fornecedor == null)
            {
                return HttpNotFound();
            }
            return View(fornecedor);
        }

        // GET: Fornecedores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fornecedores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FornecedorID, Nome")] Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                fornecedor.CriadoEm = DateTime.Now;
                fornecedor.ModificadoEm = DateTime.Now;

                db.Fornecedor.Add(fornecedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fornecedor);
        }

        // GET: Fornecedores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fornecedor fornecedor = db.Fornecedor.Find(id);
            if (fornecedor == null)
            {
                return HttpNotFound();
            }
            return View(fornecedor);
        }

        // POST: Fornecedores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FornecedorID, Nome")] Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                fornecedor.CriadoEm = DateTime.Now;
                fornecedor.ModificadoEm = DateTime.Now;

                db.Entry(fornecedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fornecedor);
        }

        // GET: Fornecedores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fornecedor fornecedor = db.Fornecedor.Find(id);
            if (fornecedor == null)
            {
                return HttpNotFound();
            }
            return View(fornecedor);
        }

        // POST: Fornecedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fornecedor fornecedor = db.Fornecedor.Find(id);
            db.Fornecedor.Remove(fornecedor);
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
