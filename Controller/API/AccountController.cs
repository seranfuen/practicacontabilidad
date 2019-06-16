using System;
using System.Collections.Generic;
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
    }
}