namespace ChatBot.Anonymous.Common.Enums.ActionSteps
{
    /// <summary>
    /// Шаги последовательных действий для действия Start
    /// </summary>
    public enum StartSteps
    {
        /// <summary>
        /// Указание пола
        /// </summary>
        GenderStep = 1000,

        /// <summary>
        /// Указание возраста
        /// </summary>
        AgeStep = 1001,

        /// <summary>
        /// Указание типа общения в чате
        /// </summary>
        ChatTypeStep = 1002
    }
}
