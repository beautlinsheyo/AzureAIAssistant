using AzureAIAssistant.Data;
using AzureAIAssistant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzureAIAssistant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssistantController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AssistantController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Assistant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatMessage>>> GetAll()
        {
            var messages = await _context.ChatMessages.ToListAsync();
            return Ok(messages);
        }

        // GET: api/Assistant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatMessage>> GetById(int id)
        {
            var message = await _context.ChatMessages.FindAsync(id);

            if (message == null)
                return NotFound();

            return Ok(message);
        }

        // POST: api/Assistant
        [HttpPost]
        public async Task<ActionResult<ChatMessage>> Create(ChatMessage message)
        {
            message.AIResponse = "This is a placeholder response. Azure AI comes later!";
            message.Timestamp = DateTime.UtcNow;

            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = message.Id }, message);
        }
    }
}
