using System.Collections.Generic;
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
    public class PresetsControllerTests
    {
        private IJournalEntryGroupPresetRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = Substitute.For<IJournalEntryGroupPresetRepository>();
        }

        [Test]
        public void Post_with_model_ok_calls_repository()
        {

            var entityUnderTest = new PresetsController(_repository);
            var entity = new JournalEntryGroupPreset
            {
                PresetEntries = new List<JournalEntryPresetEntry>
                {
                    new JournalEntryPresetEntry
                    {
                        Account = new Account { AccountId = 5},
                        DebitCredit = DebitCredit.Debit,
                        Order = 1
                    }
                }
            };
            entityUnderTest.Post(entity);

            _repository.Received(1).Save(entity);
        }

        [Test]
        public void Post_with_model_not_ok_does_not_call_repository_returns_error()
        {
            var entityUnderTest = new PresetsController(_repository);
            entityUnderTest.ModelState.AddModelError("", "Error");
            var result = entityUnderTest.Post(new JournalEntryGroupPreset());
            _repository.DidNotReceiveWithAnyArgs().Save(default);
            result.Result.Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be(500);
        }
    }
}