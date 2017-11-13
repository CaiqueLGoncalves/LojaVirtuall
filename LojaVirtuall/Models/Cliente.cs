using System.Collections.Generic;

namespace LojaVirtuall.Models
{
    public class Cliente : Usuario
    {
        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}