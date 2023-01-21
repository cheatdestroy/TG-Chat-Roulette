namespace TG.ChatBot.Common.Domain.Entities
{
    public partial class IgnoreUsers
    {
        public long UserId { get; set; }
        public long IgnoredUserId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual User IgnoredUser { get; set; } = null!;
    }
}
