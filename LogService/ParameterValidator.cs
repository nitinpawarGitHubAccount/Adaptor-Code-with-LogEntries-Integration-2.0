using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Avalara.AvaTax.Adapter.LogService
{
    internal static class ParameterValidator
    {
        [DebuggerHidden]
        public static void MustContainElements(Array array, string paramName)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException(string.Format("Parameter '{0}' must contain elements.", paramName), paramName);
            }
        }

        [DebuggerHidden]
        public static void MustNotBeNull<T>(T @object, string paramName)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(string.Format("Parameter '{0}' cannot be null.", paramName), paramName);
            }
        }

        [DebuggerHidden]
        public static void MustNotBeNullOrWhitespace(string @string, string paramName)
        {
            //if (string.IsNullOrWhiteSpace(@string))
            //{
            //    throw new ArgumentException(string.Format("Parameter '{0}' cannot be null or whitespace.", paramName), paramName);
            //}
        }

        [DebuggerHidden]
        public static void MustNotEqual<T>(T argument, T value, string paramName) where T : IComparable
        {
            if (EqualityComparer<T>.Default.Equals(argument, value))
            {
                throw new ArgumentOutOfRangeException(string.Format("Parameter '{0}' cannot equal '{1}'.", paramName, value), paramName);
            }
        }

        [DebuggerHidden]
        public static void MustBeGreaterThan<T>(T obj, string paramName, T value)
            where T : IComparable
        {
            if (Comparer<T>.Default.Compare(value, obj) >= 0)
            {
                throw new ArgumentException(string.Format("Parameter '{0}' must be greater than '{1}'.", paramName, value), paramName);
            }
        }
    }
}
