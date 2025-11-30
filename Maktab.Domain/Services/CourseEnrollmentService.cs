using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;

namespace Maktab.Domain.Services
{
     public class CourseEnrollmentService : BaseService, ICourseEnrollmentService
     {
          private const string getCourseEnrollmentByFamilyUrl = @"/api/student-course-enrollments/family/{0}";
          private const string getCourseEnrollmentByIdUrl = @"/api/student-course-enrollments/{0}";
          private const string addCourseEnrollmentUrl = @"/api/student-course-enrollments";
          private const string updateCourseEnrollmentByIdUrl = @"/api/student-course-enrollments/{0}";
          private const string removeCourseEnrollmentByIdUrl = @"/api/student-course-enrollments/{0}?hardDelete=true";


          //private IDictionary<Guid, List<StudentCourseEnrollmentResponse>> courseResponses = new Dictionary<Guid, List<StudentCourseEnrollmentResponse>>();

          public CourseEnrollmentService(IHttpService httpService, ILocalStorageService localStorageService) 
          : base(httpService, localStorageService)
          {
          }

          public async Task<StudentCourseEnrollmentResponse> AddCourseEnrollmentAsync(AddStudentCourseEnrollment studentCourseEnrollment)
          {
               var result = await _httpService.Post<StudentCourseEnrollmentResponse>(addCourseEnrollmentUrl, studentCourseEnrollment);
               return result;
          }

          public async Task<bool> RemoveCourseEnrollmentByIdAsync(Guid enrollmentId)
          {
               var formatedUrl = string.Format(removeCourseEnrollmentByIdUrl, enrollmentId);
               var result = await _httpService.Delete<bool>(formatedUrl);
               return result;
          }

          public async Task<CourseEnrollmentGroupResponse> UpdateCourseEnrollmentByIdAsync(Guid enrollmentId, StudentCourseEnrollmentResponse enrollmentResponse)
          {
               var formatedUrl = string.Format(updateCourseEnrollmentByIdUrl, enrollmentId);
               var result = await _httpService.Put<CourseEnrollmentGroupResponse>(formatedUrl, enrollmentResponse);
               return result;
          }

          public async Task<CourseEnrollmentGroupResponse> GetCourseEnrollmentByIdAsync(Guid enrollmentId)
          {
               var formatedUrl = string.Format(getCourseEnrollmentByIdUrl, enrollmentId);
               var result = await _httpService.Get<CourseEnrollmentGroupResponse>(formatedUrl);
               return result;
          }

          public async Task<IEnumerable<StudentCourseEnrollmentResponse>> GetCourseEnrollmentsByFamilyIdAsync(Guid familyId)
          {
               var formatedUrl = string.Format(getCourseEnrollmentByFamilyUrl, familyId);
               var result = await _httpService.Get<IEnumerable<StudentCourseEnrollmentResponse>>(formatedUrl);
               return result;
          }

          public async Task<IEnumerable<StudentCourseEnrollmentResponse>> AddCourseEnrollmentsAsync(IEnumerable<AddStudentCourseEnrollment> studentCourseEnrollments)
          {
               if(studentCourseEnrollments?.Any() != true)
               {
                    return Enumerable.Empty<StudentCourseEnrollmentResponse>();
               }

               var responses = new List<StudentCourseEnrollmentResponse>();
               foreach (var studentEnrollment in studentCourseEnrollments)
               {
                    var response = await AddCourseEnrollmentAsync(studentEnrollment);
                    if (response != null)
                    {
                         responses.Add(response);
                    }
               }

               return responses;
          }



          //private static StudentCourseEnrollmentResponse CreateResponseFromRequest(Guid familyId, AddStudentCourseEnrollment studentCourseEnrollment)
          //{
          //     return new MaktabDataContracts.Responses.Course.StudentCourseEnrollmentResponse()
          //     {
          //          StudentCourseEnrollmentId = Guid.NewGuid(),
          //          CourseId = studentCourseEnrollment.CourseId,
          //          CourseEnrollmentGroupId = studentCourseEnrollment.CourseEnrollmentGroupId,
          //          ChildId = studentCourseEnrollment.ChildId,
          //          FamilyId = familyId,
          //          CreatedAt = DateTime.Now,
          //          DayCareDays = studentCourseEnrollment.DayCareDays,
          //          WillUseDayCare = studentCourseEnrollment.WillUseDayCare,
          //          IsActive = studentCourseEnrollment.IsActive,
          //     };
          //}

          //public async Task<IEnumerable<StudentCourseEnrollmentResponse>> AddCourseEnrollmentsAsync(Guid familyId, IEnumerable<AddStudentCourseEnrollment> studentCourseEnrollments)
          //{
          //     if (!courseResponses.ContainsKey(familyId))
          //     {
          //          courseResponses[familyId] = new List<StudentCourseEnrollmentResponse>();
          //     }

          //     var enrollments = new List<StudentCourseEnrollmentResponse>();

          //     foreach( var request in studentCourseEnrollments)
          //     {
          //          StudentCourseEnrollmentResponse enrollment = CreateResponseFromRequest(familyId, request);
          //          enrollments.Add(enrollment);
          //     }

          //     courseResponses[familyId].AddRange(enrollments);
          //     return enrollments;
          //}

          //public async Task<bool> RemoveCourseEnrollmentAsync(Guid familyId, Guid enrollmentId)
          //{
          //     if (courseResponses[familyId]?.Any() == true)
          //     {
          //          var enrollment = courseResponses[familyId].Find(x => x.StudentCourseEnrollmentId == enrollmentId);
          //          if (enrollment != null)
          //          {
          //               courseResponses[familyId].Remove(enrollment);
          //               return true;
          //          }
          //     }

          //     return false;
          //}

          //public async Task<StudentCourseTransactionResponse> GetCoursePaymentDetailsForFamily(Guid familyId)
          //{

          //     if (courseResponses.ContainsKey(familyId))
          //     {
          //          var list = courseResponses[familyId];

          //          var transaction = new StudentCourseTransactionResponse()
          //          {
          //               FamilyId = familyId,
          //               CreatedAt = DateTime.UtcNow,
          //               PayableFee = 75 * list.Count,
          //               Enrollments = list,
          //               StudentCourseTransactionId = Guid.NewGuid(),
          //               PaymentCode = $"ABCD-{familyId.ToString()}",
          //               AmountDiscounted = (list.Count> 1)? 10 : 0,
          //               TransactionStatus = MaktabDataContracts.Enums.TransactionStatus.AwaitingPayment
          //          };

          //          transaction.TotalPayable = transaction.PayableFee - transaction.AmountDiscounted;

          //          return transaction;
          //     }

          //     return default;
          //}

          //public async Task<IEnumerable<StudentCourseEnrollmentResponse>> GetCourseEnrollmentsByFamilyIdAsync(Guid familyId)
          //{
          //     if (courseResponses.ContainsKey(familyId) && courseResponses[familyId]?.Any() == true)
          //     {
          //          return courseResponses[familyId];
          //     }

          //     return Enumerable.Empty<StudentCourseEnrollmentResponse>();
          //}
     }
}
