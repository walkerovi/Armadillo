using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Armadillo.Models
{
    public class Hoja
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int IdPrograma { get; set; }
        public Programa Programa { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public ICollection<Campo> Campos { get; set; }
    }
}
