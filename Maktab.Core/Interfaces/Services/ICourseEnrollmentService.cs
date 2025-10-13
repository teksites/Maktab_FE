using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;

namespace Maktab.Core.Interfaces.Services
{
     public interface ICourseEnrollmentService : IDomainService
     {
          Task<StudentCourseEnrollmentResponse> AddCourseEnrollmentAsync(AddStudentCourseEnrollment studentCourseEnrollment);
          Task<IEnumerable<StudentCourseEnrollmentResponse>> AddCourseEnrollmentsAsync(IEnumerable<AddStudentCourseEnrollment> studentCourseEnrollments);
          Task<CourseEnrollmentGroupResponse> GetCourseEnrollmentByIdAsync(Guid enrollmentId);
          Task<IEnumerable<StudentCourseEnrollmentResponse>> GetCourseEnrollmentsByFamilyIdAsync(Guid familyId);
          //Task<StudentCourseTransactionResponse> GetCoursePaymentDetailsForFamily(Guid familyId);
          Task<bool> RemoveCourseEnrollmentByIdAsync(Guid enrollmentId);
          Task<CourseEnrollmentGroupResponse> UpdateCourseEnrollmentByIdAsync(Guid enrollmentId, StudentCourseEnrollmentResponse enrollmentResponse);
     }
}
