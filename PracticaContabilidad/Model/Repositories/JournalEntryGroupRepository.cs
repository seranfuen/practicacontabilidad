using System;
using System.Collections.Generic;
using System.Linq;
using PracticaContabilidad.Model.UnitOfWork;

namespace PracticaContabilidad.Model.Repositories
{
    public class JournalEntryGroupRepository : IJournalEntryGroupRepository
    {
        private readonly ContabilidadDbContext _context;
        private readonly ILedgerEntryUoWFactory _uofFactory;

        public JournalEntryGroupRepository(ContabilidadDbContext context, ILedgerEntryUoWFactory uofFactory)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _uofFactory = uofFactory ?? throw new ArgumentNullException(nameof(uofFactory));
        }

        public IQueryable<JournalEntryGroup> JournalEntryGroups => _context.JournalEntryGroups;

        public void InsertDebitCreditEntries(LedgerEntry debitEntry, LedgerEntry creditEntry)
        {
            if (debitEntry == null) throw new ArgumentNullException(nameof(debitEntry));
            if (creditEntry == null) throw new ArgumentNullException(nameof(creditEntry));
            var uof = _uofFactory.GetUnitOfWork(_context);
            uof.AddEntry(debitEntry);
            uof.AddEntry(creditEntry);
            uof.DebitAccount(debitEntry.AccountId, debitEntry.EntryValue);
            uof.CreditAccount(creditEntry.AccountId, -creditEntry.EntryValue);
            _context.SaveChanges();
        }

        public void InsertEntries(IEnumerable<LedgerEntry> entries)
        {
            var uof = _uofFactory.GetUnitOfWork(_context);
            foreach (var entry in entries)
            {
                uof.AddEntry(entry);
                if (entry.Credit > 0)
                    uof.CreditAccount(entry.AccountId, entry.Credit);
                else
                    uof.DebitAccount(entry.AccountId, entry.Debit);
            }

            _context.SaveChanges();
        }

        public IEnumerable<LedgerEntry> GetLastEntriesForAccount(int accountId, int count)
        {
            return _context.JournalEntryGroups.SelectMany(x => x.LedgerEntries).Where(x => x.AccountId == accountId)
                .OrderByDescending(x => x.EntryDate).Take(count).ToList();
        }
    }
}