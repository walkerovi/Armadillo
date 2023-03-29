using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Armadillo.Models
{
    public class Campo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Indice { get; set; }

        public Tipo Tipo { get; set; }

        public string Nombre { get; set; }
        public Hoja Hoja { get; set; }
        public string Calculo { get; set; }
        public ICollection<Dato> Datos { get; set; }
    }
}
