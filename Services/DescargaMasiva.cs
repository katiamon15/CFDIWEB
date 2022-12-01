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
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO.Compression;
using iTextSharp.text.pdf.qrcode;

namespace CFDIWEB.Services
{
    public class DescargaMasiva : IDescargaMasiva
    {
        private readonly ILogger<DescargaMasiva> _logger;
        private IAutenticacionService _authService;
        private ISolicitudService _solicitudService;
        private IVerificacionService _verificacionService;
        private IDescargaService _descargaService;
        private MyAppDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private IAzureStorage _storage;


        public DescargaMasiva(ILogger<DescargaMasiva> logger, IAutenticacionService authService,ISolicitudService solicitudService
            , MyAppDbContext context, IHttpContextAccessor accessor, IVerificacionService verificacionService, IDescargaService descargaService
            , IAzureStorage storage)
        {
            _logger = logger;
            _authService = authService;
            _solicitudService = solicitudService;
            _context = context;
            _accessor = accessor;
            _descargaService = descargaService;
            _verificacionService = verificacionService;
            _storage = storage;
        }

        //Autenticacion
        public async Task DescargaCFDI(Session session)
        {
            var context = _accessor.HttpContext;

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

            if (context != null) { 
                context.Session.SetString("pfx", session.PfxUrl);
                context.Session.SetString("passwordpfx", session.PfxPassword);
                context.Session.SetString("tokenSat", session.TokenSat);
                context.Session.SetString("dateRefresh", DateTime.Now.ToString("dd/MM/yyyyHH:mm:ss"));
            }

            _logger.LogInformation("Obtuvo informacion del token");

        }

        //solicitud

        public async Task SolicitudCFDI(SolicitudForm solicitudF, Session session)
        {

            await DescargaCFDI(session);

            CancellationTokenSource cancellationTokenSource = new();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            _logger.LogInformation("Iniciando ejemplo de como utilizar los servicios para descargar los CFDIs recibidos del dia de hoy.");

            // Parametros de ejemplo
            //var rutaCertificadoPfx =@"" + pathFile;

            byte[] certificadoPfx = Convert.FromBase64String(session.PfxUrl);
            var certificadoPassword = session.PfxPassword;

            _logger.LogInformation("Creando el certificado SAT con el certificado PFX y contrasena.");
            X509Certificate2 certificadoSat = X509Certificate2Helper.GetCertificate(certificadoPfx, certificadoPassword);

            _logger.LogInformation("Fecha actual del token {}",session.Timestamp);

            //private void creaSolicitudAlmacen(){}

            // Metodo solicitud Solicitud
            // Primero llamar al metodo para crear y almacenar en base de datos
            //Datos desde el front
            DateTime fechaInicio = solicitudF.FechaInicial;
            String ejemplo = solicitudF.FechaFin.ToString("dd/MM/yyyyHH:mm:ss");
            DateTime fechaFin = solicitudF.FechaFin;
            TipoSolicitud? tipoSolicitud = TipoSolicitud.FromValue(solicitudF.TipoSolicitud);
            var rfcEmisor = !String.IsNullOrEmpty(solicitudF.rfcEmisor)? solicitudF.rfcEmisor : "";

           
            string[] arreglo = solicitudF.rfcReceptores.Split(",");
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
  
                accessToken = new AccessToken(session.TokenSat);
                
            }catch(Exception e){
                _logger.LogError("La session no contiene el token del sat");
                _logger.LogError(e.Message);
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
               //SolicitudResult solicitudResult = await _solicitudService.SendSoapRequestAsync(solicitudRequest, certificadoSat, cancellationToken);

            SolicitudResult solicitudResult = SolicitudResult.CreateInstance("a804e8a7-efac-4333-92ac-83a79c649f2d",
                "5000", "Solicitud aceptada", HttpStatusCode.Accepted, "Mensaje respuesta SAT");

            Solicitud solicitudEntity = new Solicitud();
            solicitudEntity.FechaInicial = fechaInicio;
            solicitudEntity.FechaFin = fechaFin;
            solicitudEntity.rfcEmisor = rfcEmisor;
            solicitudEntity.rfcReceptores = rfcReceptores[0];
            solicitudEntity.rfcSolicitante = rfcSolicitante;
            solicitudEntity.TipoSolicitud = tipoSolicitud.Value;
            solicitudEntity.Complemento = solicitudF.Complemento;
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

        // Verififcacion 

        public async Task VerificacionCFDI(String idSat, Session session)
        {
            List<Verificacion> etityVerificacion = _context.Verificacion.Where(element => element.IdSolicitudSat == idSat).ToList();

            if (etityVerificacion.Count > 0)
            {
                _logger.LogInformation("La lista no esta vacia");

                List<string> paquetes = new List<string>();

                paquetes.Add(idSat);


                var rutaTarget = @"C:\DesagarCFDIWEB\XMLUNZIP";

                foreach (string paquete in paquetes)
                {
                    var rutaZip = @$"C:\DesagarCFDIWEB\PaquetesZip\{paquete}.zip";

                    ZipFile.ExtractToDirectory(rutaZip, rutaTarget);
                }

                _logger.LogInformation("Proceso terminado.");

            }
            else {
                Solicitud SolicitudEntity = _context.Solicitud.Where(u => u.IdSolicitudSat == idSat)
                          .SingleOrDefault();

                Verificacion verificacionF = new Verificacion();
                verificacionF.IdSolicitudSat = SolicitudEntity.IdSolicitudSat;
                verificacionF.rfcSolicitante = SolicitudEntity.rfcSolicitante;

                await DescargaCFDI(session);

                CancellationTokenSource cancellationTokenSource = new();
                CancellationToken cancellationToken = cancellationTokenSource.Token;

                _logger.LogInformation("Iniciando ejemplo de como utilizar los servicios para descargar los CFDIs recibidos del dia de hoy.");

                // Parametros de ejemplo
                //var rutaCertificadoPfx =@"" + pathFile;

                byte[] certificadoPfx = Convert.FromBase64String(session.PfxUrl);
                var certificadoPassword = session.PfxPassword;

                _logger.LogInformation("Creando el certificado SAT con el certificado PFX y contrasena.");
                X509Certificate2 certificadoSat = X509Certificate2Helper.GetCertificate(certificadoPfx, certificadoPassword);

                AccessToken accessToken = new AccessToken("");
                try
                {

                    accessToken = new AccessToken(session.TokenSat);

                }
                catch (Exception e)
                {
                    _logger.LogError("La session no contiene el token del sat");
                    _logger.LogError(e.Message);
                    throw new Exception("No se pudo obtener el token del sat");
                }


                _logger.LogInformation("Creando solicitud de verificacion.");
                var verificacionRequest = VerificacionRequest.CreateInstance(verificacionF.IdSolicitudSat, verificacionF.rfcSolicitante, accessToken);



                _logger.LogInformation("Enviando solicitud de verificacion.");
                VerificacionResult verificacionResult = await _verificacionService.SendSoapRequestAsync(verificacionRequest,
                    certificadoSat,
                    cancellationToken);

                if (verificacionResult.DownloadRequestStatusNumber != EstadoSolicitud.Terminada.Value.ToString())
                {
                    _logger.LogError(
                        "La solicitud de verificacion no fue exitosa. DownloadRequestStatusNumber:{0} RequestStatusCode:{1} RequestStatusMessage:{2}",
                        verificacionResult.DownloadRequestStatusNumber,
                        verificacionResult.RequestStatusCode,
                        verificacionResult.RequestStatusMessage);

                    throw new Exception();
                }

                if (SolicitudEntity != null)
                {
                    SolicitudEntity.EstadoSolicitud = EstadoSolicitud.FromValue(Int32.Parse(verificacionResult.DownloadRequestStatusNumber)).Name;
                    _context.SaveChanges();_logger.LogError(
                        "El estado de la solicitud no esta Aceptada. Verificar mas tarde");
                    _logger.LogError("Estado de la solicitud {}", SolicitudEntity.EstadoSolicitud);
                }

                /*if (verificacionResult.DownloadRequestStatusNumber != EstadoSolicitud.Aceptada.Value.ToString())
                {
                    
                }

                else
                {
                    _logger.LogInformation(
                        "El estado de la solicitud es Aceptada.");

                }*/


                _logger.LogInformation("La solicitud de verificacion fue exitosa.");


                foreach (string idsPaquete in verificacionResult.PackageIds)
                {
                    Verificacion Entityverificacion = new Verificacion();
                    Entityverificacion.IdSolicitudSat = verificacionF.IdSolicitudSat;
                    Entityverificacion.rfcSolicitante = verificacionF.rfcSolicitante;
                    Entityverificacion.Idpaquetes = idsPaquete;
                    Entityverificacion.Estadosolicitud = Int32.Parse(verificacionResult.DownloadRequestStatusNumber);
                    Entityverificacion.Codigoestadosolicitud = verificacionResult.DownloadRequestStatusCode;
                    Entityverificacion.Numerocfdi = Int32.Parse(verificacionResult.NumberOfCfdis);
                    Entityverificacion.Mensaje = verificacionResult.RequestStatusMessage;
                    Entityverificacion.Codestatus = verificacionResult.RequestStatusCode;
                    _context.Verificacion.Add(Entityverificacion);
                    _context.SaveChanges();

                    _logger.LogInformation("PackageId:{0}", idsPaquete);

                }

                //Descarga

                var rutaDescarga = @"C:\DesagarCFDIWEB\PaquetesZip";

                _logger.LogInformation("Buscando el servicio de verificacion en el contenedor de servicios (Dependency Injection).");
                 List<string> paquetes = new List<string>();
                 foreach (string? idsPaquete in verificacionResult.PackageIds)
                 {
                     _logger.LogInformation("Creando solicitud de descarga.");
                     var descargaRequest = DescargaRequest.CreateInstace(idsPaquete, SolicitudEntity.rfcSolicitante, accessToken);
                
                     _logger.LogInformation("Enviando solicitud de descarga.");
                     DescargaResult? descargaResult = await _descargaService.SendSoapRequestAsync(descargaRequest,
                         certificadoSat,
                         cancellationToken);

                     string fileName = Path.Combine(rutaDescarga, $"{idsPaquete}.zip");
                     byte[] paqueteContenido = Convert.FromBase64String(descargaResult.Package);

                     _logger.LogInformation("Guardando paquete descargado en un archivo .zip en la ruta de descarga.");
                     using FileStream fileStream = File.Create(fileName, paqueteContenido.Length);
                     await fileStream.WriteAsync(paqueteContenido, 0, paqueteContenido.Length, cancellationToken);
                     
                    /*Respaldo respaldo =new Respaldo();
                    respaldo.IdSolicitudSat = idsPaquete;
                    respaldo.rutadescarga = $"{idsPaquete}.zip";
                    _context.Respaldo.Add(respaldo);
                    _context.SaveChanges();*/

                    paquetes.Add(idsPaquete);


                    var stream = new MemoryStream(paqueteContenido);

                    IFormFile file = new FormFile(stream, 0, paqueteContenido.Length, $"{idsPaquete}.zip", $"{idsPaquete}.zip");

                    _storage.UploadAsync(file);



                 }

               var rutaTarget = @"C:\DesagarCFDIWEB\XMLUNZIP";

               foreach (string paquete in paquetes)
               {
                   var rutaZip = @$"C:\DesagarCFDIWEB\PaquetesZip\{paquete}.zip";

                   ZipFile.ExtractToDirectory(rutaZip, rutaTarget);
               }

  
                _logger.LogInformation("Proceso terminado.");
            }

         

            


        }


          
 
    }

}
