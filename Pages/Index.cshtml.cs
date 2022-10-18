using CFDIWEB.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CFDIWEB.Models;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text;
using System.Threading;

namespace CFDIWEB.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private IDescargaMasiva _descargaservice;
    [BindProperty]

    public SubirArchivoModel SubirArchivo { get; set; }

    public IndexModel(ILogger<IndexModel> logger, IDescargaMasiva  descargaservice)
    {
        _logger = logger;
        _descargaservice = descargaservice;
    }

    public void OnGet()
    {
     
    }

    public async Task OnPostGuardallave(){
        Console.Write("Este es tu request");
        Console.Write(SubirArchivo.Contrasena);
        Console.Write(SubirArchivo.Archivo);

        var memoryStream = new MemoryStream();
        SubirArchivo.Archivo.CopyTo(memoryStream);
        byte[] byteArray = memoryStream.ToArray();

        Session Session = new Session();
        Session.PfxUrl = Convert.ToBase64String(byteArray);
        Session.PfxPassword = SubirArchivo.Contrasena;
         
         await _descargaservice.DescargaCFDI(Session);
        
   
    }

}
