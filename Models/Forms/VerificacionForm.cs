namespace CFDIWEB.Models.Forms
{
    public class VerificacionForm
    {
        public string IdSolicitudSat { get; set; }
        public string rfcSolicitante { get; set; }
        public string Idpaquetes { get; set; }
        public int Estadosolicitud { get; set; }
        public string Codigoestadosolicitud { get; set; }
        public int Numerocfdi { get; set; }
        public string Codestatus { get; set; }
        public string Mensaje { get; set; }
    }
}
