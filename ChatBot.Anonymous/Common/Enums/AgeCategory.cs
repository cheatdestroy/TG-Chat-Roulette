using System.ComponentModel;

namespace ChatBot.Anonymous.Common.Enums
{
    /// <summary>
    /// Категория возраста собеседника
    /// </summary>
    public enum AgeCategory
    {
        /// <summary>
        /// До 18 лет
        /// </summary>
        [Description("до 18")]
        LessThanEighteen = 1,

        /// <summary>
        /// От 18 до 21 включительно
        /// </summary>
        [Description("от 18 до 21")]
        LessThanTwentyOne = 2,

        /// <summary>
        /// От 22 до 25 включительно
        /// </summary>
        [Description("от 22 до 25")]
        LessThanTwentyFive = 3,

        /// <summary>
        /// От 26 до 30 включительно
        /// </summary>
        [Description("от 26 до 30")]
        LessThanThirty = 4,

        /// <summary>
        /// 31 год и выше
        /// </summary>
        [Description("31 и выше")]
        MoreThanThirty = 5
    }
}
