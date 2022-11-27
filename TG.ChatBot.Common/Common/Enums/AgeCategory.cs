using TG.ChatBot.Common.Common.Attributes;
using System.ComponentModel;

namespace TG.ChatBot.Common.Common.Enums
{
    /// <summary>
    /// Категория возраста собеседника
    /// </summary>
    public enum AgeCategory
    {
        /// <summary>
        /// До 18 лет
        /// </summary>
        [AgeRange(MaxAge = 17)]
        LessThanEighteen = 1,

        /// <summary>
        /// От 18 до 21 включительно
        /// </summary>
        [AgeRange(MinAge = 18, MaxAge = 21)]
        LessThanTwentyOne = 2,

        /// <summary>
        /// От 22 до 25 включительно
        /// </summary>
        [AgeRange(MinAge = 22, MaxAge = 25)]
        LessThanTwentyFive = 3,

        /// <summary>
        /// От 26 до 30 включительно
        /// </summary>
        [AgeRange(MinAge = 26, MaxAge = 30)]
        LessThanThirty = 4,

        /// <summary>
        /// 31 год и выше
        /// </summary>
        [AgeRange(MinAge = 31)]
        MoreThanThirty = 5
    }
}
