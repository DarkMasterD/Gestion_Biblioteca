using Microsoft.EntityFrameworkCore;

namespace Gestion_Biblioteca.Models
{
    public class bibliotecaContext : DbContext
    {
        public bibliotecaContext(DbContextOptions<bibliotecaContext> options) : base(options) 
        { 
        
        }

        public DbSet<Autor> Autor {  get; set; }
        public DbSet<Libro> Libro { get; set; }
    }
}
