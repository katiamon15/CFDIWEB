namespace CFDIWEB.Models
{
    public class SolicitudArhivo
    {
        public string uuid { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFin { get; set; }
        public string rfcemisor { get; set; }
        public string rfcreceptor{get; set;}
        public string rfcsolicitante { get; set;}
        public int tipocomprobante { get; set; }
        public string estadocomprobante { get; set;}
        public string complemento{ get; set; }

    }
}
