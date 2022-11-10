using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CFDIWEB.Models.Forms;
using Microsoft.Extensions.Primitives;
using System.Text;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using System.Data;
using CFDIWEB.Models;
using CFDIWEB.Models.Entity;
using Microsoft.EntityFrameworkCore;
using CFDIWEB.Data;
using CFDIWEB.Interfaces;
using CFDIWEB.Services;
using System.Transactions;

namespace CFDIWEB.Pages
{
    public class usuariosModel : PageModel
    {
        [BindProperty]
        public String Usuario { get; set; }

        [BindProperty]
        public String Contrasena { get; set; }


        public void OnGet()
        {

        }
        private MyAppDbContext _dbContext;

   
        public usuariosModel(MyAppDbContext dbContext)
        {
            _dbContext = dbContext;
          
        }


        public async Task OnPostLogin()
        {
            Console.Write("Aqui estoy");
            Console.Write(Usuario);
            Console.WriteLine(Contrasena);

            

        }





    }
}  
