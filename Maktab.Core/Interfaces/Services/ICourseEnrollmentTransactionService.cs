using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Transactions;

namespace Maktab.Core.Interfaces.Services
{
     public interface ICourseEnrollmentTransactionService : IDomainService
     {
          Task<StudentCourseTransactionResponse> AddCourseEnrollmentTranasctionAsync(AddStudentCourseTransaction addCourseTransaction);
          Task<StudentCourseTransactionResponse> GetCourseEnrollmentTranasctionByCourseIdAsync(Guid courseId);
          Task<StudentCourseTransactionResponse> GetCourseEnrollmentTranasctionByFamilyAndInstituteIdAsync(Guid familyId, Guid instituteId);
          Task<StudentCourseTransactionResponse> GetCourseEnrollmentTranasctionByIdAsync(Guid enrollmentId);
          Task<bool> RemoveCourseEnrollmentTranasctionByIdAsync(Guid enrollmentId);
          Task<StudentCourseTransactionResponse> UpdateCourseEnrollmentTranasctionByIdAsync(Guid enrollmentId, StudentCourseTransactionResponse enrollmentResponse);
     }
}
