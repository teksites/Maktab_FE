using System.Globalization;

namespace Maktab.Core.Extensions
{
     public static class StringExtensions
     {
          public static string ToTitleCase(this string text)
          {
               if (string.IsNullOrEmpty(text))
                    return text;

               TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
               return textInfo.ToTitleCase(text.ToLower());
          }
     }
}
