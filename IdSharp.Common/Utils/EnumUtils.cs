using System;
using System.ComponentModel;

namespace IdSharp.Common.Utils
{
    /// <summary>
    /// EnumUtils
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>Gets the <see cref="DescriptionAttribute.Description" /> of the specified <paramref name="enumValue"/> if it exists; otherwise returns <paramref name="enumValue" />.<see cref="System.String.ToString()"/>.</summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>The <see cref="DescriptionAttribute.Description" /> of the specified <paramref name="enumValue"/> if it exists; otherwise returns the value's string representation.</returns>
        public static string GetDescription<T>(T enumValue)
            where T : struct
        {
            string enumString = enumValue.ToString();
            object[] descriptions = typeof(T).GetField(enumString).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (descriptions.Length == 1)
                enumString = ((DescriptionAttribute)descriptions[0]).Description;
            return enumString;
        }
    }
}
