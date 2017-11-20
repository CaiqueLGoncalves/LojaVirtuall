using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LojaVirtuall.Models;
using System.Web.UI;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Data.Entity.Migrations;

namespace LojaVirtuall.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ClientesController : BaseController
    {
        private Contexto db = new Contexto();

        // GET: Clientes
        public ActionResult Index()
        {
            return View(db.Cliente.ToList());
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UsuarioID, Nome, Email, Login, Senha, ConfirmacaoSenha, Ativo")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.Senha = CalculateMD5String(cliente.Senha);
                cliente.ConfirmacaoSenha = CalculateMD5String(cliente.ConfirmacaoSenha);
                cliente.CriadoEm = DateTime.Now;
                cliente.ModificadoEm = DateTime.Now;

                db.Cliente.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // GET: Clientes/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Clientes/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "UsuarioID, Nome, Email, Login, Senha, ConfirmacaoSenha")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.Senha = CalculateMD5String(cliente.Senha);
                cliente.ConfirmacaoSenha = CalculateMD5String(cliente.ConfirmacaoSenha);
                cliente.Ativo = true;
                cliente.CriadoEm = DateTime.Now;
                cliente.ModificadoEm = DateTime.Now;

                db.Cliente.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Login", "Home");
            }

            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cliente cliente = db.Cliente.Find(id);
            cliente.Senha = null;
            cliente.ConfirmacaoSenha = null;

            if (cliente == null)
            {
                return HttpNotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UsuarioID, Nome, Email, Login, Senha, ConfirmacaoSenha, Ativo")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.Senha = CalculateMD5String(cliente.Senha);
                cliente.ConfirmacaoSenha = CalculateMD5String(cliente.ConfirmacaoSenha);
                cliente.CriadoEm = DateTime.Now;
                cliente.ModificadoEm = DateTime.Now;

                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // GET: Clientes/ManageAccount
        public ActionResult ManageAccount()
        {
            int id = Convert.ToInt32(System.Web.HttpContext.Current.Session["ID"].ToString());
            Cliente cliente = db.Cliente.Find(id);

            if (cliente == null)
            {
                return HttpNotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/ManageAccount
        [HttpPost]
        [ValidateAntiForgeryToken]
        [IgnoreModelErrors("Senha, ConfirmacaoSenha, Ativo, CriadoEm")]
        public ActionResult ManageAccount([Bind(Include = "UsuarioID, Nome, Email, Login")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                string novoNome = cliente.Nome;
                string novoEmail = cliente.Email;
                string novoLogin = cliente.Login;

                cliente = db.Cliente.Find(cliente.UsuarioID);

                cliente.Nome = novoNome;
                cliente.Email = novoEmail;
                cliente.Login = novoLogin;
                cliente.ModificadoEm = DateTime.Now;

                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(cliente);
        }

        // GET: Clientes/ChangePassword
        public ActionResult ChangePassword()
        {
            int id = Convert.ToInt32(System.Web.HttpContext.Current.Session["ID"].ToString());
            Cliente cliente = db.Cliente.Find(id);

            cliente.Senha = null;
            cliente.ConfirmacaoSenha = null;

            if (cliente == null)
            {
                return HttpNotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [IgnoreModelErrors("Nome, Email, Login, Ativo, CriadoEm")]
        public ActionResult ChangePassword([Bind(Include = "UsuarioID, Senha, ConfirmacaoSenha")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                string novaSenha = cliente.Senha;

                cliente = db.Cliente.Find(cliente.UsuarioID);

                cliente.Senha = CalculateMD5String(novaSenha);
                cliente.ConfirmacaoSenha = CalculateMD5String(novaSenha);
                cliente.ModificadoEm = DateTime.Now;

                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Cliente.Find(id);
            db.Cliente.Remove(cliente);
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