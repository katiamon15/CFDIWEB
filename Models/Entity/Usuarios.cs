using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFDIWEB.Models.Entity
{
    public class Usuarios
    {
        [Key]
        public int id { get; set; }

        public string usuario { get; set; }

        public string contraseña {get; set; }
    }
}
