using System;
using System.Collections.Generic;

namespace TG.ChatBot.Host.Domain.Entities
{
    public partial class ChatRoom
    {
        public Guid Id { get; set; }
        public long FirstUserId { get; set; }
        public long SecondUserId { get; set; }
        public int NumberMessagesFirstUser { get; set; }
        public int NumberMessagesSecondUser { get; set; }
        public int StatusRoom { get; set; }
        public long? InitiatorEndId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual User FirstUser { get; set; } = null!;
        public virtual User SecondUser { get; set; } = null!;
    }
}
