using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;
using MaktabDataContracts.Responses.Transactions;

namespace Maktab.Core.Interfaces.Services
{
     public interface IPaymentService : IDomainService
     {
          Task<StudentCourseTransactionResponse> AddtPaymentAsync(AddStudentCourseTransaction addCourseTransaction);
          Task<IEnumerable<CoursePaymentResponse>> GetPaymentByCourseIdAsync(Guid courseId);
          Task<CoursePaymentResponse> GetPaymentByIdAsync(Guid paymentId);
          Task<IEnumerable<CoursePaymentResponse>> GetPaymentByStudentTransactionsIdIdAsync(Guid studentTransactionsId);
          Task<bool> RemovePaymentByIdAsync(Guid paymentId);
          Task<CoursePaymentResponse> UpdatePaymentByIdAsync(Guid paymentId, CoursePaymentResponse coursePaymentUpdate);
     }
}
