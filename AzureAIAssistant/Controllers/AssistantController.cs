using AzureAIAssistant.Data;
using AzureAIAssistant.Models;
using AzureAIAssistant.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzureAIAssistant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssistantController : ControllerBase
    {
        private readonly IChatService _chatService;

        public AssistantController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatMessage>>> GetAll()
        {
            var messages = await _chatService.GetAllMessagesAsync();
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChatMessage>> GetById(int id)
        {
            var message = await _chatService.GetMessageByIdAsync(id);
            if (message == null) return NotFound();
            return Ok(message);
        }
        [HttpPost]
        public async Task<ActionResult<ChatMessage>> Create(ChatRequest request)
        {
            var created = await _chatService.CreateMessageAsync(request.UserMessage, request.History);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}
