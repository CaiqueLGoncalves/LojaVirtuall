﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojaVirtuall.Models
{
    public class Fornecedor
    {
        [Key]
        public int FornecedorID { get; set; }

        [StringLength(100, ErrorMessage = "O campo 'Nome' deve ter no máximo 100 caracteres.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo 'Nome' está vazio.")]
        public string Nome { get; set; }

        public virtual ICollection<Produto> Produtos { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Criado Em")]
        public DateTime? CriadoEm { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Modificado Em")]
        public DateTime? ModificadoEm { get; set; }

        [DisplayName("Criado Por")]
        public Administrador CriadoPor { get; set; }

        [DisplayName("Modificado Por")]
        public Administrador ModificadoPor { get; set; }
    }
}