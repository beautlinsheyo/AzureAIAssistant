using AzureAIAssistant.Data;
using AzureAIAssistant.Models;
using AzureAIAssistant.Services;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ChatMessage> CreateMessageAsync(ChatMessage message)
        {
            // Call the real AI instead of using placeholder text
            message.AIResponse = await _aiService.GetAIResponseAsync(message.UserMessage);
            message.Timestamp = DateTime.UtcNow;

            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }
    }
}