using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Monyk.GroundControl.Models;
using Monyk.GroundControl.Services;

namespace Monyk.GroundControl.Main.Controllers
{
    [Route("api/v1/monitors")]
    [Produces("application/json")]
    public class MonitorsController : ControllerBase
    {
        private readonly MonitorManager _service;

        public MonitorsController(MonitorManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Monitor>>> GetMonitors()
        {
            var entities = await _service.GetMonitorsAsReadOnly();
            return entities.Any() ? (ActionResult<IEnumerable<Monitor>>) Ok(entities) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Monitor>> GetMonitor(Guid id)
        {
            var monitor = await _service.GetMonitor(id);

            return monitor ?? (ActionResult<Monitor>)NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMonitor(Guid id, Monitor monitor)
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
        public async Task<ActionResult<Monitor>> CreateMonitor(Monitor monitor)
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
