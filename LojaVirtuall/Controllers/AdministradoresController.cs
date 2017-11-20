using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LojaVirtuall.Models;
using System;
using System.Web.UI;
using System.Security.Cryptography;
using System.Text;
using LojaVirtuall.Repositories;

namespace LojaVirtuall.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class AdministradoresController : BaseController
    {
        private Contexto db = new Contexto();

        // GET: Administradores
        public ActionResult Index()
        {
            return View(db.Administrador.ToList());
        }

        // GET: Administradores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrador administrador = db.Administrador.Find(id);
            if (administrador == null)
            {
                return HttpNotFound();
            }
            return View(administrador);
        }

        // GET: Administradores/Create
        public ActionResult Create()
        {
            if (GestaoUsuarios.VerificarStatusAdministrador() != null)
            {
                return View();
            }
            return null;
        }

        // POST: Administradores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UsuarioID, Nome, Email, Login, Senha, ConfirmacaoSenha, Ativo")] Administrador administrador)
        {
            if (ModelState.IsValid)
            {
                administrador.Senha = CalculateMD5String(administrador.Senha);
                administrador.ConfirmacaoSenha = CalculateMD5String(administrador.ConfirmacaoSenha);
                administrador.CriadoEm = DateTime.Now;
                administrador.ModificadoEm = DateTime.Now;

                db.Administrador.Add(administrador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(administrador);
        }

        // GET: Administradores/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Administradores/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "UsuarioID, Nome, Email, Login, Senha, ConfirmacaoSenha")] Administrador administrador)
        {
            if (ModelState.IsValid)
            {
                administrador.Senha = CalculateMD5String(administrador.Senha);
                administrador.ConfirmacaoSenha = CalculateMD5String(administrador.ConfirmacaoSenha);
                administrador.Ativo = true;
                administrador.CriadoEm = DateTime.Now;
                administrador.ModificadoEm = DateTime.Now;

                db.Administrador.Add(administrador);
                db.SaveChanges();
                return RedirectToAction("Login", "Home");
            }

            return View(administrador);
        }

        // GET: Administradores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (GestaoUsuarios.VerificarStatusAdministrador() != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Administrador administrador = db.Administrador.Find(id);
                administrador.Senha = null;
                administrador.ConfirmacaoSenha = null;

                if (administrador == null)
                {
                    return HttpNotFound();
                }
                return View(administrador);
            }
            return null;
        }

        // POST: Administradores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UsuarioID, Nome, Email, Login, Senha, ConfirmacaoSenha, Ativo")] Administrador administrador)
        {
            if (ModelState.IsValid)
            {
                administrador.Senha = CalculateMD5String(administrador.Senha);
                administrador.ConfirmacaoSenha = CalculateMD5String(administrador.ConfirmacaoSenha);
                administrador.CriadoEm = DateTime.Now;
                administrador.ModificadoEm = DateTime.Now;

                db.Entry(administrador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(administrador);
        }

        // GET: Administradores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (GestaoUsuarios.VerificarStatusAdministrador() != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Administrador administrador = db.Administrador.Find(id);
                if (administrador == null)
                {
                    return HttpNotFound();
                }
                return View(administrador);
            }

            return null;
        }

        // POST: Administradores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Administrador administrador = db.Administrador.Find(id);
            db.Administrador.Remove(administrador);
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

        private string CalculateMD5String(string entrada)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(entrada);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        private bool VerificarDisponibilidadeEmail(string email)
        {
            var admin = db.Administrador.Where(c => c.Email == email);
            var cliente = db.Cliente.Where(c => c.Email == email);

            return (admin == null && cliente == null);
        }

        private bool VerificarDisponibilidadeLogin(string login)
        {
            var admin = db.Administrador.Where(c => c.Login == login);
            var cliente = db.Cliente.Where(c => c.Login == login);

            return (admin == null && cliente == null);
        }
    }
}