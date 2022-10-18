using CFDIWEB.Enumerations;
using CFDIWEB.Models;
using CFDIWEB.Models.Forms;
using System.Drawing;

namespace CFDIWEB.Interfaces
{

    public interface IDescargaMasiva
    {
        public Task DescargaCFDI(Session session);

        public Task SolicitudCFDI(SolicitudForm solicitudF,Session session);

        public Task VerificacionCFDI(VerificacionForm verificacionF,Session session);


    }

}
