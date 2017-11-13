using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojaVirtuall.Models
{
    public class ItemPedido
    {
        [Key]
        public int ItemPedidoID { get; set; }

        public int PedidoID { get; set; }

        [Key]
        [ForeignKey("PedidoID")]
        public Pedido Pedido { get; set; }

        public int ProdutoID { get; set; }

        [ForeignKey("ProdutoID")]
        public Produto Produto { get; set; }

        [DefaultValue(1)]
        [Range(1, Int32.MaxValue)]
        [Required(ErrorMessage = "O campo 'Quantidade' está vazio.")]
        public int Quantidade { get; set; }
    }
}