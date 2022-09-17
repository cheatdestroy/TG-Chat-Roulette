using System;
using System.Collections.Generic;

namespace ChatBot.Anonymous.Domain.Entities
{
    public partial class UserSetting
    {
        public long UserId { get; set; }
        public int? PreferredGender { get; set; }
        public int? PreferredAge { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
