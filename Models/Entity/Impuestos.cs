using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFDIWEB.Models.Entity
{
    public class Impuestos
    {
        [Key]
        public  string tipoimpuesto { get; set; }

        public string descripcion { get; set; }
    }
}
