using MaktabDataContracts.Responses.Children;

namespace Maktab.Domain.Extensions
{
     public static class ChildExtensions
     {
          public static int GetAge(this ChildResponse childResponse)
          {
               int age = 0;

               // Get today's date
               DateTime today = DateTime.Today;

               // Calculate the age in years
               age = today.Year - childResponse.DateOfBirth.Year;

               // Adjust age if the birthday hasn't occurred yet this year
               if (childResponse.DateOfBirth > today.AddYears(-age))
               {
                    age--;
               }

               return age;
          }

     }
}
