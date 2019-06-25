using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using PracticaContabilidad.Model;
using PracticaContabilidad.Model.Repositories;
using PracticaContabilidad.Model.UnitOfWork;

namespace PracticaContabilidadTests
{
    [TestFixture]
    public class LedgerEntryRepositoryTests
    {
        [SetUp]
        public void SetUp()
        {
            _context = Substitute.For<ContabilidadDbContext>();
            _uofFactory = Substitute.For<ILedgerEntryUoWFactory>();
            _uof = Substitute.For<ILedgerEntryUnitOfWork>();
            _uofFactory.GetUnitOfWork(_context).Returns(_uof);
        }

        private ILedgerEntryUnitOfWork _uof;
        private ILedgerEntryUoWFactory _uofFactory;
        private ContabilidadDbContext _context;


        [Test]
        public void InsertDebitCreditEntries_uses_unit_of_work_with_all_operations()
        {
            var entityUnderTest = new JournalEntryGroupRepository(_context, _uofFactory);
            var debitEntry = new LedgerEntry
            {
                AccountId = 30,
                EntryDate = new DateTime(2019, 6, 7),
                EntryValue = 1000,
                Remarks = "hello there"
            };
            var creditEntry = new LedgerEntry
            {
                AccountId = 31,
                EntryDate = new DateTime(2019, 6, 7),
                EntryValue = -1000,
                Remarks = "hello there"
            };

            entityUnderTest.InsertDebitCreditEntries(debitEntry, creditEntry);
            _uof.Received(1).AddEntry(debitEntry);
            _uof.Received(1).AddEntry(creditEntry);
            _uof.Received(1).CreditAccount(31, 1000);
            _uof.Received(1).DebitAccount(30, 1000);
            _context.Received(1).SaveChanges();
        }

        [Test]
        public void InsertEntries_does_all_operations()
        {
            var entityUnderTest = new JournalEntryGroupRepository(_context, _uofFactory);

            entityUnderTest.InsertEntries(new List<LedgerEntry>
            {
                new LedgerEntry {AccountId = 1, EntryDate = DateTime.Now, EntryValue = 1000},
                new LedgerEntry {AccountId = 2, EntryDate = DateTime.Now, EntryValue = -800},
                new LedgerEntry {AccountId = 3, EntryDate = DateTime.Now, EntryValue = -200}
            });

            _uof.Received(3).AddEntry(Arg.Any<LedgerEntry>());
            _uof.Received(1).DebitAccount(1, 1000);
            _uof.Received(1).CreditAccount(2, 800);
            _uof.Received(1).CreditAccount(3, 200);
            _context.Received(1).SaveChanges();
        }
    }
}