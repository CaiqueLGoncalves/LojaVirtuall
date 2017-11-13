using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LojaVirtuall.Models
{
    public abstract class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }

        [StringLength(100, ErrorMessage = "O campo 'Nome' deve ter no máximo 100 caracteres.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Nome' está vazio.")]
        public string Nome { get; set; }

        [StringLength(100, ErrorMessage = "O campo 'E-mail' deve ter no máximo 100 caracteres.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'E-mail' está vazio.")]
        public string Email { get; set; }

        [MinLength(6, ErrorMessage = "O campo 'Login' deve ter no mínimo 6 caracteres.")]
        [StringLength(32, ErrorMessage = "O campo 'Login' deve ter no máximo 32 caracteres.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Login' está vazio.")]
        public string Login { get; set; }

        [MinLength(8, ErrorMessage = "O campo 'Senha' deve ter no mínimo 8 caracteres.")]
        [StringLength(32, ErrorMessage = "O campo 'Senha' deve ter no máximo 32 caracteres.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Senha' está vazio.")]
        public string Senha { get; set; }

        [DefaultValue(true)]
        public bool Ativo { get; set; }

        [DisplayName("Criado Em")]
        public DateTime? CriadoEm { get; set; }

        [DisplayName("Modificado Em")]
        public DateTime? ModificadoEm { get; set; }
    }
}