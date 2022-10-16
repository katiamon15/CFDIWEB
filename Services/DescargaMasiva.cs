using CFDIWEB.Enumerations;
using CFDIWEB.Helpers;
using CFDIWEB.Interfaces;
using CFDIWEB.Models;
using CFDIWEB.Models.Forms;
using CFDIWEB.Models.Entity;
using CFDIWEB.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;


namespace CFDIWEB.Services
{
    public class DescargaMasiva : IDescargaMasiva
    {
        private readonly ILogger<DescargaMasiva> _logger;
        private IAutenticacionService _authService;
        private ISolicitudService _solicitudService;
        private MyAppDbContext _context;
        private IHttpContextAccessor _accessor;


        public DescargaMasiva(ILogger<DescargaMasiva> logger, IAutenticacionService authService,ISolicitudService solicitudService
            , MyAppDbContext context, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _authService = authService;
            _solicitudService = solicitudService;
            _context = context;
            _accessor = accessor;
        }

        public async Task DescargaCFDI(Session session)
        {


            CancellationTokenSource cancellationTokenSource = new();
            CancellationToken cancellationToken = cancellationTokenSource.Token;


            _logger.LogInformation("Iniciando ejemplo de como utilizar los servicios para descargar los CFDIs recibidos del dia de hoy.");

            // Parametros de ejemplo
            //var rutaCertificadoPfx =@"" + pathFile;

            byte[] certificadoPfx = Convert.FromBase64String(session.PfxUrl);
            var certificadoPassword = session.PfxPassword;

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

            session.TokenSat = autenticacionResult.AccessToken.DecodedValue;
            //session.Timestamp = GetTimestamp(DateTime.Now);
            _logger.LogInformation("La solicitud de autenticacion fue exitosa. AccessToken:{0}", autenticacionResult.AccessToken.DecodedValue);
        }

        //solicitud
        

        public async void DescargaCFDI(SolicitudForm solicitudF)
        {
            HttpContext context = _accessor.HttpContext;

            CancellationTokenSource cancellationTokenSource = new();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            byte[] certificadoPfx = Encoding.ASCII.GetBytes(context.Session.GetString("pfx"));
            var certificadoPassword = context.Session.GetString("passwordpfx");

            _logger.LogInformation("Creando el certificado SAT con el certificado PFX y contrasena.");
            X509Certificate2 certificadoSat = X509Certificate2Helper.GetCertificate(certificadoPfx, certificadoPassword);

            //private void creaSolicitudAlmacen(){}

            // Metodo solicitud Solicitud
            // Primero llamar al metodo para crear y almacenar en base de datos
            //Datos desde el front
            DateTime fechaInicio = solicitudF.FechaInicial;
            DateTime fechaFin = solicitudF.FechaFin;
            TipoSolicitud? tipoSolicitud = TipoSolicitud.FromValue(solicitudF.TipoSolicitud);
            var rfcEmisor = solicitudF.rfcEmisor;

            string CADENAUSUARIO = "SDKFSLKF,KDSÑLFKASKFD,SKDFÑLAKDÑLSF,DÑFSDÑFLSFDÑL,LDFLALSDÑFLA{SFL";
            string[] arreglo = CADENAUSUARIO.Split(",");
            string cadenaReconstruido = string.Join(",", arreglo);
            var myArray = new[] { cadenaReconstruido };
            List<string> myList = myArray.ToList();
            var rfcReceptores = myList;  
            var rfcSolicitante = solicitudF.rfcSolicitante;



            //solicitudEntity.uuid = "111AAA1A-1AA1-1A11-11A1-11A1AA111A11";

            _logger.LogInformation("Buscando el servicio de solicitud de descarga en el contenedor de servicios (Dependency Injection).");

    
            AccessToken accessToken=new AccessToken(""); 
            try
            {
                string token = context.Session.GetString("tokenSat");
                accessToken = new AccessToken(token);
                
            }catch(Exception e){
                _logger.LogError("La session no contiene el token del sat");
                throw new Exception("No se pudo obtener el token del SAT");
            }


            _logger.LogInformation("Creando solicitud de solicitud de descarga.");
            var solicitudRequest = SolicitudRequest.CreateInstance(fechaInicio,
                fechaFin,
                tipoSolicitud,
                rfcEmisor,
                rfcReceptores,
                rfcSolicitante,
                accessToken);


            _logger.LogInformation("Enviando solicitud de solicitud de descarga.");
            SolicitudResult solicitudResult = await _solicitudService.SendSoapRequestAsync(solicitudRequest, certificadoSat, cancellationToken);

            /*SolicitudResult solicitudResult = SolicitudResult.CreateInstance("a804e8a7-efac-4333-92ac-83a79c649f2d",
                "5000", "Solicitud aceptada", HttpStatusCode.Accepted, "Mensaje respuesta SAT");*/

            Solicitud solicitudEntity = new Solicitud();
            solicitudEntity.FechaInicial = fechaInicio;
            solicitudEntity.FechaFin = fechaFin;
            solicitudEntity.rfcEmisor = rfcEmisor;
            solicitudEntity.rfcReceptores = rfcReceptores[0];
            solicitudEntity.rfcSolicitante = rfcSolicitante;
            solicitudEntity.TipoSolicitud = tipoSolicitud.Value;
            solicitudEntity.Complemento = 1;
            solicitudEntity.IdSolicitudSat = solicitudResult.RequestId;
            solicitudEntity.CodEstatus = solicitudResult.RequestStatusCode;
            solicitudEntity.Mensaje = solicitudResult.RequestStatusMessage;

            try
            {
                _context.Solicitud.Add(solicitudEntity);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
                throw new Exception("Problemas al insertar en la base de datos");
            }

            if (string.IsNullOrEmpty(solicitudResult.RequestId))
            {
                _logger.LogError("La solicitud de solicitud de descarga no fue exitosa. RequestStatusCode:{0} RequestStatusMessage:{1}",
                    solicitudResult.RequestStatusCode,
                    solicitudResult.RequestStatusMessage);
                throw new Exception();
            }

            _logger.LogInformation("La solicitud de solicitud de descarga fue exitosa. RequestId:{0}", solicitudResult.RequestId);


        }
    }

   
    
   

}
