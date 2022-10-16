using CFDIWEB.Data;
using CFDIWEB.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using CFDIWEB.Models;
using CFDIWEB.Models.Forms;

namespace CFDIWEB.Pages
{   public class SolicitudModel : PageModel
    {

        MyAppDbContext _dbContext;


        [BindProperty]
        public SolicitudForm SolicitudForm { get; set; }
        public SolicitudModel(MyAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<SelectListItem> Complementos { get; set; }

        public void OnGet()
        {
            cargaCatalogoComplemento();

            var task = Task.Run(() => {
                try
                {
                    Console.WriteLine("Este es el PFX");
                    Console.WriteLine(HttpContext.Session.GetString("pfx"));
                    Console.WriteLine("Este es el PASS");
                    Console.WriteLine(HttpContext.Session.GetString("passwordpfx"));
                    Console.WriteLine("Este es el token del sat");
                    Console.Write(HttpContext.Session.GetString("tokenSat"));
                }catch(Exception e){
                    Console.WriteLine("Los datos del usuario no se encontraron");
                    Console.Write(e);
                }
            });
         
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
