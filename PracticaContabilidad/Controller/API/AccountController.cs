using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticaContabilidad.Model;
using PracticaContabilidad.Model.Repositories;

namespace PracticaContabilidad.Controller.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _repository;

        public AccountController(IAccountRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public IEnumerable<Account> Get()
        {
            return _repository.Accounts;
        }

        [HttpPost]
        public ActionResult<Account> Post(AccountDto accountDto)
        {
            if (accountDto == null) throw new ArgumentNullException(nameof(accountDto));
            if (string.IsNullOrWhiteSpace(accountDto.Code)) throw new ArgumentException("The code is mandatory");
            if (string.IsNullOrWhiteSpace(accountDto.Name)) throw new ArgumentException("The name is mandatory");

            if (accountDto.Code.Length != 9 || !new Regex("^[0-9.]+$").IsMatch(accountDto.Code))
                return StatusCode(StatusCodes.Status500InternalServerError);

            var existingAccount = _repository.GetAccountWithCode(accountDto.Code);
            if (existingAccount != null) return existingAccount;

            var newAccount = new Account
            {
                Code = accountDto.Code,
                Name = accountDto.Name,
                Description = accountDto.Description
            };
            _repository.Save(newAccount);
            return newAccount;
        }
    }

    public class AccountDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}