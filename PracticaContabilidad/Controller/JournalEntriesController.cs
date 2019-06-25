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
        private const int ItemsPerPage = 10;
        private readonly IAccountRepository _accountRepository;
        private readonly IJournalEntryGroupRepository _journalEntryGroupRepository;

        public JournalEntriesController(IAccountRepository accountRepository,
            IJournalEntryGroupRepository journalEntryGroupRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _journalEntryGroupRepository =
                journalEntryGroupRepository ?? throw new ArgumentNullException(nameof(journalEntryGroupRepository));
        }

        public IActionResult Create()
        {
            return View("Edit", new InsertJournalEntryViewModel
            {
                Accounts = GetAllAccounts(),
                EntryDate = DateTime.Now
            });
        }

        public ViewResult Add()
        {
            return View();
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

            _journalEntryGroupRepository.InsertDebitCreditEntries(new LedgerEntry
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

        public IActionResult Index(int page = 1)
        {
            var journalEntries = _journalEntryGroupRepository.JournalEntryGroups.AsNoTracking()
                .Include(grp => grp.LedgerEntries).ThenInclude(entry => entry.Account)
                .OrderByDescending(entry => entry.Date).Skip((page - 1) * ItemsPerPage).Take(ItemsPerPage);
            return View("Index",
                new JournalEntriesViewModel(journalEntries,
                    new Pagination(_journalEntryGroupRepository.JournalEntryGroups.Count(), ItemsPerPage, page)));
        }

        private IEnumerable<Account> GetAllAccounts()
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