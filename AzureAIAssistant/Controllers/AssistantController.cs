using Microsoft.AspNetCore.Mvc;
using AzureAIAssistant.Models;
using AzureAIAssistant.Services;

namespace AzureAIAssistant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssistantController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILogger<AssistantController> _logger;

        public AssistantController(IChatService chatService, ILogger<AssistantController> logger)
        {
            _chatService = chatService;
            _logger = logger;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var created = await _chatService.CreateMessageAsync(request.UserMessage, request.History);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process chat message.");
                return StatusCode(503, new { error = "The assistant is temporarily unavailable. Please try again shortly." });
            }
        }
    }
}