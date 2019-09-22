using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Monitor.Models;
using System;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentDataController : ControllerBase
    {
        private readonly AgentDataContext _context;

        public AgentDataController(AgentDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [HttpGet("{id}")]
        public async Task<ActionResult<AgentData>> GetAgentData(long id)
        {
            var AgentDataItem = await _context.AgentDataEntities.FindAsync(id);

            if (AgentDataItem == null)
            {
                return NotFound();
            }

            return AgentDataItem;
        }

        [HttpPost]
        public async Task<ActionResult<AgentData>> PostAgentData(AgentData data)
        {
            if (_context.AgentDataEntities.Find(data.ID) == null)
            {
                _context.AgentDataEntities.Add(data);
                await _context.SaveChangesAsync();
            }           
           
            return CreatedAtAction(nameof(GetAgentData), data);

        }
        [HttpPut("{uid}")]
        public async Task<IActionResult> PutAgentData(string uid, AgentData data)
        {
            data.LastSeen = DateTime.Now;
            _context.Entry(data).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}