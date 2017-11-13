using System.Data.Entity;

namespace LojaVirtuall.Models
{
    public class Contexto : DbContext
    {
        public Contexto() : base("name=Contexto") { }

        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Administrador> Administrador { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<ItemPedido> ItemPedido { get; set; }
    }
}