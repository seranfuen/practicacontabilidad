using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using PracticaContabilidad.Controller.API;
using PracticaContabilidad.Model;
using PracticaContabilidad.Model.Repositories;

namespace PracticaContabilidadTests
{
    [TestFixture]
    public class JournalEntriesApiControllerTests
    {
        [SetUp]
        public void SetUp()
        {
            _accountRepository = Substitute.For<IAccountRepository>();
            _accountRepository.GetAccountWithCode("420000000").Returns(new Account
            {
                AccountId = 1,
                Code = "420000000"
            });

            _accountRepository.GetAccountWithCode("527000000").Returns(new Account
            {
                AccountId = 2,
                Code = "527000000"
            });

            _journalEntryGroupRepository = Substitute.For<IJournalEntryGroupRepository>();
        }

        private static bool ExpectedDebit(IEnumerable<LedgerEntry> entries)
        {
            var ledgerEntry = entries.Single(x => x.Account.AccountId == 2);

            ledgerEntry.Credit.Should().Be(0);
            ledgerEntry.Debit.Should().Be(1000);
            ledgerEntry.EntryDate.Should().BeSameDateAs(DateTime.Now);
            ledgerEntry.Remarks.Should().Be("GENERAL KENOBI");

            return true;
        }

        private static bool ExpectedCredit(IEnumerable<LedgerEntry> entries)
        {
            var ledgerEntry = entries.Single(x => x.Account.AccountId == 1);

            ledgerEntry.Credit.Should().Be(1000);
            ledgerEntry.Debit.Should().Be(0);
            ledgerEntry.EntryDate.Should().BeSameDateAs(DateTime.Now);
            ledgerEntry.Remarks.Should().Be("HELLO THERE");

            return true;
        }

        private IAccountRepository _accountRepository;
        private IJournalEntryGroupRepository _journalEntryGroupRepository;

        [Test]
        public void Post_calls_ledger_repository_with_entries()
        {
            var entityUnderTest = new JournalEntriesController(_journalEntryGroupRepository, _accountRepository);
            var result = entityUnderTest.Post(new List<JournalEntryViewModel>
            {
                new JournalEntryViewModel {Account = "420000000", Credit = 1000, Remarks = "HELLO THERE"},
                new JournalEntryViewModel {Account = "527000000", Debit = 1000, Remarks = "GENERAL KENOBI"}
            });

            result.Should().BeOfType<OkResult>();

            _journalEntryGroupRepository.Received(1).InsertEntries(Arg.Is<IEnumerable<LedgerEntry>>(x => ExpectedCredit(x)));
            _journalEntryGroupRepository.Received(1).InsertEntries(Arg.Is<IEnumerable<LedgerEntry>>(x => ExpectedDebit(x)));
        }

        [Test]
        public void Post_calls_with_not_existent_account_fails()
        {
            var entityUnderTest = new JournalEntriesController(_journalEntryGroupRepository, _accountRepository);
            var result = entityUnderTest.Post(new List<JournalEntryViewModel>
            {
                new JournalEntryViewModel {Account = "420000000", Credit = 1000, Remarks = "HELLO THERE"},
                new JournalEntryViewModel {Account = "527000001", Debit = 1000, Remarks = "GENERAL KENOBI"}
            });

            result.Should().BeOfType<BadRequestObjectResult>();
            _journalEntryGroupRepository.DidNotReceive().InsertEntries(Arg.Any<IEnumerable<LedgerEntry>>());
        }
    }
}