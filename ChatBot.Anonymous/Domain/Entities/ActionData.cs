namespace ChatBot.Anonymous.Domain.Entities
{
    public partial class ActionData
    {
        public long ChatId { get; set; }
        public long UserId { get; set; }
        public int? CurrentAction { get; set; }
        public int? CurrentStep { get; set; }
        public DateTime LastUpdate { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
