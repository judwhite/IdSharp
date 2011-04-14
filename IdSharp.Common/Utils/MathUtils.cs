using System;

namespace IdSharp.Common.Utils
{
    /// <summary>
    /// MathUtils
    /// </summary>
    public class MathUtils
    {
        /// <summary>
        /// Gets the maximum value from the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        public static int Max(params int[] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentNullException("array");

            int max = array[0];
            foreach (int val in array)
            {
                if (val > max) max = val;
            }

            return max;
        }

        /// <summary>
        /// Gets the minimum value from the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        public static int Min(params int[] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentNullException("array");

            int min = array[0];
            foreach (int val in array)
            {
                if (val < min) min = val;
            }

            return min;
        }

        /// <summary>
        /// Compares two double values for equality.
        /// </summary>
        /// <param name="double1">The first double.</param>
        /// <param name="double2">The second double.</param>
        /// <returns><c>true</c> if the values are equal; otherwise, <c>false</c>.</returns>
        public static bool DoublesAreEqual(double double1, double double2)
        {
            return (Math.Abs(double1 - double2) < double.Epsilon);
        }

        /// <summary>
        /// Determines whether the type is a numeric type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the type is a numeric type; otherwise, <c>false</c>.</returns>
        public static bool IsNumericType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return (type == typeof(int) ||
                    type == typeof(int?) ||
                    type == typeof(long) ||
                    type == typeof(long?) ||
                    type == typeof(short) ||
                    type == typeof(short?) ||
                    type == typeof(float) ||
                    type == typeof(float?) ||
                    type == typeof(double) ||
                    type == typeof(double?) ||
                    type == typeof(uint) ||
                    type == typeof(uint?) ||
                    type == typeof(ulong) ||
                    type == typeof(ulong?) ||
                    type == typeof(ushort) ||
                    type == typeof(ushort?) ||
                    type == typeof(decimal) ||
                    type == typeof(decimal?));
        }
    }
}
