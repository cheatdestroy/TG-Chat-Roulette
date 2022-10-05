using ChatBot.Anonymous.Common.Enums;
using System.ComponentModel;

namespace ChatBot.Anonymous.Common.Helpers
{
    /// <summary>
    /// Вспомогательные функции для перечислений
    /// </summary>
    public static class EnumsHelper
    {
        /// <summary>
        /// Переводит тип данных int в указанный enum
        /// </summary>
        /// <typeparam name="T"> Перечесление </typeparam>
        /// <param name="value"> Значение </param>
        /// <returns> Возвращает значение перечисления, если оно присутствует в указанном перечислении; иначе null </returns>
        public static T? ToEnum<T>(this int value) where T : struct, Enum
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                return null;
            }

            return (T?)Enum.ToObject(typeof(T), value);
        }

        /// <summary>
        /// Переводит тип данных int? в указанный enum
        /// </summary>
        /// <typeparam name="T"> Перечесление </typeparam>
        /// <param name="value"> Значение </param>
        /// <returns> Возвращает значение перечисления, если оно присутствует в указанном перечислении; иначе null </returns>
        public static T? ToEnum<T>(this int? value) where T : struct, Enum
        {
            if (!value.HasValue)
            {
                return null;
            }

            return value.Value.ToEnum<T>();
        }

        /// <summary>
        /// Получает описание перечислений
        /// </summary>
        /// <param name="value"> Значение перечисления </param>
        /// <param name="defaultValue"> Значение по умолчанию, если атрибут отсутствует или значение равно null </param>
        /// <returns> Возвращает описание перечисления </returns>
        public static string GetDescription(this Enum? value, string defaultValue = "неизвестно")
        {
            if (value == null)
            {
                return defaultValue;
            }

            var type = value.GetType();
            var name = Enum.GetName(type, value);

            if (name != null)
            {
                var field = type.GetField(name);

                if (field != null)
                {
                    var attr = (DescriptionAttribute?)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }

            return defaultValue;
        }
    }
}
