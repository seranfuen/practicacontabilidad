using Microsoft.EntityFrameworkCore;

namespace PracticaContabilidad.Model
{
    public class ContabilidadDbContext : DbContext
    {
        public ContabilidadDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
    }
}