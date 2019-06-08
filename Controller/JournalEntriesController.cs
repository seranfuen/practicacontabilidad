using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticaContabilidad.Model;
using PracticaContabilidad.Model.Repositories;

namespace PracticaContabilidad.Controller
{
    public class JournalEntriesController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILedgerEntryRepository _ledgerEntryRepository;

        public JournalEntriesController(IAccountRepository accountRepository,
            ILedgerEntryRepository ledgerEntryRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _ledgerEntryRepository =
                ledgerEntryRepository ?? throw new ArgumentNullException(nameof(ledgerEntryRepository));
        }

        public IActionResult Create()
        {
            return View("Edit", new InsertJournalEntryViewModel
            {
                Accounts = GetAllAccounts(),
                EntryDate = DateTime.Now
            });
        }


        [HttpPost]
        public IActionResult Edit(InsertJournalEntryViewModel viewModel)
        {
            ValidateNewEntry(viewModel);

            if (!ModelState.IsValid)
            {
                viewModel.Accounts = GetAllAccounts();
                return View("Edit", viewModel);
            }

            _ledgerEntryRepository.InsertDebitCreditEntries(new LedgerEntry
            {
                AccountId = viewModel.DebitAccountId,
                EntryDate = DateTime.Now,
                Remarks = viewModel.Remarks,
                EntryValue = viewModel.EntryAmount
            }, new LedgerEntry
            {
                AccountId = viewModel.CreditAccountId,
                EntryDate = DateTime.Now,
                Remarks = viewModel.Remarks,
                EntryValue = -viewModel.EntryAmount
            });

            return Index();
        }

        public IActionResult Index()
        {
            return View(_ledgerEntryRepository.LedgerEntries.AsNoTracking().Include(entry => entry.Account)
                .OrderByDescending(entry => entry.EntryDate));
        }

        private List<Account> GetAllAccounts()
        {
            return _accountRepository.Accounts.ToList();
        }

        private void ValidateNewEntry(InsertJournalEntryViewModel viewModel)
        {
            if (viewModel.EntryAmount <= 0m)
                ModelState.AddModelError(nameof(viewModel.EntryAmount), "La cantidad debe ser mayor que 0");
            if (viewModel.DebitAccountId == viewModel.CreditAccountId)
                ModelState.AddModelError(nameof(viewModel.DebitAccountId),
                    "No se puede cargar y abonar la misma cuenta.");
        }
    }
}