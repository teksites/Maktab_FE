// ─────────────────────────────────────────────────────────────────
// RamqValidationHelper.cs
// Zero-dependency static helper for RAMQ number validation.
// No FluentValidation, no NuGet packages required.
//
// RAMQ format stored as 12 clean chars (no spaces): AAAAYYMMDDSQ
//
//  Position  Length  Content
//  ────────  ──────  ──────────────────────────────────────────────
//  0–2          3    First 3 letters of last name  (uppercase)
//  3            1    First letter of first name    (uppercase)
//  4–5          2    Birth year   (YY)
//  6–7          2    Birth month  (MM) male=01–12 / female=51–62
//  8–9          2    Birth day    (DD)
//  10–11        2    Sequence number (01–99)
//
// All methods return:
//   null          → valid
//   string        → error message to show the user
// ─────────────────────────────────────────────────────────────────

using Maktab.Consumer.Localization;
using MaktabDataContracts.Enums;
using Microsoft.Extensions.Localization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

public static class RamqValidationHelper
{
    // ── Entry point — runs all rules, returns first error found ───
    // Call this from OnBlur to validate the full RAMQ at once.
    public static string? ValidateAll(
        string ramq,
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        Gender gender,
        IStringLocalizer<MaktabResources> stringLocalizer)
    {
        var clean = Clean(ramq);

        return ValidateFormat(clean, stringLocalizer)
            ?? ValidateLastName(clean, lastName, stringLocalizer)
            ?? ValidateFirstName(clean, firstName, stringLocalizer)
            ?? ValidateGender(clean, gender, stringLocalizer)
            ?? ValidateBirthDate(clean, dateOfBirth, gender, stringLocalizer)
            ?? ValidateSequence(clean, stringLocalizer);
    }

    // ── Individual rules ──────────────────────────────────────────
    // Useful if you want to validate specific fields independently.

    /// <summary>
    /// Rule 1 — Base format: 4 letters + 8 digits, exactly 12 chars.
    /// </summary>
    public static string? ValidateFormat(string ramq, IStringLocalizer<MaktabResources> stringLocalizer)
    {
        var clean = Clean(ramq);

        if (string.IsNullOrWhiteSpace(clean))
            return stringLocalizer[MaktabResources.Msg_Info_Value_Is_Required, MaktabResources.RAMQ_Number];

        if (clean.Length != 12)
            return stringLocalizer[MaktabResources.Msg_RAMQ_Length_Not_Valid];

        if (!Regex.IsMatch(clean, @"^[A-Za-z]{4}\d{8}$"))
            return stringLocalizer[MaktabResources.Msg_RAMQ_Pattern_Not_Valid];

        return null;
    }

     /// <summary>
     /// Rule 2 — Positions 1–3 must match first 3 letters of last name.
     /// </summary>
     public static string? ValidateLastName(string ramq, string lastName, IStringLocalizer<MaktabResources> stringLocalizer)
     {
          if (string.IsNullOrWhiteSpace(lastName))
          {
               return stringLocalizer[MaktabResources.Msg_Info_Value_Is_Required, MaktabResources.Last_Name];
          }

          var clean = Clean(ramq);
          var expected = NormalizeNamePart(lastName, 3);
          var actual = clean.Substring(0, 3).ToUpperInvariant();

          if (!string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase))
               return stringLocalizer[MaktabResources.Msg_RAMQ_Pattern_Mismatch_Last_Name, actual, lastName.ToUpper()];

          return null;
     }

    /// <summary>
    /// Rule 3 — Position 4 must match first letter of first name.
    /// </summary>
    public static string? ValidateFirstName(string ramq, string firstName, IStringLocalizer<MaktabResources> stringLocalizer)
    {
          if (string.IsNullOrWhiteSpace(firstName))
          {
               return stringLocalizer[MaktabResources.Msg_Info_Value_Is_Required, MaktabResources.First_Name];
          }
          var clean = Clean(ramq);

        var expected = NormalizeNamePart(firstName, 1);
        var actual   = clean.Substring(3, 1).ToUpperInvariant();

        if (!string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase))
               return stringLocalizer[MaktabResources.Msg_RAMQ_Pattern_Mismatch_First_Name, actual, firstName.ToUpper()];
          //return $"RAMQ position 4 ('{actual}') must match the first letter of first name '{firstName.ToUpper()}'. Expected '{expected}'.";

          return null;
    }

    /// <summary>
    /// Rule 4 — Birth month must match gender.
    /// Male: 01–12, Female: 51–62.
    /// </summary>
    public static string? ValidateGender(string ramq, Gender gender, IStringLocalizer<MaktabResources> stringLocalizer)
    {
          if (gender == Gender.Unknown)
          {
               return stringLocalizer[MaktabResources.Msg_Info_Value_Is_Required, MaktabResources.Gender];
          }

        var clean = Clean(ramq);

        if (!int.TryParse(clean.Substring(6, 2), out int month))
            return stringLocalizer[MaktabResources.Msg_RAMQ_Birth_Month_Invalid];

        var isMaleMonth   = month >= 1  && month <= 12;
        var isFemaleMonth = month >= 51 && month <= 62;

          if (gender == Gender.Male && isFemaleMonth)
          {
               return stringLocalizer[MaktabResources.Msg_RAMQ_Pattern_Mismatch_Gender, month.ToString("D2"), MaktabResources.Gender_Type_Male];
          }
          else if (gender == Gender.Female && isMaleMonth)
          {
               return stringLocalizer[MaktabResources.Msg_RAMQ_Pattern_Mismatch_Gender, month.ToString("D2"), MaktabResources.Gender_Type_Female];
          }

          return null;
    }

    /// <summary>
    /// Rule 5 — YYMMDD encoded in positions 5–10 must match date of birth.
    /// Female month is offset by 50 (subtract 50 before comparing).
    /// </summary>
    public static string? ValidateBirthDate(string ramq, DateTime dob, Gender gender, IStringLocalizer<MaktabResources> stringLocalizer)
    {
        var clean = Clean(ramq);

        if (!int.TryParse(clean.Substring(4, 2), out int yy)) return stringLocalizer[MaktabResources.Msg_RAMQ_Invalid_Birth_Year];
        if (!int.TryParse(clean.Substring(6, 2), out int mm)) return stringLocalizer[MaktabResources.Msg_RAMQ_Invalid_Birth_Month];
        if (!int.TryParse(clean.Substring(8, 2), out int dd)) return stringLocalizer[MaktabResources.Msg_RAMQ_Invalid_Birth_Day];

        // Female months are offset by +50 — subtract to get real month
        if (gender == Gender.Female) mm -= 50;

        var expectedYY = dob.Year % 100;
        var expectedMM = dob.Month;
        var expectedDD = dob.Day;

        if (yy != expectedYY || mm != expectedMM || dd != expectedDD)
        {
            var expectedPart = BuildExpectedDatePart(dob, gender);
            return stringLocalizer[MaktabResources.Msg_RAMQ_Pattern_Mismatch_Date_Of_Birth, dob.ToShortDateString()];
        }

        return null;
    }

    /// <summary>
    /// Rule 6 — Sequence (positions 11–12) must be 01–99. 00 is invalid.
    /// </summary>
    public static string? ValidateSequence(string? ramq, IStringLocalizer<MaktabResources> stringLocalizer)
    {
        var clean = Clean(ramq);

        if (!int.TryParse(clean.Substring(10, 2), out int seq) || seq < 1 || seq > 99)
            return stringLocalizer[MaktabResources.Msg_RAMQ_Invalid_Sequence_Number];

        return null;
    }

    // ── Private helpers ───────────────────────────────────────────

    /// <summary>
    /// Strips spaces the mask may have inserted before any validation.
    /// </summary>
    private static string Clean(string? value) =>
        (value ?? string.Empty).Replace(" ", "").Trim();

    /// <summary>
    /// Quick base format check used to gate cross-validation rules.
    /// Avoids showing cross-validation errors before the format is valid.
    /// </summary>
    private static bool IsBaseFormatValid(string? clean) =>
        !string.IsNullOrWhiteSpace(clean) &&
        clean.Length == 12 &&
        Regex.IsMatch(clean, @"^[A-Za-z]{4}\d{8}$");

    /// <summary>
    /// Normalizes a name part for RAMQ comparison:
    /// - Strips accents (é→e, à→a, ô→o)
    /// - Removes non-letter characters (hyphens, spaces, apostrophes)
    /// - Uppercases
    /// - Pads with X if shorter than required length (RAMQ convention)
    /// </summary>
    private static string NormalizeNamePart(string name, int length)
    {
        // Decompose accented chars then remove non-ASCII
        var decomposed = name.Normalize(NormalizationForm.FormD);
        var lettersOnly = Regex.Replace(decomposed, @"[^a-zA-Z]", "");
        var upper = lettersOnly.ToUpperInvariant();

        // Pad with X if name is shorter than required (RAMQ convention)
        return upper.Length >= length
            ? upper.Substring(0, length)
            : upper.PadRight(length, 'X');
    }

    private static string BuildExpectedDatePart(DateTime? dob, Gender gender)
    {
        if (!dob.HasValue) return "??????";
        var yy = dob.Value.Year % 100;
        var mm = gender == Gender.Female ? dob.Value.Month + 50 : dob.Value.Month;
        var dd = dob.Value.Day;
        return $"{yy:D2}{mm:D2}{dd:D2}";
    }
}
