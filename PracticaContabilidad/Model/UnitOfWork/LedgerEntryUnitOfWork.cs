using System;
using System.Linq;

namespace PracticaContabilidad.Model.UnitOfWork
{
    public class LedgerEntryUnitOfWork : ILedgerEntryUnitOfWork
    {
        private readonly ContabilidadDbContext _context;

        public LedgerEntryUnitOfWork(ContabilidadDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddEntry(LedgerEntry entry)
        {
            _context.LedgerEntries.Add(entry);
        }

        public void CreditAccount(int accountId, decimal amount)
        {
            var account = GetAccount(accountId);
            account.Credit += amount;
        }

        public void DebitAccount(int accountId, decimal amount)
        {
            var account = GetAccount(accountId);
            account.Debit += amount;
        }

        private Account GetAccount(int accountId)
        {
            return _context.Accounts.Single(acc => acc.AccountId == accountId);
        }
    }
}