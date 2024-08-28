using System;

namespace GPT4.Models
{
    public class ChatMessages
    {

        public string userMessage { get; set; }
        public string BotMessage { get; set; }
        public DateTime MessageTimeStamp { get; set; } = DateTime.UtcNow;

    }
}
