using System;
using System.Collections.Generic;

namespace ChatBot.Anonymous.Domain.Entities
{
    public partial class User
    {
        public User()
        {
            ChatRoomFirstUsers = new HashSet<ChatRoom>();
            ChatRoomSecondUsers = new HashSet<ChatRoom>();
        }

        public long UserId { get; set; }
        public int? Gender { get; set; }
        public int? Age { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual UserSetting? UserSetting { get; set; }
        public virtual ICollection<ChatRoom> ChatRoomFirstUsers { get; set; }
        public virtual ICollection<ChatRoom> ChatRoomSecondUsers { get; set; }
    }
}
