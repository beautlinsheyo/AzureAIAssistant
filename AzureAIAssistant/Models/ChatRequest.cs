using System.ComponentModel.DataAnnotations;
using AzureAIAssistant.Services;

namespace AzureAIAssistant.Models
{
    public class ChatRequest
    {
        [Required(ErrorMessage = "Message cannot be empty.")]
        [StringLength(2000 , ErrorMessage = "Message cannot exceed 2000 characters.")]
        public string UserMessage { get; set; } = string.Empty;
        public List<ChatTurn> History { get; set; } = new();
    }
}