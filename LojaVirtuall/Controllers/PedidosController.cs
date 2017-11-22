using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LojaVirtuall.Models;
using System.Web.UI;
using LojaVirtuall.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;

namespace LojaVirtuall.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class PedidosController : BaseController
    {
        private Contexto db = new Contexto();

        public ActionResult EmitirPedido()
        {
            if (GestaoUsuarios.VerificarStatusCliente() != null)
            {
                Pedido novoPedido = new Pedido();
                List<ItemPedido> listaItens = new List<ItemPedido>();

                var carrinho = GestaoCarrinho.RetornarCarrinho();

                if (carrinho.Count > 0)
                {
                    // Cria novo pedido
                    novoPedido.DataPedido = DateTime.Now;
                    novoPedido.UsuarioID = Convert.ToInt32(System.Web.HttpContext.Current.Session["ID"].ToString());
                    novoPedido.Cliente = db.Cliente.Find(novoPedido.UsuarioID);
                    novoPedido.Ativo = true;
                    novoPedido.ModificadoEm = null;
                    novoPedido.ModificadoPor = null;

                    db.Pedido.Add(novoPedido);
                    db.SaveChanges();

                    // Adiciona os itens do pedido
                    foreach (var item in carrinho)
                    {
                        ItemPedido itemPedido = new ItemPedido
                        {
                            ProdutoID = item.Key.ProdutoID,
                            PedidoID = novoPedido.PedidoID,
                            Quantidade = item.Value
                        };

                        db.ItemPedido.Add(itemPedido);
                        db.SaveChanges();

                        // Remove produtos do estoque
                        Produto produto = db.Produto.Find(itemPedido.ProdutoID);
                        produto.Quantidade -= itemPedido.Quantidade;
                        db.Produto.AddOrUpdate(produto);
                        db.SaveChanges();
                    }
                }

                // Limpa o carrinho
                GestaoCarrinho.LimparCarrinho();

                return RedirectToAction("ExibirPedido", new { id = novoPedido.PedidoID });
            }

            return null;
        }

        public ActionResult ExibirPedido(int? id)
        {
            if (GestaoUsuarios.VerificarStatusCliente() != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Pedido pedido = db.Pedido.Find(id);

                if (pedido == null)
                {
                    return HttpNotFound();
                }

                if (pedido.UsuarioID == Convert.ToInt32(System.Web.HttpContext.Current.Session["ID"].ToString()))
                {
                    pedido.Cliente = db.Cliente.Find(pedido.UsuarioID);

                    foreach (var item in pedido.Itens)
                    {
                        item.Produto = db.Produto.Find(item.ProdutoID);
                    }

                    return View(pedido);
                }
                else
                {
                    return RedirectToAction("MeusPedidos", "Pedidos");
                }
            }

            return null;
        }

        public ActionResult MeusPedidos()
        {
            if (GestaoUsuarios.VerificarStatusCliente() != null)
            {
                var id = Convert.ToInt32(System.Web.HttpContext.Current.Session["ID"].ToString());
                var pedidos = db.Pedido.Where(p => p.UsuarioID == id).Include(p => p.Cliente);
                return View(pedidos.ToList());
            }

            return null;
        }

        public JsonResult CancelarPedido(int id)
        {
            if (GestaoUsuarios.VerificarStatusCliente() != null)
            {
                var idUsuario = Convert.ToInt32(System.Web.HttpContext.Current.Session["ID"].ToString());

                Pedido pedido = db.Pedido.Find(id);

                if (pedido.UsuarioID == idUsuario)
                {
                    DateTime dataAtual = DateTime.Now;
                    DateTime dataPedido = Convert.ToDateTime(pedido.DataPedido);

                    double horas = (dataAtual - dataPedido).TotalHours;

                    if (horas <= 6)
                    {
                        if (pedido.Ativo)
                        {
                            // Desativar o Pedido
                            pedido.Ativo = false;
                            db.Pedido.AddOrUpdate(pedido);
                            int resultado = db.SaveChanges();

                            if (resultado > 0)
                            {
                                foreach (var item in pedido.Itens)
                                {
                                    Produto produto = db.Produto.Find(item.ProdutoID);
                                    produto.Quantidade += item.Quantidade;
                                    db.Produto.AddOrUpdate(produto);
                                    db.SaveChanges();
                                }

                                return Json(new { OK = true, Mensagem = "Pedido cancelado com sucesso." }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { OK = false, Mensagem = "Erro ao cancelar o pedido." }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { OK = false, Mensagem = "Este pedido já foi cancelado anteriormente." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { OK = false, Mensagem = "Após seis horas o pedido não pode ser mais cancelado." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { OK = false, Mensagem = "Não é possível cancelar um pedido de outro usuário." }, JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }

        public ActionResult Index()
        {
            var pedido = db.Pedido.Include(p => p.Cliente);
            return View(pedido.ToList());
        }

        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Pedido pedido = db.Pedido.Find(id);

            if (pedido == null)
            {
                return HttpNotFound();
            }

            pedido.Cliente = db.Cliente.Find(pedido.UsuarioID);

            foreach (var item in pedido.Itens)
            {
                item.Produto = db.Produto.Find(item.ProdutoID);
            }

            return View(pedido);
        }

        public ActionResult CancelOrder(int id)
        {
            Pedido pedido = db.Pedido.Find(id);

            if (pedido.Ativo)
            {
                int idAdmin = Convert.ToInt32(System.Web.HttpContext.Current.Session["ID"].ToString());
                Administrador admin = db.Administrador.Find(idAdmin);

                // Desativar o Pedido
                pedido.Ativo = false;
                pedido.ModificadoEm = DateTime.Now;
                pedido.ModificadoPor = admin.Nome;
                db.Pedido.AddOrUpdate(pedido);
                int resultado = db.SaveChanges();

                if (resultado > 0)
                {
                    foreach (var item in pedido.Itens)
                    {
                        Produto produto = db.Produto.Find(item.ProdutoID);
                        produto.Quantidade += item.Quantidade;
                        produto.ModificadoEm = DateTime.Now;
                        produto.ModificadoPor = admin;
                        db.Produto.AddOrUpdate(produto);
                        db.SaveChanges();
                    }     
                }
            }

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