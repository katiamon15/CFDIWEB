using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFDIWEB.Models.Entity
{
    public class Provedores
    {
      public string rfc { get; set; }
       public string nombre { get; set; }
        [Key]
        public int idprovedor { get; set; }
    }
}
