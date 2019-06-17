using System;
using System.Linq;
using PracticaContabilidad.Model.UnitOfWork;

namespace PracticaContabilidad.Model.Repositories
{
    public class LedgerEntryRepository : ILedgerEntryRepository
    {
        private readonly ContabilidadDbContext _context;
        private readonly ILedgerEntryUoWFactory _uofFactory;

        public LedgerEntryRepository(ContabilidadDbContext context, ILedgerEntryUoWFactory uofFactory)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _uofFactory = uofFactory ?? throw new ArgumentNullException(nameof(uofFactory));
        }

        public IQueryable<LedgerEntry> LedgerEntries => _context.LedgerEntries;

        public void InsertDebitCreditEntries(LedgerEntry debitEntry, LedgerEntry creditEntry)
        {
            var uof = _uofFactory.GetUnitOfWork(_context);
            uof.AddEntry(debitEntry);
            uof.AddEntry(creditEntry);
            uof.DebitAccount(debitEntry.AccountId, debitEntry.EntryValue);
            uof.CreditAccount(creditEntry.AccountId, -creditEntry.EntryValue);
            _context.SaveChanges();
        }
    }
}