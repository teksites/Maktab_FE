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
        /// Validate Canadian Social Insurance Number (SIN).
        /// Rules:
        /// - Must be exactly 9 digits
        /// - First digit must be 1-7 (not 0, 8, or 9)
        /// - Must pass Luhn checksum validation
          /// </summary>
        /// <param name="sin">SIN number (with or without formatting)</param>
        /// <returns>Collection of error messages. Empty if valid.</returns>
          public static IEnumerable<string> ValidateCanadianIndividualTaxCode(string sin)
          {
            // Remove formatting characters
               sin = sin.RemoveSpecialCharacthers();

            // ✅ FIXED: Inverted logic - check if NOT 9 digits
            if (!Regex.IsMatch(sin, @"^\d{9}$"))
               {
                yield return "SIN_InvalidFormat"; // Must be 9 digits
                yield break; // Stop processing - no point continuing
               }

            var charDigits = sin.ToCharArray();
            int[] digits = new int[charDigits.Length];

            // Parse all characters as digits
            for (int i = 0; i < charDigits.Length; i++)
               {
                if (!int.TryParse(charDigits[i].ToString(), out digits[i]))
                    {
                    yield return "SIN_InvalidFormat_OnlyDigits";
                    yield break;
                    }
               }

            // ✅ NEW: Validate first digit (must be 1-7, not 0, 8, or 9)
            int firstDigit = digits[0];
            if (firstDigit == 0 || firstDigit == 8 || firstDigit == 9)
            {
                yield return "SIN_InvalidFirstDigit"; // Invalid SIN prefix
                yield break;
            }

            // Calculate checksum using Luhn algorithm
               var total = digits.Where((value, index) => index % 2 == 0 && index != 8).Sum()
                        + digits.Where((value, index) => index % 2 != 0)
                              .Select(v => v * 2)
                              .SelectMany(v => v.ToDigitEnumerable())
                              .Sum();

               var checkDigit = (10 - (total % 10)) % 10;
               bool isValid = digits.Last() == checkDigit;

            // ✅ FIXED: Return error message instead of empty string
            if (!isValid)
            {
                yield return "SIN_Invalid_Checksum";
            }
            // If valid, don't yield anything (empty enumeration means valid)
        }

        /// <summary>
        /// Check if SIN is valid (simpler boolean version).
        /// </summary>
        public static bool IsValidCanadianSIN(string sin)
        {
            var errors = ValidateCanadianIndividualTaxCode(sin);
            return !errors.Any(); // Valid if no errors
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
