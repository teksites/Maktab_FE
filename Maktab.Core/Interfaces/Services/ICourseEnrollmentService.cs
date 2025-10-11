using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;
using MaktabDataContracts.Responses.Transactions;

namespace Maktab.Core.Interfaces.Services
{
     public interface ICourseEnrollmentService : IDomainService
     {
          Task<StudentCourseEnrollmentResponse> AddCourseEnrollmentAsync(Guid familyId, AddStudentCourseEnrollment studentCourseEnrollment);
          Task<IEnumerable<StudentCourseEnrollmentResponse>> AddCourseEnrollmentsAsync(Guid familyId, IEnumerable<AddStudentCourseEnrollment> studentCourseEnrollments);
          Task<IEnumerable<StudentCourseEnrollmentResponse>> GetCourseEnrollmentsByFamilyIdAsync(Guid familyId);
          Task<StudentCourseTransactionResponse> GetCoursePaymentDetailsForFamily(Guid familyId);
          Task<bool> RemoveCourseEnrollmentAsync(Guid familyId, Guid enrollmentId);
     }
}
