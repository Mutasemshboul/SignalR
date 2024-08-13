using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR.Data;

namespace SignalR.Models
{
    public class ChatHub:Hub
    {
        private readonly ApplicationDbContext _context;
        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SendMessageToAll(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        // Send a private message to a specific connection ID
        public async Task SendPrivateMessage(string receiverUserId, string message)
        {
            var senderUserId = Context.UserIdentifier;

            // Save the message to the database
            var chatMessage = new ChatMessage
            {
                SenderId = senderUserId,
                ReceiverId = receiverUserId,
                Message = message,
                Timestamp = DateTime.Now
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            // Send the message to the receiver
            await Clients.User(receiverUserId).SendAsync("ReceiveMessage", Context.User.Identity.Name, message);

            
        }

        // Optional: Method to get the connection ID of the current user
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
