using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;

namespace Maktab.Core.Interfaces.Services
{
     /// <summary>
     /// Service interface for course operations.
     /// All methods have proper error handling, validation, and logging.
     /// </summary>
     public interface ICourseService : IDomainService
     {
          /// <summary>
          /// Get course by ID with error handling
          /// </summary>
          Task<CourseResponseDetailed> GetCourseByIdAsync(Guid courseId);

          /// <summary>
          /// Add new course with validation
          /// </summary>
          Task<CourseResponseDetailed> AddCourseAsync(AddCourse course);

          /// <summary>
          /// Remove course (backend uses DELETE)
          /// </summary>
          Task<bool> RemoveCourseAsync(Guid courseId);

          /// <summary>
          /// Deactivate course (not implemented in backend)
          /// </summary>
          Task<bool> DeactivateCourseAsync(Guid courseId);

          /// <summary>
          /// Get courses with optional filtering
          /// </summary>
          Task<IEnumerable<CourseResponseDetailed>> GetCoursesAsync(DateTime? offeredFromDate = null, DateTime? offeredToDate = null, bool isActive = true, IEnumerable<Guid> instituteIds = null, IEnumerable<string> academicGroups = null);

          /// <summary>
          /// Get courses by institution ID
          /// </summary>
          Task<IEnumerable<CourseResponseDetailed>> GetCoursesByInstitutionIdAsync(DateTime offeredFromDate, DateTime offeredToDate, Guid institutionId);

          /// <summary>
          /// Get all courses for date range
          /// </summary>
          Task<IEnumerable<CourseResponseDetailed>> GetAllCoursesAsync(DateTime offeredFromDate, DateTime offeredToDate);

          /// <summary>
          /// Update course (backend uses PUT)
          /// </summary>
          Task<CourseResponseDetailed> UpdateCourseAsync(Guid courseId, CourseResponseDetailed course);

          /// <summary>
          /// Get course groups by course ID
          /// </summary>
          Task<IEnumerable<CourseEnrollmentGroupResponse>> GetCourseGroupsByCourseIdAsync(Guid courseId);

          /// <summary>
          /// Get course group by ID
          /// </summary>
          Task<CourseEnrollmentGroupResponse> GetCourseGroupsByIdAsync(Guid courseGroupId);

          /// <summary>
          /// Add new course group
          /// </summary>
          Task<CourseEnrollmentGroupResponse> AddCourseGroupAsync(AddCourseEnrollmentGroup addCourseGroup);

          /// <summary>
          /// Update course group (backend uses PUT)
          /// </summary>
          Task<CourseEnrollmentGroupResponse> UpdateCourseGroupAsync(Guid courseGroupId, CourseEnrollmentGroupResponse courseGroup);

          /// <summary>
          /// Remove course group (backend uses DELETE)
          /// </summary>
          Task<bool> RemoveCourseGroupAsync(Guid courseGroupId);

          /// <summary>
          /// Get current active courses (within 4 month window)
          /// </summary>
          Task<IEnumerable<CourseResponseDetailed>> GetCurrentActiveCoursesAsync();

          /// <summary>
          /// Get courses by IDs
          /// </summary>
          Task<IEnumerable<CourseResponseDetailed>> GetCoursesByIdsAsync(IEnumerable<Guid> instituteIds);
     }
}
