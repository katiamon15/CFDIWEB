using CFDIWEB.Enumerations;
using CFDIWEB.Models;
using System.Drawing;

namespace CFDIWEB.Interfaces
{

    public interface IDescargaMasiva
    {
        public Task DescargaCFDI(Session session);
    }

    public interface ISolicitudDescarga
    {
        public void SolicitarCFDI(String uuid, DateTime FechaInicial, DateTime FechaFin, String rfcReceptores,String rfcEmisor,String rfcSolicitante, int tipoComprobante,String RfcACuentaTerceros, String Complemento);
    }


}
