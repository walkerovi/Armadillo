﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Armadillo.Models
{
    public class Dato
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int NoFila { get; set; }
        public int Indice { get; set; }
        public int IdCampo { get; set; }
        public Campo Campo { get; set; }
        public string Valor { get; set; }
    }
}
