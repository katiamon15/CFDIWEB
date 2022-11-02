namespace CFDIWEB.Models.ComprobanteLectura
{
    public class Cfdi
    {
        public String? Importe { get; set; }
        public String? RFCEmisor { get; set; }
        public String? RFCReceptor { get; set; }
        public String? NombreProveedor { get; set; }
        public String? Foliofactura { get; set; }
        public String? Cuenta { get; set; }
        public String? Total { get; set; }
        public List<Concepto>? Conceptos { get; set; }
        public TimbreFiscal? TimbreFislcal { get; set; }
  
    } 
}
