using AzureAIAssistant.Data;
using AzureAIAssistant.Models;
using Microsoft.EntityFrameworkCore;

namespace AzureAIAssistant.Services
{
    public class ChatService : IChatService
    {
        private readonly AppDbContext _context;

        public ChatService(AppDbContext context)
        {
            _context = context;
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
            message.AIResponse = "This is a placeholder response. Azure AI comes later!";
            message.Timestamp = DateTime.UtcNow;

            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }
    }
}