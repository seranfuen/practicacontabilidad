using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PracticaContabilidad.Model;
using PracticaContabilidad.Model.Repositories;

namespace PracticaContabilidad.Controller.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalEntriesController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILedgerEntryRepository _ledgerEntryRepository;

        public JournalEntriesController(ILedgerEntryRepository ledgerEntryRepository,
            IAccountRepository accountRepository)
        {
            _ledgerEntryRepository =
                ledgerEntryRepository ?? throw new ArgumentNullException(nameof(ledgerEntryRepository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        [HttpPost]
        public IActionResult Post(IEnumerable<JournalEntryViewModel> entries)
        {
            var ledgerEntries = entries.Select(ToLedgerEntry).ToList();

            if (ledgerEntries.Any(x => x.Account == null))
                return BadRequest("One or more accounts do not exist");

            _ledgerEntryRepository.InsertEntries(ledgerEntries);

            return Ok();
        }

        private LedgerEntry ToLedgerEntry(JournalEntryViewModel entry)
        {
            return new LedgerEntry
            {
                Account = _accountRepository.GetAccountWithCode(entry.Account),
                EntryValue = entry.Credit > 0 ? -entry.Credit : entry.Debit,
                EntryDate = DateTime.Now,
                Remarks = entry.Remarks
            };
        }
    }
}