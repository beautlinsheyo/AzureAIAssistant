using Microsoft.EntityFrameworkCore;
using AzureAIAssistant.Models;
using AzureAIAssistant.Data;


namespace AzureAIAssistant.Services
{
    public class ChatService : IChatService
    {
        private readonly AppDbContext _context;
        private readonly IAIService _aiService;

        public ChatService(AppDbContext context, IAIService aiService)
        {
            _context = context;
            _aiService = aiService;
        }

        public async Task<IEnumerable<ChatMessage>> GetAllMessagesAsync()
        {
            return await _context.ChatMessages.ToListAsync();
        }

        public async Task<ChatMessage?> GetMessageByIdAsync(int id)
        {
            return await _context.ChatMessages.FindAsync(id);
        }

        public async Task<ChatMessage> CreateMessageAsync(string userMessage, List<ChatTurn> history)
        {
            // Add the new user message to the conversation history
            history.Add(new ChatTurn { Role = "user", Content = userMessage });

            // Send the FULL conversation (with memory) to the AI
            var aiReply = await _aiService.GetAIResponseAsync(history);

            var message = new ChatMessage
            {
                UserMessage = userMessage,
                AIResponse = aiReply,
                Timestamp = DateTime.UtcNow
            };

            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }
    }
}