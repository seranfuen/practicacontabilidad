namespace PracticaContabilidad.Model.Repositories
{
    public interface ILedgerEntryRepository
    {
        /// <summary>
        ///     This is a very simple accounting entry where the same quantity that is debited to one
        ///     account is credited to the other.
        ///     As the application evolves, we will go to a more complex system.
        /// </summary>
        /// <param name="debitEntry">the entry that debits an account</param>
        /// <param name="creditEntry">the entry that credits a different account</param>
        void InsertDebitCreditEntries(LedgerEntry debitEntry, LedgerEntry creditEntry);
    }
}