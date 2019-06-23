using System;
using System.Linq;
using AutoMapper;

namespace PracticaContabilidad.Model.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ContabilidadDbContext _context;
        private readonly IMapper _mapper;

        public AccountRepository(ContabilidadDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper;
        }

        public IQueryable<Account> Accounts => _context.Accounts;

        public void Save(Account account)
        {
            if (account.AccountId == 0)
            {
                _context.Accounts.Add(account);
            }
            else
            {
                var existingAccount = _context.Accounts.Single(acc => acc.AccountId == account.AccountId);
                _mapper.Map(account, existingAccount);
            }

            _context.SaveChanges();
        }

        public Account GetAccountWithCode(string code)
        {
            return Accounts.SingleOrDefault(x => x.Code.Equals(code));
        }
    }
}