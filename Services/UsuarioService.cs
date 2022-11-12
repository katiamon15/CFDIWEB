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

        public bool Usuario(UsuariosFrom usuariosFrom)
        {
            Usuarios Usuario = _dbContext
                          .Usuarios
                          .Where(u => u.usuario == usuariosFrom.usuario)
                          .Where(u => u.contraseña == usuariosFrom.contraseña)
                          .SingleOrDefault();

            return Usuario != null;
        }


    }
}
