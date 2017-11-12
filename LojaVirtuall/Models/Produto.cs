using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojaVirtuall.Models
{
    public class Produto
    {
        [Key]
        public int ProdutoID { get; set; }

        [StringLength(100, ErrorMessage = "O campo 'Nome' deve ter no máximo 100 caracteres.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Nome' está vazio.")]
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [DisplayName("Preço")]
        [Required(ErrorMessage = "O campo 'Preço' está vazio.")]
        public double Preco { get; set; }

        [DefaultValue(0)]
        [Range(0, Int32.MaxValue)]
        [Required(ErrorMessage = "O campo 'Quantidade' está vazio.")]
        public int Quantidade { get; set; }

        [DefaultValue(true)]
        public bool Ativo { get; set; }

        [DisplayName("Criado Em")]
        public DateTime? CriadoEm { get; set; }

        [DisplayName("Modificado Em")]
        public DateTime? ModificadoEm { get; set; }

        public int CategoriaID { get; set; }

        [ForeignKey("CategoriaID")]
        public Categoria Categoria { get; set; }

        public int FornecedorID { get; set; }

        [ForeignKey("FornecedorID")]
        public Fornecedor Fornecedor { get; set; }
    }
}