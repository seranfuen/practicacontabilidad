using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PracticaContabilidad.Model
{
    public class JournalEntryGroup
    {
        public int JournalEntryGroupId { get; set; }
        public DateTime Date { get; set; }

        public ICollection<LedgerEntry> LedgerEntries { get; set; }
    }
}