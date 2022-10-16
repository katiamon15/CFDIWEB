using Microsoft.EntityFrameworkCore;
using CFDIWEB.Models.Entity;


namespace CFDIWEB.Data
{
    public class MyAppDbContext : DbContext
    {
        public DbSet<Complemento> Complemento { get; set; }

        public DbSet<Solicitud> Solicitud { get; set; }  

        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options)
        {

        }
    }
}
