using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings;

namespace MovieCLI
{
    static class StringExtensionMethods
    {
        /// <summary>
        /// This strips all special diacritics and accents from strings.
        /// Very useful for searching foreign movies
        /// </summary>
        /// <param name="str"></param>
        /// <returns>string without diacritics</returns>
        public static string StripDiacritics(this string str)
        {
            string normalized = str.Normalize(NormalizationForm.FormD);
            StringBuilder builder = new StringBuilder();

            foreach (char c in normalized)
            {
                var v = CharUnicodeInfo.GetUnicodeCategory(c);
                if (v != UnicodeCategory.NonSpacingMark)
                    builder.Append(c);
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
