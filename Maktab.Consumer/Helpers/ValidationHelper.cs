using System.Text;
using System.Text.RegularExpressions;

namespace Maktab.Consumer.Helpers
{
     public static class ValidationHelper
     {

          public static string RemoveSpecialCharacthers(this string ssn)
          {
               StringBuilder sb = new StringBuilder();

               for (int i = 0; i < ssn?.Length; i++)
               {
                    if (char.IsLetterOrDigit(ssn[i]))
                    {
                         sb.Append(ssn[i]);
                    }
               }
               return sb.ToString();
          }

          public static IEnumerable<int> ToDigitEnumerable(this int number)
          {
               IList<int> digits = new List<int>();

               while (number > 0)
               {
                    digits.Add(number % 10);
                    number = number / 10;
               }

               //digits are currently backwards, reverse the order
               return digits.Reverse();
          }

          /// <summary>
          /// Validate SIN
          /// </summary>
          /// <param name="sin"></param>
          /// <returns></returns>
          public static IEnumerable<string> ValidateCanadianIndividualTaxCode(string sin)
          {
               sin = sin.RemoveSpecialCharacthers();

               var chardDigits = sin.ToCharArray();
               if (Regex.IsMatch(sin, @"^\d{9}$"))
               {
                    yield return "SIN_InvalidFormat";//("123-456-789");
               }

               int[] digits = new int[chardDigits.Length];
               for (int i = 0; i < chardDigits.Length; i++)
               {
                    if (!int.TryParse(chardDigits[i].ToString(), out digits[i]))
                    {
                         yield return "SIN_InvalidFormat_OnlyDigits";//ValidationResult.Invalid("Invalid format! Only digits are allowed");
                    }
               }

               var total = digits.Where((value, index) => index % 2 == 0 && index != 8).Sum()
                           + digits.Where((value, index) => index % 2 != 0).Select(v => v * 2)
                                 .SelectMany(v => v.ToDigitEnumerable()).Sum();

               var checkDigit = (10 - (total % 10)) % 10;

               bool isValid = digits.Last() == checkDigit;

               yield return isValid ? string.Empty : "SIN_Invalid"; //ValidationResult.Success() : ValidationResult.InvalidChecksum();

          }


          public static IEnumerable<string> PasswordStrength(string pw)
          {
               if (string.IsNullOrWhiteSpace(pw))
               {
                    yield return "Password is required!";
                    yield break;
               }
               if (pw.Length < 8)
                    yield return "Password must be at least 8 characters long.";
               if (!Regex.IsMatch(pw, @"[A-Z]"))
                    yield return "Password must contain at least one uppercase letter.";
               if (!Regex.IsMatch(pw, @"[a-z]"))
                    yield return "Password must contain at least one lowercase letter.";
               if (!Regex.IsMatch(pw, @"[0-9]"))
                    yield return "Password must contain at least one digit.";
          }
     }
}
