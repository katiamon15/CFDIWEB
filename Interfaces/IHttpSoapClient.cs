using System.Threading;
using System.Threading.Tasks;
using CFDIWEB.Models;

namespace CFDIWEB.Interfaces
{
    public interface IHttpSoapClient
    {
        Task<SoapRequestResult> SendRequestAsync(string url,
                                                 string soapAction,
                                                 AccessToken accessToken,
                                                 string requestContent,
                                                 CancellationToken cancellationToken);
    }
}