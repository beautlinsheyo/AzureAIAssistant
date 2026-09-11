using Microsoft.EntityFrameworkCore;
using AzureAIAssistant.Models;
namespace AzureAIAssistant.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
