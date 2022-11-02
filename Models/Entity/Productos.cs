using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFDIWEB.Models.Entity
{
    public class Productos
    {
        public int claveprodserv { get; set; }
        public string descripcion { get; set; }
        [Key]
        public int idproducto { get; set; }
    }
}
