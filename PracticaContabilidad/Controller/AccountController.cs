using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticaContabilidad.Helpers;
using PracticaContabilidad.Model;
using PracticaContabilidad.Model.Repositories;

namespace PracticaContabilidad.Controller
{
    public class AccountController : Microsoft.AspNetCore.Mvc.Controller
    {
        private const int EntriesToShow = 30;
        private readonly IJournalEntryGroupRepository _journalRepository;
        private readonly IAccountRepository _repository;

        public AccountController(IAccountRepository repository, IJournalEntryGroupRepository journalRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _journalRepository = journalRepository ?? throw new ArgumentNullException(nameof(journalRepository));
        }

        public ViewResult Create()
        {
            ViewBag.Title = "Crear cuenta";
            return View("Edit", new Account());
        }

        public ViewResult Edit(int accountId)
        {
            ViewBag.Title = "Modificar cuenta";
            return View("Edit", _repository.Accounts.Single(account => account.AccountId == accountId));
        }

        public ViewResult Show(int accountId)
        {
            var account = _repository.GetAccount(accountId);
            var entries = _journalRepository.GetLastEntriesForAccount(accountId, 30);
            ViewBag.Title = $"Cuenta {account.Name ?? account.Code}";

            return View(new AccountViewModel
            {
                AccountId = accountId,
                Code = account.Code,
                Name = account.Name,
                Description = account.Description,
                Balance = account.Balance,
                Entries = entries
            });
        }

        [HttpPost]
        public ViewResult Edit(Account account)
        {
            if (account.AccountId == 0) SetUserInputCodeToLongFormat(account);

            if (!ModelState.IsValid)
                return View("Edit", account);

            _repository.Save(account);

            return View("Index", _repository.Accounts.AsNoTracking().OrderBy(acc => acc.Code));
        }


        public IActionResult Index()
        {
            return View(_repository.Accounts.OrderBy(acc => acc.Code));
        }

        private void SetUserInputCodeToLongFormat(Account account)
        {
            var shortToLongCode = new ShortToLongAccountCode(account.Code);
            if (!shortToLongCode.IsCorrectCode)
                ModelState.AddModelError(nameof(Account.Code),
                    "The code is in a wrong format. It must be supplied as up to 9 digits, with an optional dot (.) to separate first part from last part of the code.");
            else
                account.Code = shortToLongCode.LongCode;
        }
    }
}