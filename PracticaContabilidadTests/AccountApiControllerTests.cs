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
    public class AccountApiControllerTests
    {
        [SetUp]
        public void SetUp()
        {
            _accountRepository = Substitute.For<IAccountRepository>();
            _accountRepository.GetAccountWithCode("420000000").Returns(new Account
            {
                AccountId = 10,
                Code = "420000000",
                Name = "Test"
            });
        }

        private IAccountRepository _accountRepository;

        [Test]
        public void Post_account_does_not_exist_creates_it_and_returns_it()
        {
            var entityUnderTest = new AccountController(_accountRepository);
            var account = new AccountDto {Code = "480000000", Name = "Hello there", Description = "General Kenobi"};
            var result = entityUnderTest.Post(account);
            result.Value.Name.Should().Be("Hello there");
            result.Value.Code.Should().Be("480000000");
            result.Value.Description.Should().Be("General Kenobi");
            _accountRepository.Received(1).Save(result.Value);
        }

        [Test]
        public void Post_account_exists_returns_it()
        {
            var entityUnderTest = new AccountController(_accountRepository);
            var account = new AccountDto { Code = "420000000", Name = "Test2", Description = "" };
            var result = entityUnderTest.Post(account);
            result.Value.AccountId.Should().Be(10);
            result.Value.Name.Should().Be("Test");
            _accountRepository.DidNotReceive().Save(Arg.Any<Account>());
        }

        [Test]
        public void Post_will_not_accept_code_that_is_not_9_digits_long()
        {
            var entityUnderTest = new AccountController(_accountRepository);
            var account = new AccountDto { Code = "4200000", Name = "Test2", Description = "" };
            var result = entityUnderTest.Post(account);
            _accountRepository.DidNotReceive().Save(Arg.Any<Account>());
            result.Result.Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be(500);
        }
    }
}