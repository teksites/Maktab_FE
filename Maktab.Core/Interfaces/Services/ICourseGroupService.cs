using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;

namespace Maktab.Core.Interfaces.Services
{
     public interface ICourseGroupService : IDomainService
     {
          //Task<IEnumerable<CourseEnrollmentGroupResponse>> GetAllCoursesAsync();

          Task<IEnumerable<CourseEnrollmentGroupResponse>> GetEnrollmentGroupsByCourseIdAsync(Guid courseId);
          Task<CourseEnrollmentGroupResponse> GetCourseEnrollmentGroupByIdAsync(Guid courseEnrollmentGroupId);

          Task<CourseResponse> AddCourseEnrollmentGroupAsync(Guid courseId, AddCouseEnrollmentGroup addGroup);
          //Task<bool> IsCourseEnrollmentGroupExistAsync(Guid instituteId, string courseGroupName);
          //Task<bool> RemoveCourseEnrollmentGroupAsync(Guid courseEnrollmentGroupId);

          //Task<bool> DeactivateCourseEnrollmentGroupAsync(Guid courseEnrollmentGroupId);
     }
}
