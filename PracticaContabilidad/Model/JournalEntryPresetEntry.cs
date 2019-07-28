using System.ComponentModel.DataAnnotations;

namespace PracticaContabilidad.Model
{
    public class JournalEntryPresetEntry
    {
        public int Id { get; set; }

        [Required] public DebitCredit DebitCredit { get; set; }

        [Required] public Account Account { get; set; }

        [Required] [Range(1, int.MaxValue)] public int Order { get; set; }
    }
}