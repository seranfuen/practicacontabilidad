using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
            return View(new InsertJournalEntryViewModel
            {
                Accounts = _accountRepository.Accounts.ToList(),
                EntryDate = DateTime.Now
            });
        }

        [HttpPost]
        public IActionResult Edit(InsertJournalEntryViewModel viewModel)
        {
            ValidateNewEntry(viewModel);

            if (!ModelState.IsValid) return Create();
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
            return View();
        }

        private void ValidateNewEntry(InsertJournalEntryViewModel viewModel)
        {
            if (viewModel.EntryAmount <= 0m)
                ModelState.AddModelError(nameof(viewModel.EntryAmount), "The entry amount cannot be 0 or less than 0");
            if (viewModel.DebitAccountId == viewModel.CreditAccountId)
                ModelState.AddModelError(nameof(viewModel.DebitAccountId),
                    "The debit and the credit accounts cannot be the same");
        }
    }
}