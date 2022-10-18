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
{
    public class verificacionModel : PageModel
    {
        private MyAppDbContext _dbContext;

        private IDescargaMasiva _descargaservice;

        [BindProperty]
        public VerificacionForm  verificacionForm { get; set; }

         public List<Verificacion> VerificacionList { get; set; }

        public verificacionModel(MyAppDbContext dbContext, IDescargaMasiva descargaservice)
        {
            _dbContext = dbContext;
            _descargaservice = descargaservice;
        }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostDatos() {
            VerificacionList = _dbContext.Verificacion.ToList();
            return new JsonResult(VerificacionList);
        }

    }
}
