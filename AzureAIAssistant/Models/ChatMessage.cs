namespace AzureAIAssistant.Models
{
    public class ChatMessage
    {
            public int Id { get; set; }

            public string UserMessage { get; set; } = string.Empty;

            public string? AIResponse { get; set; }

            public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
    }
}

