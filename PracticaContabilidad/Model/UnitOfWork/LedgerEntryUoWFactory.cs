namespace PracticaContabilidad.Model.UnitOfWork
{
    public class LedgerEntryUoWFactory : ILedgerEntryUoWFactory
    {
        public ILedgerEntryUnitOfWork GetUnitOfWork(ContabilidadDbContext context)
        {
            return new LedgerEntryUnitOfWork(context);
        }
    }
}