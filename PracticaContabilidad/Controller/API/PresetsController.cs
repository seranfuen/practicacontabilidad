using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticaContabilidad.Model;
using PracticaContabilidad.Model.Repositories;

namespace PracticaContabilidad.Controller.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresetsController : ControllerBase
    {
        private readonly IJournalEntryGroupPresetRepository _repository;

        public PresetsController(IJournalEntryGroupPresetRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpPost]
        public ActionResult<JournalEntryGroupPreset> Post(JournalEntryGroupPreset journalEntryPreset)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status500InternalServerError);

            _repository.Save(journalEntryPreset);
            return journalEntryPreset;
        }
    }
}