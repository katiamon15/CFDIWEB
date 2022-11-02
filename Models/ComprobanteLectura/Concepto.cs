namespace CFDIWEB.Models.ComprobanteLectura
{
    public class Concepto
    {
        public String? ClaveOServicio { get; set; }
        public String? Importe { get; set; }
        public String? Unidad { get; set; }
        public String? Descripcion { get; set; }
        public String? DescripcionCatalogo { get; set; }
        public List<ImpuestoConcepto>? Impuestos { get; set; }
    }
}
