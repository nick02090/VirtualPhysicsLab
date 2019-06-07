using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualPhysicsLab.Data.Models;
using VirtualPhysicsLab.Web.Interfaces;

namespace VirtualPhysicsLab.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/mesh")]
    [ApiController]
    public class MeshController : ControllerBase
    {
        public IMeshService MeshService { get; }
        public IMeshRepository MeshRepository { get; }
        public IExperimentRepository ExperimentRepository { get;  }

        public MeshController(IMeshService meshService, IMeshRepository meshRepository, IExperimentRepository experimentRepository)
        {
            MeshService = meshService;
            MeshRepository = meshRepository;
            ExperimentRepository = experimentRepository;
        }

        // GET: api/Meshes
        [HttpGet]
        public async Task<IEnumerable<Mesh>> GetMeshes()
        {
            return await MeshRepository.GetAsync();
        }

        // GET: api/Meshes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMesh([FromQuery] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mesh = await MeshRepository.GetAsync(id);

            if (mesh == null)
            {
                return NotFound();
            }

            return Ok(mesh);
        }

        [HttpGet("experiment")]
        public async Task<IActionResult> GetMeshesByExperiment([FromQuery] Guid id)
        {
            var result = await MeshRepository.GetByExperimentAsync(id);

            return Ok(result);
        }

        // PUT: api/Meshes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMesh([FromRoute] Guid id, [FromBody] Mesh mesh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mesh.Id)
            {
                return BadRequest();
            }

            await MeshRepository.UpdateAsync(mesh);

            return NoContent();
        }

        [HttpPut("settings")]
        public async Task<IActionResult> PutMeshSettings([FromBody] MeshSettings meshSettings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await MeshService.UpdateSettingsAsync(meshSettings);

            return NoContent();
        }

        // POST: api/Meshes
        [HttpPost]
        public async Task<IActionResult> PostMesh([FromBody] Mesh mesh)
        {
            mesh.LogicalName = "mesh";
            mesh.CreatedOn = DateTime.Now;
            mesh.Experiment = await ExperimentRepository.GetByIdAsync(mesh.Experiment.Id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            mesh = await MeshRepository.CreateAsync(mesh);
            
            return Ok(mesh.Id);
        }

        [HttpPost("settings")]
        public async Task<IActionResult> PostMeshSettings([FromBody] MeshSettings meshSettings)
        {
            meshSettings.LogicalName = "experiment_settings";
            meshSettings.CreatedOn = DateTime.Now;
            meshSettings.Mesh = await MeshRepository.GetAsync(meshSettings.MeshId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            meshSettings = await MeshService.CreateSettingsAsync(meshSettings);

            return CreatedAtAction("GetMeshSettings", new { id = meshSettings.Id }, meshSettings);
        }

        // DELETE: api/Meshes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMesh([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await MeshRepository.DeleteAsync(id);

            return Ok();
        }
    }
}