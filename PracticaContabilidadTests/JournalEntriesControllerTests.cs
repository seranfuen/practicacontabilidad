using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using PracticaContabilidad.Controller;
using PracticaContabilidad.Model;
using PracticaContabilidad.Model.Repositories;

namespace PracticaContabilidadTests
{
    [TestFixture]
    public class JournalEntriesControllerTests
    {
        [SetUp]
        public void SetUp()
        {
            _repository = Substitute.For<IAccountRepository>();
            _repository.Accounts.Returns(new List<Account>
            {
                new Account(),
                new Account(),
                new Account(),
                new Account(),
                new Account()
            }.AsQueryable());

            _journalRepository = Substitute.For<ILedgerEntryRepository>();
        }

        private IAccountRepository _repository;
        private ILedgerEntryRepository _journalRepository;

        private static bool IsExpectedCredit(LedgerEntry ledgerEntry)
        {
            ledgerEntry.AccountId.Should().Be(3);
            ledgerEntry.EntryDate.Should().BeSameDateAs(DateTime.Now);
            ledgerEntry.Remarks.Should().Be("Hello there");
            ledgerEntry.EntryValue.Should().Be(-2000);
            return true; // will not return true if the previous assertions do not pass, as an exception will be thrown
        }

        private static bool IsExpectedDebit(LedgerEntry ledgerEntry)
        {
            ledgerEntry.AccountId.Should().Be(2);
            ledgerEntry.EntryDate.Should().BeSameDateAs(DateTime.Now);
            ledgerEntry.Remarks.Should().Be("Hello there");
            ledgerEntry.EntryValue.Should().Be(2000);
            return true;
        }

        private JournalEntriesController InitializeEntityUnderTest()
        {
            return new JournalEntriesController(_repository, _journalRepository);
        }

        private static InsertJournalEntryViewModel GetTestViewModel()
        {
            return new InsertJournalEntryViewModel
            {
                CreditAccountId = 3,
                DebitAccountId = 2,
                EntryAmount = 2000,
                Remarks = "Hello there"
            };
        }

        [Test]
        public void Edit_new_entry_initialized_with_current_date_and_existing_accounts()
        {
            var entityUnderTest = InitializeEntityUnderTest();
            var result = (ViewResult) entityUnderTest.Create();
            var model = (InsertJournalEntryViewModel) result.Model;
            model.Accounts.Should().HaveCount(5);
            model.EntryDate.Should().BeSameDateAs(DateTime.Now);
        }

        [Test]
        public void Edit_with_amount_0_or_negative_results_in_error()
        {
            var entityUnderTest = InitializeEntityUnderTest();
            var insertJournalEntryViewModel = GetTestViewModel();
            insertJournalEntryViewModel.EntryAmount = 0;
            entityUnderTest.Edit(insertJournalEntryViewModel);
            entityUnderTest.ModelState.IsValid.Should().BeFalse();
            _journalRepository.DidNotReceive().InsertDebitCreditEntries(Arg.Any<LedgerEntry>(), Arg.Any<LedgerEntry>());
        }

        [Test]
        public void Edit_with_debit_and_credit_account_being_the_same_results_in_error()
        {
            var entityUnderTest = InitializeEntityUnderTest();
            var insertJournalEntryViewModel = GetTestViewModel();
            insertJournalEntryViewModel.DebitAccountId = insertJournalEntryViewModel.CreditAccountId = 2;
            entityUnderTest.Edit(insertJournalEntryViewModel);
            entityUnderTest.ModelState.IsValid.Should().BeFalse();
            _journalRepository.DidNotReceive().InsertDebitCreditEntries(Arg.Any<LedgerEntry>(), Arg.Any<LedgerEntry>());
        }

        [Test]
        public void Edit_with_errors_does_not_call_repository()
        {
            var entityUnderTest = InitializeEntityUnderTest();
            entityUnderTest.ModelState.AddModelError("", "Error");
            _journalRepository.DidNotReceive().InsertDebitCreditEntries(Arg.Any<LedgerEntry>(), Arg.Any<LedgerEntry>());
        }

        [Test]
        public void Edit_with_no_errors_calls_repository_to_save_entries()
        {
            var entityUnderTest = InitializeEntityUnderTest();
            entityUnderTest.Edit(GetTestViewModel());
            _journalRepository.Received(1).InsertDebitCreditEntries(Arg.Is<LedgerEntry>(e1 => IsExpectedDebit(e1)),
                Arg.Is<LedgerEntry>(e2 => IsExpectedCredit(e2)));
        }
    }
}