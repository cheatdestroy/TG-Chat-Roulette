namespace TG.ChatBot.Common.StepByStep.Enums
{
    /// <summary>
    /// Шаги последовательных действий для действия Start
    /// </summary>
    public enum Step
    {
        /// <summary>
        /// Указание пола
        /// </summary>
        Gender = 1000,

        /// <summary>
        /// Указание возраста
        /// </summary>
        Age = 1001,

        /// <summary>
        /// Указание типа общения в чате
        /// </summary>
        ChatType = 1002,

        /// <summary>
        /// Указание предпочитаемого пола собеседника
        /// </summary>
        PreferredGender = 1004,

        /// <summary>
        /// Указание предпочитаемого возраста собеседника
        /// </summary>
        PreferredAge = 1003,
    }
}
