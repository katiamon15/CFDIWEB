namespace CFDIWEB.Models
{
    public class SolicitudTable
    { 
        public string UUID { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFin { get; set; }
        public string rfcEmisor { get; set; }
        public string rfcReceptores { get; set; }
        public string rfcSolicitante { get; set; }
        public string TipoSolicitud { get; set; }
        public string TipoComprobante { get; set; }
        public string EstadoComprobante { get; set; }
        public string Complemento { get; set; }
        public string CodEstatus { get; set; }
        public string Mensaje { get; set; }
        public string IdSolicitudSat { get; set; }
        public string EstadoSolicitud { get; set; }
    }
}
