namespace TG.ChatBot.Common.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class AgeRangeAttribute : Attribute
    {
        /// <summary>
        /// Минимальный возраст
        /// </summary>
        public int MinAge { get; set; }

        /// <summary>
        /// Максимальный возраст
        /// </summary>
        public int MaxAge { get; set; } = int.MaxValue;

        public AgeRangeAttribute()
        {
        }

        public override string ToString()
        {
            if (MinAge != default && MinAge != int.MinValue)
            {
                if (MaxAge != default && MaxAge != int.MaxValue)
                {
                    return $"от {MinAge} до {MaxAge}";
                }
                else
                {
                    return $"{MinAge} и выше";
                }
            }
            else if (MaxAge != default && MaxAge != int.MaxValue)
            {
                return $"до {MaxAge}";
            }

            return string.Empty;
        }
    }
}
