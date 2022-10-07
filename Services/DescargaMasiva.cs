using CFDIWEB.Enumerations;
using CFDIWEB.Helpers;
using CFDIWEB.Interfaces;
using CFDIWEB.Models;
using System.Security.Cryptography.X509Certificates;

namespace CFDIWEB.Services
{
    public class DescargaMasiva : IDescargaMasiva
    {
        private readonly ILogger<DescargaMasiva> _logger;
        private IAutenticacionService _authService;

        public DescargaMasiva(ILogger<DescargaMasiva> logger, IAutenticacionService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        public async void DescargaCFDI(byte[] byteArray, String keyFile)
        {
        

            CancellationTokenSource cancellationTokenSource = new();
            CancellationToken cancellationToken = cancellationTokenSource.Token;


            _logger.LogInformation("Iniciando ejemplo de como utilizar los servicios para descargar los CFDIs recibidos del dia de hoy.");

            // Parametros de ejemplo
            //var rutaCertificadoPfx =@"" + pathFile;

            byte[] certificadoPfx = byteArray;
            var certificadoPassword = keyFile;
            _logger.LogInformation("Creando el certificado SAT con el certificado PFX y contrasena.");
            X509Certificate2 certificadoSat = X509Certificate2Helper.GetCertificate(certificadoPfx, certificadoPassword);

            // Autenticacion
            _logger.LogInformation("Buscando el servicio de autenticacion en el contenedor de servicios (Dependency Injection).");
            var autenticacionService = _authService;

            _logger.LogInformation("Creando solicitud de autenticacion.");
            var autenticacionRequest = AutenticacionRequest.CreateInstance();

            _logger.LogInformation("Enviando solicitud de autenticacion.");
            AutenticacionResult autenticacionResult =
                await autenticacionService.SendSoapRequestAsync(autenticacionRequest, certificadoSat, cancellationToken);

            if (!autenticacionResult.AccessToken.IsValid)
            {
                _logger.LogError("La solicitud de autenticacion no fue exitosa. FaultCode:{0} FaultString:{1}",
                    autenticacionResult.FaultCode,
                    autenticacionResult.FaultString);
                throw new Exception();
            }

            _logger.LogInformation("La solicitud de autenticacion fue exitosa. AccessToken:{0}", autenticacionResult.AccessToken.DecodedValue);
        }

        
    }

}
