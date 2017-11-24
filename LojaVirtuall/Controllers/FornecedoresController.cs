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
    public class FornecedoresController : BaseController
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
            if (GestaoUsuarios.VerificarStatusAdministrador() != null)
            {
                return View();
            }

            return null;
        }

        // POST: Fornecedores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FornecedorID, Nome")] Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                int idUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["ID"].ToString());

                fornecedor.CriadoEm = DateTime.Now;
                fornecedor.ModificadoEm = DateTime.Now;
                fornecedor.CriadoPor = db.Administrador.Find(idUser);
                fornecedor.ModificadoPor = db.Administrador.Find(idUser);

                db.Fornecedor.Add(fornecedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fornecedor);
        }

        // GET: Fornecedores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (GestaoUsuarios.VerificarStatusAdministrador() != null)
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
            return null;
        }

        // POST: Fornecedores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FornecedorID, Nome")] Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                int idUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["ID"].ToString());

                fornecedor.CriadoEm = DateTime.Now; // Não conseguimos ajustar isto.
                fornecedor.ModificadoEm = DateTime.Now;
                fornecedor.CriadoPor = db.Administrador.Find(idUser); // Não conseguimos ajustar isto.
                fornecedor.ModificadoPor = db.Administrador.Find(idUser);

                db.Entry(fornecedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fornecedor);
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
