﻿using MVC_Project_Orange.Models;

namespace SignalR.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; } 

        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; } 

        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
