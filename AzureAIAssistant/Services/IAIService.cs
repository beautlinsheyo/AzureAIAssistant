namespace AzureAIAssistant.Services
{
    public interface IAIService
    {
        Task<string> GetAIResponseAsync(string userMessage);
    }
}
