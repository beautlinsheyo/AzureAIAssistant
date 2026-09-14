using AzureAIAssistant.Models;
namespace AzureAIAssistant.Services
{
    public interface IChatService
    {
        
            Task<IEnumerable<ChatMessage>> GetAllMessagesAsync();
            Task<ChatMessage?> GetMessageByIdAsync(int id);
            Task<ChatMessage> CreateMessageAsync(ChatMessage message);
        
    }
}
