namespace Maktab.Core.Extensions
{
     public static class IntegerExtensions
     {
          public static float MonthToYears(this int ageInMonths)
          {
               return ageInMonths / 12.0f;
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
