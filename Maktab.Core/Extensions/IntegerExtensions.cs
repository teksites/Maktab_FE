namespace Maktab.Core.Extensions
{
     public static class IntegerExtensions
     {
          public static float MonthToYears(this int ageInMonths)
          {
               return ageInMonths / 12.0f;
          }

          public static string MonthToYearsInString(this int ageInMonths)
          {
               if (ageInMonths > 0)
               {
                    int years = ageInMonths / 12;   // Integer division gives full years
                    int months = ageInMonths % 12;  // Remainder gives leftover months

                    if (years > 0)
                    {
                         if (months > 0)
                         {
                              return $"{years}y {months}m";
                         }
                         else
                         {
                              return $"{years}y";
                         }
                    }

                    if (months > 0)
                    {
                         return $"{months}m";
                    }
               }

               return string.Empty;
          }

          public static int MonthToYearsAdjustedMidPoint(this int ageInMonths)
          {
               var years = MonthToYears(ageInMonths);
               var yearInt = Convert.ToInt32(years);
               var executeCeling = years - yearInt > 0.4;
               return executeCeling ? Convert.ToInt32(Math.Ceiling(years)) : yearInt;

          }
     }
}
