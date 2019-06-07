using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VirtualPhysicsLab.Data.Models;
using VirtualPhysicsLab.Web.Interfaces;
using VirtualPhysicsLab.Web.Models;

namespace VirtualPhysicsLab.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/experiment")]
    [ApiController]
    public class ExperimentsController : ControllerBase
    {
        public IExperimentService ExperimentService{ get; }
        public IExperimentRepository ExperimentRepository{ get; }
        public IUserRepository UserRepository { get; }

        public ExperimentsController(IExperimentService experimentService, IExperimentRepository experimentRepository, IUserRepository userRepository)
        {
            ExperimentService = experimentService;
            ExperimentRepository = experimentRepository;
            UserRepository = userRepository;
        }

        // GET: api/Experiments
        [HttpGet]
        public async Task<IEnumerable<Experiment>> GetExperiments()
        {
            return await ExperimentRepository.GetAsync();
        }

        // GET: api/Experiments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExperiment([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var experiment = await ExperimentRepository.GetAsync(id);

            if (experiment == null)
            {
                return NotFound();
            }

            return Ok(experiment);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetExperimentsByUser([FromQuery] Guid id)
        {
            var result = await ExperimentRepository.GetByUserAsync(id);

            return Ok(result);
        }

        // PUT: api/Experiments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExperiment([FromRoute] Guid id, [FromBody] Experiment experiment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != experiment.Id)
            {
                return BadRequest();
            }

            await ExperimentRepository.UpdateAsync(experiment);

            return NoContent();
        }

        [HttpPut("settings")]
        public async Task<IActionResult> PutExperimentSettings([FromBody] ExperimentSettings experimentSettings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await ExperimentService.UpdateSettingsAsync(experimentSettings);

            return NoContent();
        }

        [HttpPost("check-availability")]
        public async Task<IActionResult> CheckAvailability([FromQuery] string title, [FromQuery] Guid userId)
        {
            var result = await ExperimentService.CheckAvailability(title, userId);

            return Ok(result);
        }

        // POST: api/Experiments
        [HttpPost]
        public async Task<IActionResult> PostExperiment([FromBody] Experiment experiment)
        {
            experiment.LogicalName = "experiment";
            experiment.Name = experiment.Title;
            experiment.CreatedOn = DateTime.Now;
            experiment.CreatedBy = await UserRepository.GetAsync(experiment.CreatedBy.Id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            experiment = await ExperimentRepository.CreateAsync(experiment);

            return Ok(experiment.Id);
        }

        [HttpPost("settings")]
        public async Task<IActionResult> PostExperimentSettings([FromBody] ExperimentSettings experimentSettings)
        {
            experimentSettings.LogicalName = "experiment_settings";
            experimentSettings.CreatedOn = DateTime.Now;
            experimentSettings.Experiment = await ExperimentRepository.GetByIdAsync(experimentSettings.ExperimentId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            experimentSettings = await ExperimentService.CreateSettingsAsync(experimentSettings);

            return CreatedAtAction("GetExperimentSettings", new { id = experimentSettings.Id }, experimentSettings);
        }

        // DELETE: api/Experiments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExperiment([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await ExperimentRepository.DeleteAsync(id);

            return Ok();
        }
    }
}