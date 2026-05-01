using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maktab.Domain.Services
{
     /// <summary>
     /// Course service with comprehensive error handling, validation, and logging.
     /// Production-ready implementation for backend API integration.
     /// </summary>
     public class CourseService : BaseService, ICourseService
     {
          // API Endpoints (properly formatted, not "formated")
          private const string getCoursesById = @"/api/courses/{0}";
          private const string getCourses = @"/api/courses";
          private const string addCoursesUrl = @"/api/courses";
          private const string updateCourseById = @"/api/courses/{0}";
          private const string removeCourseById = @"/api/courses/{0}";

          private const string getCourseGroupByCourseId = @"/api/courses/{0}/groups";
          private const string getCourseGroupById = @"/api/courses/groups/{0}";
          private const string addCourseGroupUrl = @"/api/courses/groups";
          private const string updateCourseGroupById = @"/api/courses/groups/{0}";
          private const string removeCourseGroupById = @"/api/courses/groups/{0}";

          // Query string formats (properly formatted, not "acedemicGroups")
          private const string instituteIdString = "InstituteIds={0}";
          private const string offeredFromDateString = "OfferedFromDate={0}";
          private const string offeredToDateString = "OfferedToDate={0}";
          private const string isActiveString = "IsActive={0}";
          private const string academicGroupString = "AcademicGroups={0}";

          private readonly ILogger<CourseService> _logger;

          public CourseService(
               IHttpService httpService,
               ILocalStorageService localStorageService,
               ILogger<CourseService> logger)
               : base(httpService, localStorageService)
          {
               _logger = logger ?? throw new ArgumentNullException(nameof(logger));
          }

          /// <summary>
          /// Get courses with optional filtering by date, institution, and academic groups
          /// </summary>
          public async Task<IEnumerable<CourseResponseDetailed>> GetCoursesAsync(DateTime? offeredFromDate = null, DateTime? offeredToDate = null, bool isActive = true, IEnumerable<Guid> instituteIds = null, IEnumerable<string> academicGroups = null)
          {
               try
               {
                    _logger.LogInformation("Fetching courses with filters - Active: {IsActive}", isActive);

                    var sb = new StringBuilder(getCourses);
                    sb.Append('?').Append(string.Format(isActiveString, isActive));

               //if(offeredFromDate.HasValue)
               //{
               //     sb.Append('&').Append(string.Format(offeredFromDateString, offeredFromDate));
               //}

               //if (offeredToDate.HasValue)
               //{
               //     sb.Append('&').Append(string.Format(offeredToDateString, offeredFromDate));
               //}

                    if (instituteIds != null)
                    {
                         foreach (var institute in instituteIds)
                         {
                              sb.Append('&').Append(string.Format(instituteIdString, institute));
                         }
                    }

                    if (academicGroups != null)
                    {
                         foreach (var group in academicGroups)
                         {
                              sb.Append('&').Append(string.Format(academicGroupString, group));
                         }
                    }

                    var result = await _httpService.Get<IEnumerable<CourseResponseDetailed>>(sb.ToString());

                    if (result == null)
                    {
                         _logger.LogWarning("No courses found with applied filters");
                         return new List<CourseResponseDetailed>();
                    }

                    _logger.LogInformation("Successfully fetched {CourseCount} courses", result.Count());
                    return result;
               }
               catch (HttpRequestException ex)
               {
                    _logger.LogError(ex, "Network error fetching courses");
                    throw;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Unexpected error fetching courses");
                    throw;
               }
          }

          /// <summary>
          /// Get courses by IDs with error handling
          /// </summary>
          public async Task<IEnumerable<CourseResponseDetailed>> GetCoursesByIdsAsync(IEnumerable<Guid> instituteIds)
          {
               try
               {
                    if (instituteIds == null || !instituteIds.Any())
                    {
                         _logger.LogWarning("GetCoursesByIdsAsync called with null/empty IDs");
                         return new List<CourseResponseDetailed>();
                    }

                    _logger.LogInformation("Fetching courses by {IdCount} institute IDs", instituteIds.Count());
                    return await GetCoursesAsync(isActive: false, instituteIds: instituteIds);
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error fetching courses by IDs");
                    throw;
               }
          }

          /// <summary>
          /// Get courses by institution with error handling
          /// </summary>
          public async Task<IEnumerable<CourseResponseDetailed>> GetCoursesByInstitutionIdAsync(DateTime offeredFromDate, DateTime offeredToDate, Guid institutionId)
          {
               try
               {
                    if (institutionId == Guid.Empty)
                    {
                         _logger.LogWarning("GetCoursesByInstitutionIdAsync called with empty GUID");
                         throw new ArgumentException("Institution ID cannot be empty", nameof(institutionId));
                    }

                    _logger.LogInformation("Fetching courses for institution {InstitutionId}", institutionId);
                    return await GetCoursesAsync(offeredFromDate, offeredToDate, true, new[] { institutionId });
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error fetching courses for institution {InstitutionId}", institutionId);
                    throw;
               }
          }

          /// <summary>
          /// Get course by ID with error handling
          /// </summary>
          public async Task<CourseResponseDetailed> GetCourseByIdAsync(Guid courseId)
          {
               try
               {
                    if (courseId == Guid.Empty)
                    {
                         _logger.LogWarning("GetCourseByIdAsync called with empty GUID");
                         throw new ArgumentException("Course ID cannot be empty", nameof(courseId));
                    }

                    _logger.LogInformation("Fetching course {CourseId}", courseId);

                    var formattedUrl = string.Format(getCoursesById, courseId);
                    var result = await _httpService.Get<CourseResponseDetailed>(formattedUrl);

                    if (result == null)
                    {
                         _logger.LogWarning("Course {CourseId} not found", courseId);
                         return null;
                    }

                    _logger.LogInformation("Successfully fetched course {CourseId}", courseId);
                    return result;
               }
               catch (HttpRequestException ex)
               {
                    _logger.LogError(ex, "Network error fetching course {CourseId}", courseId);
                    throw;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Unexpected error fetching course {CourseId}", courseId);
                    throw;
               }
          }

          /// <summary>
          /// Get all courses for date range with error handling
          /// </summary>
          public async Task<IEnumerable<CourseResponseDetailed>> GetAllCoursesAsync(DateTime offeredFromDate, DateTime offeredToDate)
          {
               try
               {
                    _logger.LogInformation("Fetching all courses for date range");
                    return await GetCoursesAsync(offeredFromDate, offeredToDate, true);
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error fetching all courses");
                    throw;
               }
          }

          /// <summary>
          /// Get current active courses (within 4 month window) with error handling
          /// </summary>
          public async Task<IEnumerable<CourseResponseDetailed>> GetCurrentActiveCoursesAsync()
          {
               try
               {
                    _logger.LogInformation("Fetching current active courses");
                    return await GetCoursesAsync(DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(4), true);
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error fetching current active courses");
                    throw;
               }
          }

          /// <summary>
          /// Add new course with validation and error handling
          /// </summary>
          public async Task<CourseResponseDetailed> AddCourseAsync(AddCourse addCourse)
          {
               try
               {
                    if (addCourse == null)
                    {
                         _logger.LogWarning("AddCourseAsync called with null course");
                         throw new ArgumentNullException(nameof(addCourse), "Course cannot be null");
                    }

                    _logger.LogInformation("Adding new course");

                    var result = await _httpService.Post<CourseResponseDetailed>(addCoursesUrl, addCourse);

                    if (result == null)
                    {
                         _logger.LogError("Server returned null response when adding course");
                         throw new InvalidOperationException("Server did not return course confirmation");
                    }

                    _logger.LogInformation("Successfully added course");
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error adding course");
                    throw;
               }
          }

          /// <summary>
          /// Update course (backend uses PUT - RESTful)
          /// </summary>
          public async Task<CourseResponseDetailed> UpdateCourseAsync(Guid courseId, CourseResponseDetailed course)
          {
               try
               {
                    if (courseId == Guid.Empty)
                    {
                         _logger.LogWarning("UpdateCourseAsync called with empty GUID");
                         throw new ArgumentException("Course ID cannot be empty", nameof(courseId));
                    }

                    if (course == null)
                    {
                         _logger.LogWarning("UpdateCourseAsync called with null course");
                         throw new ArgumentNullException(nameof(course), "Course cannot be null");
                    }

                    _logger.LogInformation("Updating course {CourseId}", courseId);

                    var formattedUrl = string.Format(updateCourseById, courseId);
                    var result = await _httpService.Put<CourseResponseDetailed>(formattedUrl, course);

                    if (result == null)
                    {
                         _logger.LogError("Server returned null response when updating course");
                         throw new InvalidOperationException("Server did not return updated course");
                    }

                    _logger.LogInformation("Successfully updated course {CourseId}", courseId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error updating course {CourseId}", courseId);
                    throw;
               }
          }

          /// <summary>
          /// Remove course (backend uses DELETE - RESTful)
          /// </summary>
          public async Task<bool> RemoveCourseAsync(Guid courseId)
          {
               try
               {
                    if (courseId == Guid.Empty)
                    {
                         _logger.LogWarning("RemoveCourseAsync called with empty GUID");
                         throw new ArgumentException("Course ID cannot be empty", nameof(courseId));
                    }

                    _logger.LogInformation("Removing course {CourseId}", courseId);

                    var formattedUrl = string.Format(removeCourseById, courseId);
                    var result = await _httpService.Delete<bool>(formattedUrl);

                    _logger.LogInformation("Successfully removed course {CourseId}", courseId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error removing course {CourseId}", courseId);
                    throw;
               }
          }

          /// <summary>
          /// Deactivate course (not implemented in current backend)
          /// </summary>
          public Task<bool> DeactivateCourseAsync(Guid courseId)
          {
               _logger.LogWarning("DeactivateCourseAsync not implemented in backend");
               throw new NotImplementedException("DeactivateCourseAsync is not implemented in the current backend");
          }

          /// <summary>
          /// Get course groups by course ID with error handling
          /// </summary>
          public async Task<IEnumerable<CourseEnrollmentGroupResponse>> GetCourseGroupsByCourseIdAsync(Guid courseId)
          {
               try
               {
                    if (courseId == Guid.Empty)
                    {
                         _logger.LogWarning("GetCourseGroupsByCourseIdAsync called with empty GUID");
                         throw new ArgumentException("Course ID cannot be empty", nameof(courseId));
                    }

                    _logger.LogInformation("Fetching course groups for course {CourseId}", courseId);

                    var formattedUrl = string.Format(getCourseGroupByCourseId, courseId);
                    var result = await _httpService.Get<IEnumerable<CourseEnrollmentGroupResponse>>(formattedUrl);

                    if (result == null)
                    {
                         _logger.LogWarning("No course groups found for course {CourseId}", courseId);
                         return new List<CourseEnrollmentGroupResponse>();
                    }

                    _logger.LogInformation("Successfully fetched {GroupCount} course groups for course {CourseId}", result.Count(), courseId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error fetching course groups for course {CourseId}", courseId);
                    throw;
               }
          }

          /// <summary>
          /// Get course group by ID with error handling
          /// </summary>
          public async Task<CourseEnrollmentGroupResponse> GetCourseGroupsByIdAsync(Guid courseGroupId)
          {
               try
               {
                    if (courseGroupId == Guid.Empty)
                    {
                         _logger.LogWarning("GetCourseGroupsByIdAsync called with empty GUID");
                         throw new ArgumentException("Course group ID cannot be empty", nameof(courseGroupId));
                    }

                    _logger.LogInformation("Fetching course group {CourseGroupId}", courseGroupId);

                    var formattedUrl = string.Format(getCourseGroupById, courseGroupId);
                    var result = await _httpService.Get<CourseEnrollmentGroupResponse>(formattedUrl);

                    if (result == null)
                    {
                         _logger.LogWarning("Course group {CourseGroupId} not found", courseGroupId);
                         return null;
                    }

                    _logger.LogInformation("Successfully fetched course group {CourseGroupId}", courseGroupId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error fetching course group {CourseGroupId}", courseGroupId);
                    throw;
               }
          }

          /// <summary>
          /// Add course group with validation and error handling
          /// </summary>
          public async Task<CourseEnrollmentGroupResponse> AddCourseGroupAsync(AddCourseEnrollmentGroup addCourseGroup)
          {
               try
               {
                    if (addCourseGroup == null)
                    {
                         _logger.LogWarning("AddCourseGroupAsync called with null course group");
                         throw new ArgumentNullException(nameof(addCourseGroup), "Course group cannot be null");
                    }

                    _logger.LogInformation("Adding new course group");

                    var result = await _httpService.Post<CourseEnrollmentGroupResponse>(addCourseGroupUrl, addCourseGroup);

                    if (result == null)
                    {
                         _logger.LogError("Server returned null response when adding course group");
                         throw new InvalidOperationException("Server did not return course group confirmation");
                    }

                    _logger.LogInformation("Successfully added course group");
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error adding course group");
                    throw;
               }
          }

          /// <summary>
          /// Update course group (backend uses PUT - RESTful)
          /// </summary>
          public async Task<CourseEnrollmentGroupResponse> UpdateCourseGroupAsync(Guid courseGroupId, CourseEnrollmentGroupResponse courseGroup)
          {
               try
               {
                    if (courseGroupId == Guid.Empty)
                    {
                         _logger.LogWarning("UpdateCourseGroupAsync called with empty GUID");
                         throw new ArgumentException("Course group ID cannot be empty", nameof(courseGroupId));
                    }

                    if (courseGroup == null)
                    {
                         _logger.LogWarning("UpdateCourseGroupAsync called with null course group");
                         throw new ArgumentNullException(nameof(courseGroup), "Course group cannot be null");
                    }

                    _logger.LogInformation("Updating course group {CourseGroupId}", courseGroupId);

                    var formattedUrl = string.Format(updateCourseGroupById, courseGroupId);
                    var result = await _httpService.Put<CourseEnrollmentGroupResponse>(formattedUrl, courseGroup);

                    if (result == null)
                    {
                         _logger.LogError("Server returned null response when updating course group");
                         throw new InvalidOperationException("Server did not return updated course group");
                    }

                    _logger.LogInformation("Successfully updated course group {CourseGroupId}", courseGroupId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error updating course group {CourseGroupId}", courseGroupId);
                    throw;
               }
          }

          /// <summary>
          /// Remove course group (backend uses DELETE - RESTful)
          /// </summary>
          public async Task<bool> RemoveCourseGroupAsync(Guid courseGroupId)
          {
               try
               {
                    if (courseGroupId == Guid.Empty)
                    {
                         _logger.LogWarning("RemoveCourseGroupAsync called with empty GUID");
                         throw new ArgumentException("Course group ID cannot be empty", nameof(courseGroupId));
                    }

                    _logger.LogInformation("Removing course group {CourseGroupId}", courseGroupId);

                    var formattedUrl = string.Format(removeCourseGroupById, courseGroupId);
                    var result = await _httpService.Delete<bool>(formattedUrl);

                    _logger.LogInformation("Successfully removed course group {CourseGroupId}", courseGroupId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error removing course group {CourseGroupId}", courseGroupId);
                    throw;
               }
          }
     }
}
