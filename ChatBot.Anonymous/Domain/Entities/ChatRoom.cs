using System;
using System.Collections.Generic;

namespace ChatBot.Anonymous.Domain.Entities
{
    public partial class ChatRoom
    {
        public Guid Id { get; set; }
        public int FirstUserId { get; set; }
        public int SecondUserId { get; set; }
        public int NumberMessagesFirstUser { get; set; }
        public int NumberMessagesSecondUser { get; set; }
        public int StatusRoom { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual User FirstUser { get; set; } = null!;
        public virtual User SecondUser { get; set; } = null!;
    }
}
