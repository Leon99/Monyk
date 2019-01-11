using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Monyk.GroundControl.Db.Entities;
using Monyk.GroundControl.Services;

namespace Monyk.GroundControl.Main.Controllers
{
    [Route("api/v1/monitors")]
    //[Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class MonitorsController : ControllerBase
    {
        private readonly MonitorManager _service;

        public MonitorsController(MonitorManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MonitorEntity>>> GetMonitors()
        {
            var entities = await _service.GetMonitorsAsReadOnly();
            return entities.Any() ? (ActionResult<IEnumerable<MonitorEntity>>) Ok(entities) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MonitorEntity>> GetMonitor(Guid id)
        {
            var monitor = await _service.GetMonitor(id);

            return monitor ?? (ActionResult<MonitorEntity>)NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMonitor(Guid id, MonitorEntity monitor)
        {
            if (id != monitor.Id)
            {
                return BadRequest();
            }

            if (!await _service.TryUpdateMonitor(id, monitor))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<MonitorEntity>> CreateMonitor(MonitorEntity monitor)
        {
            await _service.CreateMonitor(monitor);

            return CreatedAtAction(nameof(GetMonitor), new {id = monitor.Id}, monitor);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMonitor(Guid id)
        {
            return await _service.TryDeleteMonitor(id) 
                ? (ActionResult)Ok() 
                : NotFound();
        }
    }
}
