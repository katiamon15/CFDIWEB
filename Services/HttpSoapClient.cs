using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using CFDIWEB.Interfaces;
using CFDIWEB.Models;

namespace CFDIWEB.Services
{
    public class HttpSoapClient : IHttpSoapClient
    {
        private readonly HttpClient _httpClient;

        public HttpSoapClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SoapRequestResult> SendRequestAsync(string url,
                                                              string soapAction,
                                                              AccessToken accessToken,
                                                              string requestContent,
                                                              CancellationToken cancellationToken)
        {
            if (url is null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (soapAction is null)
            {
                throw new ArgumentNullException(nameof(soapAction));
            }

            if (accessToken is null)
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            if (requestContent is null)
            {
                throw new ArgumentNullException(nameof(requestContent));
            }

            _httpClient.DefaultRequestHeaders.Clear();

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url));

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Text.Xml));

            request.Headers.Add("SOAPAction", soapAction);

            if (accessToken.IsValid)
            {
                request.Headers.Add("Authorization", accessToken.HttpAuthorizationHeader);
            }

            request.Content = new StringContent(requestContent, Encoding.UTF8, MediaTypeNames.Text.Xml);

            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

            return SoapRequestResult.CreateInstance(response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}