using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Helpers
{
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class FileHelper
    {
        public static string SanitizeFileName(string fileName)
        {
            // Loại bỏ ký tự Unicode
            string normalized = fileName.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            string sanitized = sb.ToString().Normalize(NormalizationForm.FormC);

            // Thay khoảng trắng và ký tự đặc biệt
            sanitized = Regex.Replace(sanitized, @"[^a-zA-Z0-9_.]+", "_");

            return sanitized;
        }
    }

}
