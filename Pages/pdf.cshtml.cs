using CFDIWEB.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CFDIWEB.Models;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text;
using System.Threading;
using CFDIWEB.Services;
using System.Net;
using Microsoft.CodeAnalysis.RulesetToEditorconfig;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting.Server;
using CFDIWEB.Models.Forms;

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

        



        public async Task<FileResult> OnPostLeer()
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

            List<byte[]> pdf  = await _pdfService.Pdfversion(files);

            SolicitudForm osolitud = new SolicitudForm();
            

           

            //Send the File to Download.
            return File(pdf[0], "application/octet-stream","Formato.pdf");

        }






    }
}
