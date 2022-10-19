using CFDIWEB.Data;
using CFDIWEB.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using CFDIWEB.Models;
using CFDIWEB.Models.Forms;
using System;
using Microsoft.Build.Framework;
using CFDIWEB.Interfaces;
using CFDIWEB.Enumerations;
using Microsoft.Data.SqlClient.Server;

namespace CFDIWEB.Pages
{
    public class verificacionModel : PageModel
    {
        private MyAppDbContext _dbContext;

        private IDescargaMasiva _descargaservice;
        

         public List<Solicitud> VerificacionList { get; set; }
         public List<Verificacion> verificacions { get; set; }
        public verificacionModel(MyAppDbContext dbContext, IDescargaMasiva descargaservice)
        {
            _dbContext = dbContext;
            _descargaservice = descargaservice;
        }

        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPostVerifica([FromBody]string dato)
        {
            var bodyResponse = new
            {
                dato = dato
            };
            return new JsonResult(bodyResponse);
        }

        public async Task<IActionResult> OnPostDatos() {
            try
            {
                VerificacionList = _dbContext.Solicitud.ToList();
            }catch(Exception e)
            {
                Console.Write("Este es el error de verdad");
                Console.Write(e.StackTrace);
                Console.Write(e.Message);
            }

            return new JsonResult(obtieneDatos(VerificacionList));
        }

        private List<SolicitudTable> obtieneDatos(List<Solicitud> VerificacionList) {

            List<SolicitudTable> solicitudList = new List<SolicitudTable>();
            foreach (Solicitud solicitud in VerificacionList) {
                SolicitudTable solicitudTable = new SolicitudTable();
                solicitudTable.UUID = solicitud.UUID;
                solicitudTable.FechaInicial = solicitud.FechaInicial == null ? "n/a" : ((DateTime)solicitud.FechaInicial).ToString("dd/MM/yyyy"); 
                solicitudTable.FechaFin = solicitud.FechaFin == null ? "n/a" : ((DateTime)solicitud.FechaFin).ToString("dd/MM/yyyy");
                solicitudTable.rfcEmisor = solicitud.rfcEmisor;
                solicitudTable.rfcReceptores = solicitud.rfcReceptores;
                solicitudTable.rfcSolicitante = solicitud.rfcSolicitante;
                solicitudTable.CodEstatus = solicitud.CodEstatus;
                solicitudTable.TipoSolicitud = TipoSolicitud.FromValue(solicitud.TipoSolicitud.Value).Name;
                solicitudTable.TipoComprobante = !String.IsNullOrEmpty(solicitud.TipoComprobante) ? 
                    TipoComprobante.FromValue(solicitud.TipoComprobante).Name:"No definido";
                solicitudTable.EstadoComprobante = solicitud.EstadoComprobante != null?
                    EstadoComprobante.FromValue(solicitud.EstadoComprobante.Value).Name: "No definido";

                string Name = _dbContext
                          .Complemento
                          .Where(u => u.id == solicitud.Complemento)
                          .Select(u => u.tipocomplemento)
                          .SingleOrDefault();

                solicitudTable.Complemento = !String.IsNullOrEmpty(Name) ? Name : "No especificado";
                solicitudTable.Mensaje = solicitud.Mensaje;
                solicitudTable.IdSolicitudSat = solicitud.IdSolicitudSat;
                solicitudTable.EstadoSolicitud = solicitud.EstadoSolicitud;
                
                solicitudList.Add(solicitudTable);
            }
            return solicitudList;
        }

      

    }
}
