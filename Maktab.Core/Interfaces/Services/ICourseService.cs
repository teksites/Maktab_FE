using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;

namespace Maktab.Core.Interfaces.Services
{
     public interface ICourseService : IDomainService
     {
          Task<IEnumerable<CourseResponse>> GetAllCoursesAsync();

          Task<IEnumerable<CourseResponse>> GetCoursesByInstitutionIdAsync(Guid institutionId);
          Task<CourseResponse> GetCourseByIdAsync(Guid courseId);

          Task<CourseResponse> AddInstitutionAsync(AddCourse addInstitute);
          Task<bool> IsCourseExistAsync(Guid instituteId, string courseName);
          Task<bool> RemoveCourseAsync(Guid instituteId);

          Task<bool> DeactivateCourseAsync(Guid instituteId);
     }
}
