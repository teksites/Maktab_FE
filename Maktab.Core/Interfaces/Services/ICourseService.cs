using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;

namespace Maktab.Core.Interfaces.Services
{
     public interface ICourseService : IDomainService
     {
          Task<IEnumerable<CourseResponseDetailed>> GetAllCoursesAsync();

          Task<IEnumerable<CourseResponseDetailed>> GetCoursesByInstitutionIdAsync(Guid institutionId);
          Task<CourseResponseDetailed> GetCourseByIdAsync(Guid courseId);

          Task<CourseResponseDetailed> AddCourseAsync(AddCourse addInstitute);
          Task<bool> IsCourseExistAsync(Guid instituteId, string courseName);
          Task<bool> RemoveCourseAsync(Guid instituteId);

          Task<bool> DeactivateCourseAsync(Guid instituteId);
     }
}
