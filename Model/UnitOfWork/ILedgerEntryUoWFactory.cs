namespace PracticaContabilidad.Model.UnitOfWork
{
    public interface ILedgerEntryUoWFactory
    {
        ILedgerEntryUnitOfWork GetUnitOfWork(ContabilidadDbContext context);
    }
}