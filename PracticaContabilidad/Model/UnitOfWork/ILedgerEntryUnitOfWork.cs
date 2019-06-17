namespace PracticaContabilidad.Model.UnitOfWork
{
    public interface ILedgerEntryUnitOfWork
    {
        void AddEntry(LedgerEntry entry);
        void CreditAccount(int accountId, decimal amount);
        void DebitAccount(int accountId, decimal amount);
    }
}