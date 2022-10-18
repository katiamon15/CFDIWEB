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

namespace CFDIWEB.Pages
{   public class SolicitudModel : PageModel
    {

        private MyAppDbContext _dbContext;
        private IDescargaMasiva _descargaservice;
   
        [BindProperty]
        public SolicitudForm SolicitudForm { get; set; }
        public SolicitudModel(MyAppDbContext dbContext, IDescargaMasiva descargaservice)
        {
            _dbContext = dbContext;
            _descargaservice = descargaservice;
        }

        public List<SelectListItem> Complementos { get; set; }

        public async Task OnGet()
        {
     
            cargaCatalogoComplemento();
      
        }


        private void cargaCatalogoComplemento() {
            Complementos = _dbContext.Complemento.Select(a => new SelectListItem
            {
                Value = a.id.ToString(),
                Text = a.tipocomplemento
            })
            .ToList();
        }

        public async Task OnPostSolicitudformulario()
        {
            var context = HttpContext;

            Console.Write("Este es el uuid");
            Console.Write(SolicitudForm.UUID);

            Session session = new Session();
            if (context != null)
            {
                session.PfxUrl = context.Session.GetString("pfx");
                session.PfxPassword = context.Session.GetString("passwordpfx");
                session.TokenSat = context.Session.GetString("tokenSat");
                session.Timestamp = context.Session.GetString("dateRefresh");
            }

            await _descargaservice.SolicitudCFDI(SolicitudForm, session);

        }

    }
}
