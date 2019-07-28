using Microsoft.EntityFrameworkCore;

namespace PracticaContabilidad.Model
{
    public class ContabilidadDbContext : DbContext
    {
        public ContabilidadDbContext()
        {

        }
        public ContabilidadDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<JournalEntryGroup> JournalEntryGroups { get; set; }

        public DbSet<JournalEntryGroupPreset> JournalEntryGroupPresets { get; set; }
    }
}