using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Transactions;

namespace Maktab.Domain.Services
{
     public class CourseEnrollmentTransaction : BaseService, ICourseEnrollmentTransactionService
     {
          private const string getCourseEnrollmentTranasctionByFamilyAndInstitureIdUrl = @"/api/student-course-transactions/family/{0}/institute/{1}";
          private const string getCourseEnrollmentTranasctionByFamilyAndCourseIdUrl = @"/api/student-course-transactions/family/{0}/course/{1}";
          private const string getCourseEnrollmentTranasctionByCourseIdUrl = @"/api/student-course-transactions/course/{0}";

          private const string getCourseEnrollmentTranasctionByIdUrl = @"/api/student-course-transactions/{0}";
          private const string addCourseEnrollmentTranasctionUrl = @"/api/student-course-transactions";
          private const string updateCourseEnrollmentTranasctionByIdUrl = @"/api/student-course-transactions/{0}";
          private const string removeCourseEnrollmentTranasctionByIdUrl = @"/api/student-course-transactions/{0}";


          public CourseEnrollmentTransaction(IHttpService httpService, ILocalStorageService localStorageService) 
          : base(httpService, localStorageService)
          {
          }

          public async Task<StudentCourseTransactionResponse> AddCourseEnrollmentTranasctionAsync(AddStudentCourseTransaction addCourseTransaction)
          {
               var result = await _httpService.Post<StudentCourseTransactionResponse>(addCourseEnrollmentTranasctionUrl, addCourseTransaction);
               return result;
          }

          public async Task<bool> RemoveCourseEnrollmentTranasctionByIdAsync(Guid enrollmentId)
          {
               var formatedUrl = string.Format(removeCourseEnrollmentTranasctionByIdUrl, enrollmentId);
               var result = await _httpService.Delete<bool>(formatedUrl);
               return result;
          }

          public async Task<StudentCourseTransactionResponse> UpdateCourseEnrollmentTranasctionByIdAsync(Guid enrollmentId, StudentCourseTransactionResponse enrollmentResponse)
          {
               var formatedUrl = string.Format(updateCourseEnrollmentTranasctionByIdUrl, enrollmentId);
               var result = await _httpService.Put<StudentCourseTransactionResponse>(formatedUrl, enrollmentResponse);
               return result;
          }

          public async Task<StudentCourseTransactionResponse> GetCourseEnrollmentTranasctionByIdAsync(Guid enrollmentId)
          {
               var formatedUrl = string.Format(getCourseEnrollmentTranasctionByIdUrl, enrollmentId);
               var result = await _httpService.Get<StudentCourseTransactionResponse>(formatedUrl);
               return result;
          }

          public async Task<IList<StudentCourseTransactionResponse>> GetCourseEnrollmentTranasctionByFamilyAndInstituteIdAsync(Guid familyId, Guid instituteId)
          {
               var formatedUrl = string.Format(getCourseEnrollmentTranasctionByFamilyAndInstitureIdUrl, familyId, instituteId);
               var result = await _httpService.Get<IList<StudentCourseTransactionResponse>>(formatedUrl);
               return result;
          }

          public async Task<IList<StudentCourseTransactionResponse>> GetCourseEnrollmentTranasctionByFamilyAndCourseIdAsync(Guid familyId, Guid courseId)
          {
               var formatedUrl = string.Format(getCourseEnrollmentTranasctionByFamilyAndCourseIdUrl, familyId, courseId);
               var result = await _httpService.Get<IList<StudentCourseTransactionResponse>>(formatedUrl);
               return result;
          }

          public async Task<StudentCourseTransactionResponse> GetCourseEnrollmentTranasctionByCourseIdAsync(Guid courseId)
          {
               var formatedUrl = string.Format(getCourseEnrollmentTranasctionByCourseIdUrl, courseId);
               var result = await _httpService.Get<StudentCourseTransactionResponse>(formatedUrl);
               return result;
          }
     }
}
