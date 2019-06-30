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
        private readonly IJournalEntryGroupRepository _journalEntryGroupRepository;

        public JournalEntriesController(IJournalEntryGroupRepository journalEntryGroupRepository,
            IAccountRepository accountRepository)
        {
            _journalEntryGroupRepository =
                journalEntryGroupRepository ?? throw new ArgumentNullException(nameof(journalEntryGroupRepository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        [HttpPost]
        public IActionResult Post(IEnumerable<JournalEntryViewModel> entries)
        {
            var ledgerEntries = entries.Select(ToLedgerEntry).ToList();

            if (ledgerEntries.Any(x => x.Account == null))
                return BadRequest("One or more accounts do not exist");

            _journalEntryGroupRepository.InsertEntries(ledgerEntries);

            return Ok();
        }

        private LedgerEntry ToLedgerEntry(JournalEntryViewModel entry)
        {
            var accountWithCode = _accountRepository.GetAccountWithCode(entry.Account);
            return new LedgerEntry
            {
                AccountId = accountWithCode.AccountId,
                Account = accountWithCode,
                EntryValue = entry.Credit > 0 ? -entry.Credit : entry.Debit,
                EntryDate = DateTime.Now,
                Remarks = entry.Remarks
            };
        }
    }
}