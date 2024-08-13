using Microsoft.AspNetCore.Identity;

namespace SignalR.Models
{
    public class ChatViewModel
    {
        public IEnumerable<IdentityUser>? Users { get; set; }
        public IEnumerable<ChatMessage>? Messages { get; set; }
        public string CurrentUserId { get; set; }
    }
}
