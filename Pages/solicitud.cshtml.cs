using CFDIWEB.Data;
using CFDIWEB.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CFDIWEB.Pages
{   public class SolicitudModel : PageModel
    {

        MyAppDbContext _dbContext;

        public SolicitudModel(MyAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<SelectListItem> Complementos { get; set; }

        public void OnGet()
        {
            cargaCatalogoComplemento();
        }


        private void cargaCatalogoComplemento(){
            Complementos = _dbContext.Complemento.Select(a => new SelectListItem
            {
                Value = a.id.ToString(),
                Text = a.tipocomplemento
            })
            .ToList();
        }

       
    }
}
