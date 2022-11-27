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
        /// Получает атрибут перечисления или структуры
        /// </summary>
        /// <typeparam name="TResult"> Тип возвращаемого значения </typeparam>
        /// <typeparam name="TParam"> Тип значения </typeparam>
        /// <param name="value"> Значение </param>
        /// <returns> Если атрибут найден, то возвращает атрибут структуры/перечисления; иначе null </returns>
        public static TResult? GetCustomAttribute<TResult, TParam>(this TParam value)
            where TResult : Attribute
            where TParam : struct, Enum
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);

            if (name != null)
            {
                var field = type.GetField(name);

                if (field != null)
                {
                    var attr = (TResult?)Attribute.GetCustomAttribute(field, typeof(TResult));

                    if (attr != null)
                    {
                        return attr;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Получает минимальный и максимальный возраст из атрбитута AgeRange
        /// </summary>
        /// <param name="value"> Значение у которого есть атрибут AgeRange </param>
        /// <returns> Если атрибут найден, то возвращает кортеж с минимальным и максимальным значением; иначе null </returns>
        public static (int? Min, int? Max)? GetAgeRange(this AgeCategory value)
        {
            var result = value.GetCustomAttribute<AgeRangeAttribute, AgeCategory>();

            return result != null ? (result.MinAge, result.MaxAge) : null;
        }

        /// <summary>
        /// Получает описание из атрибута AgeRange
        /// </summary>
        /// <param name="value"> Значение у которого есть атрибут AgeRange </param>
        /// <returns> Если атрибут найден, то возвращает описание; иначе null </returns>
        public static string GetAgeRangeDescription(this AgeCategory value)
        {
            var result = value.GetCustomAttribute<AgeRangeAttribute, AgeCategory>();

            return result?.ToString() ?? string.Empty;
        }
    }
}
