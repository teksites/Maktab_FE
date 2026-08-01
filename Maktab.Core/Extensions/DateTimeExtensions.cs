namespace Maktab.Core.Extensions
{
     public static class DateTimeExtensions
     {
          public static int GetAgeInYearsInt(this DateTime dateOfBirth)
          {
               return GetAgeInYearsInt(dateOfBirth, DateTime.Today);
          }

          public static int GetAgeInYearsInt(this DateTime dateOfBirth, DateTime referenceDate)
          {
               int age = 0;

               // Get today's date
               DateTime today = referenceDate;

               // Calculate the age in years
               age = today.Year - dateOfBirth.Year;

               // Adjust age if the birthday hasn't occurred yet this year
               if (dateOfBirth > today.AddYears(-age))
               {
                    age--;
               }

               return age;
          }

          public static float GetAgeInYearsFloat(this DateTime dateOfBirth, DateTime referenceDate)
          {
               //We need to get age in float number like 4.5 years old
               var ageInDays = (referenceDate - dateOfBirth).TotalDays;
               return (float)ageInDays / 365.25f;
          }

          public static float GetAgeInMonths(this DateTime dateOfBirth, DateTime referenceDate)
          {
               return GetAgeInYearsFloat(dateOfBirth, referenceDate) * 12;
          }
     }
}
