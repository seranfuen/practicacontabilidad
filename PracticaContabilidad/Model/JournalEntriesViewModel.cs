using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PracticaContabilidad.Model
{
    public class JournalEntriesViewModel
    {
        public JournalEntriesViewModel(IEnumerable<JournalEntryGroup> ledgerEntryGroups, Pagination pagination)
        {
            LedgerEntryGroups = ledgerEntryGroups;
            Pagination = pagination;
        }

        public IEnumerable<JournalEntryGroup> LedgerEntryGroups { get; }
        public Pagination Pagination { get; }

        [NotMapped] public decimal SumDebit => LedgerEntryGroups.SelectMany(x => x.LedgerEntries).Sum(x => x.Debit);
        [NotMapped] public decimal SumCredit => LedgerEntryGroups.SelectMany(x => x.LedgerEntries).Sum(x => x.Credit);
    }
}