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

        public void OnGet()
        {
             
        }
        private MyAppDbContext _dbContext;
        private IUsuarios _usuarios;

        public usuariosModel(MyAppDbContext dbContext, IUsuarios usuarioscs)
        {
            _dbContext = dbContext;
            _usuarios = usuarioscs;
          
        }


        public async Task<IActionResult> OnPostLogin([FromBody]UsuariosFrom UsuariosFrom)
        {
            string responseMessage = "";

            if (!_usuarios.Usuario(UsuariosFrom))
            {
                responseMessage = "Usuario Invalido,Verifica tus datos ";
            }

            var bodyResponse = new
            {
                responseMessage = responseMessage
            };
            return new JsonResult(bodyResponse);
        }

    }
}  
