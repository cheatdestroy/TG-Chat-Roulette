using TG.ChatBot.Common.Common.Attributes;
using TG.ChatBot.Common.Common.Enums;
using System.ComponentModel;

namespace TG.ChatBot.Common.Common.Helpers
{
    /// <summary>
    /// Вспомогательные функции для перечислений
    /// </summary>
    public static class EnumsExtension
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
        /// Получает значение атрибута Description
        /// </summary>
        /// <param name="value"> Значение перечисления </param>
        /// <param name="defaultValue"> Значение по умолчанию, если атрибут отсутствует или значение равно null </param>
        /// <returns> Если найдено, то возвращает значение атрибута; иначе значение по умолчанию  </returns>
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

        /// <summary>
        /// Получает значение атрибута AgeRange
        /// </summary>
        /// <param name="value"> Значение перечисления </param>
        /// <returns> Возвращает значение атрибута; иначе null </returns>
        public static (int Min, int Max)? GetAgeRange(this AgeCategory value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);

            if (name != null)
            {
                var field = type.GetField(name);

                if (field != null)
                {
                    var attr = (AgeRangeAttribute?)Attribute.GetCustomAttribute(field, typeof(AgeRangeAttribute));

                    if (attr != null)
                    {
                        return (attr.MinAge, attr.MaxAge);
                    }
                }
            }

            return null;
        }
    }
}
