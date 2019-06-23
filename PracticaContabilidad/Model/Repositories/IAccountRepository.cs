using System.Linq;

namespace PracticaContabilidad.Model.Repositories
{
    public interface IAccountRepository
    {
        IQueryable<Account> Accounts { get; }
        void Save(Account account);
        Account GetAccountWithCode(string code);
    }
}