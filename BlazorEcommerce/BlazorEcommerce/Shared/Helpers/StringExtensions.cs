namespace BlazorEcommerce.Shared.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Check if in the source the value chosed is equals but putted to upper to check
        /// </summary>
        /// <param name="source">
        /// The source strings
        /// </param>
        /// <param name="value">
        /// The value to be equal with
        /// </param>
        /// <returns>
        /// If Equals or not
        /// </returns>
        public static bool ToUpperEquals(this string source, string value)
        {
            if (string.IsNullOrWhiteSpace(source) && string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var response = source.ToUpper().Equals(value.ToUpper());

            return response;
        }

        /// <summary>
        /// Check if in the int32 source the value chosed is equals but putted to upper to check
        /// </summary>
        /// <param name="source">
        /// The source int32
        /// </param>
        /// <param name="value">
        /// The value to be equal with
        /// </param>
        /// <returns>
        /// If Equals or not
        /// </returns>
        public static bool ToUpperEquals(this int source, string value)
        {
            if (string.IsNullOrWhiteSpace(source.ToString()) && string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(source.ToString()) || string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var response = source.ToString().ToUpper().Equals(value.ToUpper());

            return response;
        }

        /// <summary>
        /// Check if in the source the value chosed is contained but putted to upper to check
        /// </summary>
        /// <param name="source">
        /// The source strings
        /// </param>
        /// <param name="value">
        /// The value to contains in
        /// </param>
        /// <returns>
        /// If contains or not
        /// </returns>
        public static bool ToUpperContains(this string source, string value)
        {
            if (string.IsNullOrWhiteSpace(source) && string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var response = source.ToUpper().Contains(value.ToUpper());

            return response;
        }

        /// <summary>
        /// Check if in the source the value chosed is equals but putted to lower to check
        /// </summary>
        /// <param name="source">
        /// The source strings
        /// </param>
        /// <param name="value">
        /// The value to be equal with
        /// </param>
        /// <returns>
        /// If Equals or not
        /// </returns>
        public static bool ToLowerEquals(this string source, string value)
        {
            if (string.IsNullOrWhiteSpace(source) && string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var response = source.ToLower().Equals(value.ToLower());

            return response;
        }

        /// <summary>
        /// Check if in the int32 source the value chosed is equals but putted to lower to check
        /// </summary>
        /// <param name="source">
        /// The source int32
        /// </param>
        /// <param name="value">
        /// The value to be equal with
        /// </param>
        /// <returns>
        /// If Equals or not
        /// </returns>
        public static bool ToLowerEquals(this int source, string value)
        {
            if (string.IsNullOrWhiteSpace(source.ToString()) && string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(source.ToString()) || string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var response = source.ToString().ToLower().Equals(value.ToLower());

            return response;
        }

        /// <summary>
        /// Check if in the source the value chosed is contained but putted to lower to check
        /// </summary>
        /// <param name="source">
        /// The source strings
        /// </param>
        /// <param name="value">
        /// The value to contains in
        /// </param>
        /// <returns>
        /// If contains or not
        /// </returns>
        public static bool ToLowerContains(this string source, string value)
        {
            if (string.IsNullOrWhiteSpace(source) && string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var response = source.ToLower().Contains(value.ToLower());

            return response;
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified string in the current
        /// instance are replaced with nothing.
        /// </summary>
        /// <param name="source">
        /// The current string.
        /// </param>
        /// <param name="value">
        /// The string to be replaced.
        /// </param>
        /// <returns>
        /// A string that is equivalent to the current string except that all instances of value are
        /// replaced with nothing. If value is not found in the current instance, the method returns
        /// the current instance unchanged.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// source is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// source is the empty string ("").
        /// </exception>
        public static string Replace(this string source, string value)
        {
            return source.Replace(value, "");
        }
    }
}
