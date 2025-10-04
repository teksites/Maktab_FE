using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Enums;
using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;

namespace Maktab.Domain.Services
{
     public class CourseGroupService : ICourseGroupService
     {
          public Task<CourseResponse> AddCourseEnrollmentGroupAsync(Guid courseId, AddCourseEnrollmentGroup addGroup)
          {
               throw new NotImplementedException();
          }

          public Task<CourseEnrollmentGroupResponse> GetCourseEnrollmentGroupByIdAsync(Guid courseEnrollmentGroupId)
          {
               throw new NotImplementedException();
          }

          public async Task<IEnumerable<CourseEnrollmentGroupResponse>> GetEnrollmentGroupsByCourseIdAsync(Guid courseId)
          {
               var enrollmentGroups = new List<CourseEnrollmentGroupResponse>()
               {
                    new CourseEnrollmentGroupResponse()
                    {
                         GroupId =  Guid.NewGuid(),
                         CourseId = courseId,
                         CreatedAt = DateTime.Now,
                         Description = "Week 1",
                         GroupTitle = "December 22 - December 26",
                         Fee = 100,
                         InstituteId = Guid.Empty,
                         AcedemicGroups = new List<string>{ AcedemicGroupType.Kindergarten.ToString() },
                         MaxStudents = 50,
                         IsActive = true,
                    },
                    new CourseEnrollmentGroupResponse()
                    {
                         GroupId =  Guid.NewGuid(),
                         CourseId = courseId,
                         CreatedAt = DateTime.Now,
                         Description = "Week 2",
                         GroupTitle = "December 29 - January 2",
                         Fee = 100,
                         InstituteId = Guid.Empty,
                         AcedemicGroups = new List<string>{ AcedemicGroupType.PreK.ToString() },
                         MaxStudents = 50,
                         IsActive = true,
                    },
               };

               return await Task.FromResult(enrollmentGroups);
          }
     }
}
