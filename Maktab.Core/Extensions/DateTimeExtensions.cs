namespace Maktab.Core.Extensions
{
     public static class DateTimeExtensions
     {
          public static int GetAge(this DateTime dateOfBirth)
          {
               int age = 0;

               // Get today's date
               DateTime today = DateTime.Today;

               // Calculate the age in years
               age = today.Year - dateOfBirth.Year;

               // Adjust age if the birthday hasn't occurred yet this year
               if (dateOfBirth > today.AddYears(-age))
               {
                    age--;
               }

               return age;
          }
     }
}
