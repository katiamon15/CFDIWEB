using Microsoft.EntityFrameworkCore;
using CFDIWEB.Models.Entity;


namespace CFDIWEB.Data
{
    public class MyAppDbContext : DbContext
    {
        public DbSet<Complemento> Complemento { get; set; }

        public DbSet<Solicitud> Solicitud { get; set; }  

        public DbSet<Verificacion> Verificacion { get; set; }

        public DbSet<Impuestos> Impuestos { get; set; }
        public DbSet<Productos> Productos { get; set; }

        public DbSet<Provedores> Provedores { get; set; }

        public DbSet<Respaldo> Respaldos { get; set; }

        public DbSet<Usuarios> Usuarios { get; set; }

        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options)
        {

        }
    }
}
