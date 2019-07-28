using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PracticaContabilidad.Model
{
    public class JournalEntryGroupPreset
    {
        public int Id { get; set; }
        public ICollection<JournalEntryPresetEntry> PresetEntries { get; set; }

        public bool SoftDeleted { get; set; }

        [Required]
        public string Name { get; set; }
    }
}