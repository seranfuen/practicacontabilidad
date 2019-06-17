using System;
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
            var entityUnderTest = new LedgerEntryRepository(_context, _uofFactory);
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
    }
}