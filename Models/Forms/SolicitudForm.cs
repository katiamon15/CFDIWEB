
using System.ComponentModel.DataAnnotations;

namespace CFDIWEB.Models.Forms
{
    public class SolicitudForm
    {
        public int IdSolicitudIntern { get; set; }
        public string UUID { get; set; }
        [Required(ErrorMessage = "Introduce la Fecha Inical")]
        public DateTime FechaInicial { get; set; }
        [Required(ErrorMessage = "Introduce la Fecha Final")]
        public DateTime FechaFin { get; set; }
        public string rfcEmisor { get; set; }
        [Required(ErrorMessage = "Introduce el RFC del Receptor")]
        [StringLength(13,ErrorMessage = "Longitud de 13 caracteres.",
                      MinimumLength = 13)]
        [RegularExpression(@"^([A-ZÑ\x26]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1]))([A-Z\d]{3})?$",
            ErrorMessage = "El RFC introducido no es valido")]
        public string rfcReceptores { get; set; }
        [Required(ErrorMessage = "Introduce el RFC del Solicitante")]
        [StringLength(13, ErrorMessage = "Longitud de 13 caracteres.",
                      MinimumLength = 13)]
        [RegularExpression(@"^([A-ZÑ\x26]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1]))([A-Z\d]{3})?$",
            ErrorMessage = "El RFC introducido no es valido")]
        public string rfcSolicitante { get; set; }
        public int TipoSolicitud { get; set; }
        public string TipoComprobante { get; set; }
        public int EstadoComprobante { get; set; }
        public int Complemento { get; set; }
        public byte[] signature { get; set; }
        public string CodEstatus { get; set; }
        public string Mensaje { get; set; }
        public string IdSolicitudSat { get; set; }
        public string EstadoSolicitud { get; set; }
    }
}
