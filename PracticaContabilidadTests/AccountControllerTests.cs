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
    public class AccountControllerTests
    {
        private IAccountRepository _accountRepository;

        [TestCase("420", "420000000")]
        [TestCase("420.1", "420000001")]
        [TestCase("42032.20", "420320020")]
        [TestCase("123456789", "123456789")]
        public void Edit_calls_repository_to_save_account_setting_correct_code(string codeFromUser,
            string expectedCodeToPersist)
        {
            var entityUnderTest = new AccountController(_accountRepository);
            entityUnderTest.Edit(new Account {Code = codeFromUser, Name = "Test"});
            _accountRepository.Received(1).Save(Arg.Is<Account>(acc => acc.Code == expectedCodeToPersist));
        }

        [Test]
        public void Edit_with_wrong_code_sets_model_error_does_not_persist()
        {
            var entityUnderTest = new AccountController(_accountRepository);
            entityUnderTest.Edit(new Account {Code = "ABC", Name = "Test"});

            entityUnderTest.ModelState.IsValid.Should().BeFalse();
            _accountRepository.DidNotReceive().Save(Arg.Any<Account>());
        }

        [Test]
        public void Edit_with_existing_account_does_not_replace_code()
        {
            var entityUnderTest = new AccountController(_accountRepository);
            entityUnderTest.Edit(new Account { AccountId = 80, Code = "420", Name = "Test" });
            _accountRepository.Received(1).Save(Arg.Is<Account>(acc => acc.Code == "420"));
        }

        [Test]
        public void Index_returns_all_accounts_sorted_by_code()
        {
            var entityUnderTest = new AccountController(_accountRepository);
            var result = (ViewResult) entityUnderTest.Index();
            var accounts = ((IEnumerable<Account>) result.Model).ToList();

            accounts.Should().HaveCount(3);
            accounts[0].Code.Should().Be("100");
            accounts[1].Code.Should().Be("101.1");
            accounts[2].Code.Should().Be("300");
        }

        [SetUp]
        public void Setup()
        {
            _accountRepository = Substitute.For<IAccountRepository>();
            _accountRepository.Accounts.Returns(new List<Account>
            {
                new Account {Code = "101.1"},
                new Account {Code = "300"},
                new Account {Code = "100"}
            }.AsQueryable());
        }
    }
}