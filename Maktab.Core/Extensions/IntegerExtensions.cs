namespace Maktab.Core.Extensions
{
     public static class IntegerExtensions
     {
          public static float MonthToYears(this int ageInMonths)
          {
               return ageInMonths / 12.0f;
          }

          public static int MonthToYearsCeiling(this int ageInMonths)
          {
               return Convert.ToInt32(Math.Ceiling(MonthToYears(ageInMonths)));
          }
     }
}
