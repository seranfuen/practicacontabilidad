using System.Collections.Generic;
using System.Linq;

namespace PracticaContabilidad.Model.Repositories
{
    public interface IJournalEntryGroupRepository
    {
        IQueryable<JournalEntryGroup> JournalEntryGroups { get; }

        /// <summary>
        ///     This is a very simple accounting entry where the same quantity that is debited to one
        ///     account is credited to the other.
        ///     As the application evolves, we will go to a more complex system.
        /// </summary>
        /// <param name="debitEntry">the entry that debits an account</param>
        /// <param name="creditEntry">the entry that credits a different account</param>
        void InsertDebitCreditEntries(LedgerEntry debitEntry, LedgerEntry creditEntry);

        /// <summary>
        ///     Inserts a ledger entry (apunte contable) that credits or debits one account
        /// </summary>
        /// <param name="entry"></param>
        void InsertEntries(IEnumerable<LedgerEntry> entry);

        IEnumerable<LedgerEntry> GetLastEntriesForAccount(int accountId, int count);
    }
}