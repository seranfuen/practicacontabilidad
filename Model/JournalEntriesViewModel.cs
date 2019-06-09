using System.Collections.Generic;

namespace PracticaContabilidad.Model
{
    public class JournalEntriesViewModel
    {
        public JournalEntriesViewModel(IEnumerable<LedgerEntry> journalEntries, Pagination pagination)
        {
            JournalEntries = journalEntries;
            Pagination = pagination;
        }

        public IEnumerable<LedgerEntry> JournalEntries { get; }
        public Pagination Pagination { get; }
    }
}