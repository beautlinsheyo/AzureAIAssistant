
using AzureAIAssistant.Services;

namespace AzureAIAssistant.Models
{
    public class ChatRequest
    {
        public string UserMessage { get; set; } = string.Empty;
        public List<ChatTurn> History { get; set; } = new();
    }
}