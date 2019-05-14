using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Monyk.GroundControl.Models;
using Monyk.GroundControl.Services;

namespace Monyk.GroundControl.Api.Controllers
{
    [Route("monitors")]
    [Consumes("application/json")]
    [Produces("application/json")]
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
        public async Task<IActionResult> UpdateMonitor(Guid id, MonitorEntity monitorEntity)
        {
            if (id != monitorEntity.Id)
            {
                return BadRequest();
            }

            if (!await _service.TryUpdateMonitor(id, monitorEntity))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<MonitorEntity>> CreateMonitor([FromBody]MonitorEntity monitorEntity)
        {
            await _service.CreateMonitor(monitorEntity);

            return CreatedAtAction(nameof(GetMonitor), new {id = monitorEntity.Id}, monitorEntity);
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
