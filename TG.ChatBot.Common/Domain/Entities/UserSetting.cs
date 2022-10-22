using System;
using System.Collections.Generic;

namespace TG.ChatBot.Common.Domain.Entities
{
    public partial class UserSetting
    {
        public long UserId { get; set; }
        public int? PreferredGender { get; set; }
        public int? PreferredAge { get; set; }
        public int? PreferredChatType { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
