using MaktabDataContracts.Responses.Course;

namespace Maktab.Domain.Extensions
{
     public static class CourseExtensions
     {
          public static double GetProgress(this CourseResponseDetailed course)
          {
               if (course.StartDate > DateTime.Today)
                    return 0;

               var total = (course.EndDate - course.StartDate).TotalDays;
               if (total <= 0) return 100;

               var elapsed = (DateTime.Today - course.StartDate).TotalDays;
               return Math.Clamp((elapsed / total) * 100, 0, 100);
          }
     }
}
