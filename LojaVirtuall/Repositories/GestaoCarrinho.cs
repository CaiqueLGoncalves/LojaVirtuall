using LojaVirtuall.Models;
using System.Collections.Generic;

namespace LojaVirtuall.Repositories
{
    public class GestaoCarrinho
    {
        public static Dictionary<int, int> lista;
        const string CHAVE_PEDIDOS = "Carrinho_LojaVirtuall";

        public static Dictionary<Produto, int> RetornarCarrinho()
        {
            Contexto db = new Contexto();

            if (RetornarProdutos() != null)
            {
                lista = RetornarProdutos();
            }
            else
            {
                CriarCarrinho();
            }

            Dictionary<Produto, int> listaProdutos = new Dictionary<Produto, int>();

            foreach (var item in lista)
            {
                listaProdutos.Add(db.Produto.Find(item.Key), item.Value);
            }

            return listaProdutos;
        }

        public static double RetornarTotalCarrinho()
        {
            double total = 0.0;

            foreach (var item in RetornarCarrinho())
            {
                total += (item.Key.Preco * item.Value);
            }

            return total;
        }

        public static void AdicionarProdutoCarrinho(int produtoID, int quantidade)
        {
            if (RetornarProdutos() != null)
            {
                lista = RetornarProdutos();
            }
            else
            {
                CriarCarrinho();
            }

            Contexto db = new Contexto();

            if (db.Produto.Find(produtoID).Quantidade > 0)
            {
                if (lista.ContainsKey(produtoID))
                {
                    IncrementarProdutoCarrinho(produtoID);
                }
                else
                {
                    lista.Add(produtoID, quantidade);
                }
            }

            AtualizarLista();
        }

        public static void RemoverProdutoCarrinho(int produtoID)
        {
            lista = RetornarProdutos();
            lista.Remove(produtoID);
            AtualizarLista();
        }

        public static void AtualizarProdutoCarrinho(int produtoID, int novaQuantidade)
        {
            lista = RetornarProdutos();
            lista[produtoID] = novaQuantidade;
            AtualizarLista();
        }

        public static void IncrementarProdutoCarrinho(int produtoID)
        {
            Contexto db = new Contexto();
            int max = db.Produto.Find(produtoID).Quantidade;

            lista = RetornarProdutos();

            if (lista[produtoID] < max)
            {
                lista[produtoID]++;
            }
            else if (lista[produtoID] > max)
            {
                lista[produtoID] = max;
            }

            AtualizarLista();
        }

        public static void DecrementarProdutoCarrinho(int produtoID)
        {
            lista = RetornarProdutos();

            if (lista[produtoID] > 1)
            {
                lista[produtoID]--;
            }

            AtualizarLista();
        }

        public static void LimparCarrinho()
        {
            lista.Clear();
            AtualizarLista();
        }

        public static bool EstaNoCarrinho(int produtoID)
        {
            lista = RetornarProdutos();

            if (lista == null)
            {
                CriarCarrinho();
            }

            return lista.ContainsKey(produtoID);
        }

        private static void CriarCarrinho()
        {
            lista = new Dictionary<int, int>();
        }

        private static void AtualizarLista()
        {
            System.Web.HttpContext.Current.Session[CHAVE_PEDIDOS] = lista;
        }

        private static Dictionary<int, int> RetornarProdutos()
        {
            var carrinho = System.Web.HttpContext.Current.Session[CHAVE_PEDIDOS];

            if (carrinho != null)
            {
                return carrinho as Dictionary<int, int>;
            }
            else
            {
                return null;
            }
        }
    }
}