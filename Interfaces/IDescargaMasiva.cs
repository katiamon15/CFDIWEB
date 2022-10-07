using CFDIWEB.Enumerations;
using System.Drawing;

namespace CFDIWEB.Interfaces
{

    public interface IDescargaMasiva
    {
        public void DescargaCFDI(byte[] byteArray, String keyFile);

        public void SolicitudCFDI( );

    }


}
