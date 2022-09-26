namespace ChatBot.Anonymous.Common.Helpers
{
    /// <summary>
    /// Вспомогательные функции для перечислений
    /// </summary>
    public static class EnumsHelper
    {
        /// <summary>
        /// Проверяет на отсутствие указанного числа в перечислении
        /// </summary>
        /// <param name="value"> Число </param>
        /// <param name="enumType"> Перечисление </param>
        /// <returns> Возвращает true, если указанное число отсуствует в перечислении; иначе false </returns>
        public static bool IsNotOwnerValue(this int? value, Type enumType)
        {
            return (!value.HasValue || !Enum.IsDefined(enumType, value));
        }

        /// <summary>
        /// Проверяет на отсутствие указанного числа в перечислении
        /// </summary>
        /// <param name="value"> Число </param>
        /// <param name="enumType"> Перечисление </param>
        /// <returns> Возвращает true, если указанное число отсуствует в перечислении; иначе false </returns>
        public static bool IsNotOwnerValue(this int value, Type enumType)
        {
            return !Enum.IsDefined(enumType, value);
        }
    }
}
