using System;

namespace PracticaContabilidad.Model.Repositories
{
    public class JournalEntryGroupPresetRepository : IJournalEntryGroupPresetRepository
    {
        private readonly ContabilidadDbContext _context;

        public JournalEntryGroupPresetRepository(ContabilidadDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Save(JournalEntryGroupPreset journalEntryPreset)
        {
            _context.JournalEntryGroupPresets.Add(journalEntryPreset);
            _context.SaveChanges();
        }
    }
}