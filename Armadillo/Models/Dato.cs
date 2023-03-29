using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Armadillo.Models
{
    public class Dato
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Indice { get; set; }
        public Campo Campo { get; set; }
        public string Valor { get; set; }
    }
}
