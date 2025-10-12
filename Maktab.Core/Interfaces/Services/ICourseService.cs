using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;

namespace Maktab.Core.Interfaces.Services
{
     public interface ICourseService : IDomainService
     {

          Task<CourseResponseDetailed> GetCourseByIdAsync(Guid courseId);

          Task<CourseResponseDetailed> AddCourseAsync(AddCourse course);
          Task<bool> RemoveCourseAsync(Guid courseId);
          Task<bool> DeactivateCourseAsync(Guid courseId);

          Task<IEnumerable<CourseResponseDetailed>> GetCoursesAsync(DateTime? offeredFromDate = null, DateTime? offeredToDate = null, bool isActive = true, IEnumerable<Guid> instituteIds = null, IEnumerable<string> achedemicGroups = null);
          
          Task<IEnumerable<CourseResponseDetailed>> GetCoursesByInstitutionIdAsync(DateTime offeredFromDate, DateTime offeredToDate, Guid institutionId);
          Task<IEnumerable<CourseResponseDetailed>> GetAllCoursesAsync(DateTime offeredFromDate, DateTime offeredToDate);
          Task<CourseResponseDetailed> UpdateCourseAsync(Guid courseId, CourseResponseDetailed course);
          Task<IEnumerable<CourseEnrollmentGroupResponse>> GetCourseGroupsByCourseIdAsync(Guid courseId);
          Task<CourseEnrollmentGroupResponse> GetCourseGroupsByIdAsync(Guid courseGroupId);
          Task<CourseEnrollmentGroupResponse> AddCourseGroupAsync(AddCourseEnrollmentGroup addInstitute);
          Task<CourseEnrollmentGroupResponse> UpdateCourseGroupAsync(Guid courseGroupId, CourseEnrollmentGroupResponse courseGroup);
          Task<bool> RemoveCourseGroupAsync(Guid courseGroupId);
          Task<IEnumerable<CourseResponseDetailed>> GetCurrentActiveCoursesAsync();
          Task<IEnumerable<CourseResponseDetailed>> GetCoursesByIdsAsync(IEnumerable<Guid> instituteIds);
     }
}
