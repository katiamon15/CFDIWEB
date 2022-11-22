using CFDIWEB.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CFDIWEB.Models;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text;
using System.Threading;
using CFDIWEB.Services;

namespace CFDIWEB.Pages
{
    public class pdfModel : PageModel
    {
        IPdf _pdfService;

        public pdfModel(IPdf pdfService)
        {
            _pdfService = pdfService;
        }

        [BindProperty]
        public PdfModels ArchivoPdf { get; set; }

        
        public void OnGet()
        {

        }

        public async Task OnPostLeer()
        {
            Console.Write("Este es tu request");
            Console.Write(ArchivoPdf.ArchivoPdf);
            List<byte[]> files = new List<byte[]>();

            foreach (var file in ArchivoPdf.ArchivoPdf)
            {
                var memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                byte[] byteArray = memoryStream.ToArray();
                files.Add(byteArray);
                memoryStream.Dispose();
            }

            //var pdf = Convert.ToBase64String(files[0]);

            _pdfService.Pdfversion(files);

        }

         


    }
}
