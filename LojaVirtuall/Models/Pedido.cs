using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojaVirtuall.Models
{
    public class Pedido
    {
        [Key]
        public int PedidoID { get; set; }

        [DisplayName("Data do Pedido")]
        public DateTime? DataPedido { get; set; }

        public int UsuarioID { get; set; }

        [ForeignKey("UsuarioID")]
        public Cliente Cliente { get; set; }

        [DefaultValue(true)]
        public bool Ativo { get; set; }

        public virtual ICollection<ItemPedido> Itens { get; set; }

        [DisplayName("Modificado Em")]
        public DateTime? ModificadoEm { get; set; }

        [DisplayName("Modificado Por")]
        public Administrador ModificadorPor { get; set; }
    }
}