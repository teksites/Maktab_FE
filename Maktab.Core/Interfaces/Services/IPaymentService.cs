using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;
using MaktabDataContracts.Responses.Transactions;

namespace Maktab.Core.Interfaces.Services
{
     /// <summary>
     /// Service interface for payment operations.
     /// All methods have proper error handling, validation, and logging.
     /// </summary>
     public interface IPaymentService : IDomainService
     {
          /// <summary>
          /// Add new payment with validation
          /// </summary>
          Task<StudentCourseTransactionResponse> AddtoPaymentAsync(AddStudentCourseTransaction addCourseTransaction);

          /// <summary>
          /// Get payments by course ID with error handling
          /// </summary>
          Task<IEnumerable<CoursePaymentResponse>> GetPaymentByCourseIdAsync(Guid courseId);

          /// <summary>
          /// Get payment by ID with error handling
          /// </summary>
          Task<CoursePaymentResponse> GetPaymentByIdAsync(Guid paymentId);

          /// <summary>
          /// Get payments by student transaction ID with error handling
          /// </summary>
          Task<IEnumerable<CoursePaymentResponse>> GetPaymentByStudentTransactionsIdIdAsync(Guid studentTransactionsId);

          /// <summary>
          /// Remove payment by ID (backend uses DELETE)
          /// </summary>
          Task<bool> RemovePaymentByIdAsync(Guid paymentId);

          /// <summary>
          /// Update payment (backend uses PUT)
          /// </summary>
          Task<CoursePaymentResponse> UpdatePaymentByIdAsync(Guid paymentId, CoursePaymentResponse coursePaymentUpdate);
     }
}

