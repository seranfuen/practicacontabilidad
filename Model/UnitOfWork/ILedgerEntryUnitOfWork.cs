namespace PracticaContabilidad.Model.UnitOfWork
{
    public interface ILedgerEntryUnitOfWork
    {
        void AddEntry(LedgerEntry entry);
        void DebitAccount(int accountId);
        void CreditAccount(int accountId);
    }
}