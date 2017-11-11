using System.Data.Entity;

namespace LojaVirtuall.Models
{
    public class Contexto : DbContext
    {
        public Contexto() : base("name=Contexto") { }

        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<Produto> Produto { get; set; }
    }
}