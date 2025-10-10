using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;
using System.Text;

namespace Maktab.Domain.Services
{
     public class CourseService : BaseService, ICourseService
     {
          private const string getCoursesById = @"/api/courses/{0}";
          private const string getCourses = @"/api/courses";
          private const string addCoursesUrl = @"/api/courses";
          private const string updateCourseById = @"/api/courses/{0}";
          private const string removeCourseById = @"/api/courses/{0}";

          private const string getCourseGroupByCourseId = @"/api/courses/{0}/groups";
          private const string getCourseGroupById = @"/api/courses/groups/{0}";
          private const string addCourseGroupUrl = @"/api/courses/groups/{0}";
          private const string updateCourseGroupById = @"/api/courses/groups/{0}";
          private const string removeCourseGroupById = @"/api/courses/groups/{0}";

          private const string institureIdString = "InstituteIds={0}";
          private const string offeredFromDateString = "OfferedFromDate={0}";
          private const string offeredToDateString = "OfferedToDate={0}";
          private const string isActiveString = "IsActive={0}";
          private const string acedemicGroupString = "AcedemicGroups={0}";

          public CourseService(IHttpService httpService, ILocalStorageService localStorageService)
          : base(httpService, localStorageService)
          {
          }

          public async Task<IEnumerable<CourseResponseDetailed>> GetCoursesAsync(DateTime offeredFromDate, DateTime offeredToDate, bool isActive = true, IEnumerable<Guid> instituteIds = null, IEnumerable<string> achedemicGroups = null)
          {
               var sb = new StringBuilder(getCourses);
               sb.Append('?')
               //.Append(string.Format(offeredFromDateString, offeredFromDate));
               //sb.Append('&').Append(string.Format(offeredToDateString, offeredFromDate));
               //sb.Append('&')
               .Append(string.Format(isActiveString, isActive));

               if (instituteIds != null)
               {
                    foreach (var institute in instituteIds)
                    {
                         sb.Append('&').Append(string.Format(institureIdString, institute));
                    }
               }

               if (achedemicGroups != null)
               {
                    foreach (var group in achedemicGroups)
                    {
                         sb.Append('&').Append(string.Format(acedemicGroupString, group));
                    }
               }

               var result = await _httpService.Get<IEnumerable<CourseResponseDetailed>>(sb.ToString());
               return result;
          }

          public async Task<IEnumerable<CourseResponseDetailed>> GetCoursesByInstitutionIdAsync(DateTime offeredFromDate, DateTime offeredToDate, Guid institutionId)
          {
               return await this.GetCoursesAsync(offeredFromDate, offeredToDate, true, [institutionId]);
          }

          public async Task<CourseResponseDetailed> GetCourseByIdAsync(Guid courseId)
          {
               var formatedUrl = string.Format(getCoursesById, courseId);
               var result = await _httpService.Get<CourseResponseDetailed>(formatedUrl);
               return result;
          }

          public async Task<IEnumerable<CourseResponseDetailed>> GetAllCoursesAsync(DateTime offeredFromDate, DateTime offeredToDate)
          {
               return await this.GetCoursesAsync(offeredFromDate, offeredToDate, true);
          }

          public async Task<IEnumerable<CourseResponseDetailed>> GetCurrentActiveCoursesAsync()
          {
               return await this.GetCoursesAsync(DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow.AddMonths(4), true);
          }

          public async Task<CourseResponseDetailed> AddCourseAsync(AddCourse addCourse)
          {
               var result = await _httpService.Post<CourseResponseDetailed>(addCoursesUrl, addCourse);
               return result;
          }

          public async Task<CourseResponseDetailed> UpdateCourseAsync(Guid courseId, CourseResponseDetailed course)
          {
               var formatedUrl = string.Format(updateCourseById, courseId);
               var result = await _httpService.Put<CourseResponseDetailed>(formatedUrl, course);
               return result;
          }

          public async Task<bool> RemoveCourseAsync(Guid courseId)
          {
               var formatedUrl = string.Format(removeCourseById, courseId);
               var result = await _httpService.Delete<bool>(formatedUrl);
               return result;
          }

          public Task<bool> DeactivateCourseAsync(Guid instituteId)
          {
               throw new NotImplementedException();
          }


          public async Task<IEnumerable<CourseEnrollmentGroupResponse>> GetCourseGroupsByCourseIdAsync(Guid courseId)
          {
               var formatedUrl = string.Format(getCourseGroupByCourseId, courseId);
               var result = await _httpService.Get<IEnumerable<CourseEnrollmentGroupResponse>>(formatedUrl);
               return result;
          }

          public async Task<CourseEnrollmentGroupResponse> GetCourseGroupsByIdAsync(Guid courseGroupId)
          {
               var formatedUrl = string.Format(getCourseGroupById, courseGroupId);
               var result = await _httpService.Get<CourseEnrollmentGroupResponse>(formatedUrl);
               return result;
          }

          public async Task<CourseEnrollmentGroupResponse> AddCourseGroupAsync(AddCourseEnrollmentGroup addInstitute)
          {
               var result = await _httpService.Post<CourseEnrollmentGroupResponse>(addCourseGroupUrl, addInstitute);
               return result;
          }

          public async Task<CourseEnrollmentGroupResponse> UpdateCourseGroupAsync(Guid courseGroupId, CourseEnrollmentGroupResponse courseGroup)
          {
               var formatedUrl = string.Format(updateCourseGroupById, courseGroupId);
               var result = await _httpService.Put<CourseEnrollmentGroupResponse>(formatedUrl, courseGroup);
               return result;
          }

          public async Task<bool> RemoveCourseGroupAsync(Guid courseGroupId)
          {
               var formatedUrl = string.Format(removeCourseGroupById, courseGroupId);
               var result = await _httpService.Delete<bool>(formatedUrl);
               return result;
          }

     }
}
