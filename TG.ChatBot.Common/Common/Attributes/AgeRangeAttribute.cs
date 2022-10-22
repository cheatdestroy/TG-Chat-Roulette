namespace TG.ChatBot.Common.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class AgeRangeAttribute : Attribute
    {
        public int MinAge { get; private set; }
        public int MaxAge { get; private set; }

        public AgeRangeAttribute(int minAge, int maxAge)
        {
            MinAge = minAge;
            MaxAge = maxAge;
        }
    }
}
