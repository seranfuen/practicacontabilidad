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
        private readonly IAccountRepository _repository;

        public AccountController(IAccountRepository repository)
        {
            _repository = repository;
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

        [HttpPost]
        public ViewResult Edit(Account account)
        {
            if (account.AccountId == 0)
            {
                SetUserInputCodeToLongFormat(account);
            }

            if (!ModelState.IsValid)
                return View("Edit", account);

            _repository.Save(account);

            return View("Index", _repository.Accounts.AsNoTracking().OrderBy(acc => acc.Code));
        }

        private void SetUserInputCodeToLongFormat(Account account)
        {
            var shortToLongCode = new ShortToLongAccountCode(account.Code);
            if (!shortToLongCode.IsCorrectCode)
            {
                ModelState.AddModelError(nameof(Account.Code),
                    "The code is in a wrong format. It must be supplied as up to 9 digits, with an optional dot (.) to separate first part from last part of the code.");
            }
            else
            {
                account.Code = shortToLongCode.LongCode;
            }
        }


        public IActionResult Index()
        {
            return View(_repository.Accounts.OrderBy(acc => acc.Code));
        }
    }
}