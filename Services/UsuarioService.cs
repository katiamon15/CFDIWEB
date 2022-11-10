using CFDIWEB.Data;
using CFDIWEB.Interfaces;
using CFDIWEB.Models.Entity;
using CFDIWEB.Models.Forms;
using Microsoft.EntityFrameworkCore;

namespace CFDIWEB.Services
{
    public class UsuarioService: IUsuarios
    {

        private MyAppDbContext _dbContext;

        public UsuarioService(MyAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Usuario(UsuariosFrom usuariosFrom)
        {
            string login = _dbContext
                          .Usuarios
                          .Where(u => u.usuario == usuariosFrom.usuario)
                          .Select(u => u.contraseña)
                          .SingleOrDefault();
        }


    }
}
