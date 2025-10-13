using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;
using MaktabDataContracts.Responses.Transactions;

namespace Maktab.Domain.Services
{
     public class PaymentService : BaseService, IPaymentService
     {
          private const string getPaymentByCourseIdUrl = @"/api/course-payments/course/{0}";
          private const string getPaymentByStudentTransactionsIdUrl = @"/api/course-payments/studenttransactions/{0}";

          private const string getPaymentByIdUrl = @"/api/course-payments/{0}";
          private const string addPaymentUrl = @"/api/course-payments";
          private const string updatePaymentByIdUrl = @"/api/course-payments/{0}";
          private const string removePaymentByIdUrl = @"/api/course-payments/{0}";


          public PaymentService(IHttpService httpService, ILocalStorageService localStorageService) 
          : base(httpService, localStorageService)
          {
          }

          public async Task<StudentCourseTransactionResponse> AddtPaymentAsync(AddStudentCourseTransaction addCourseTransaction)
          {
               var result = await _httpService.Post<StudentCourseTransactionResponse>(addPaymentUrl, addCourseTransaction);
               return result;
          }

          public async Task<bool> RemovePaymentByIdAsync(Guid paymentId)
          {
               var formatedUrl = string.Format(removePaymentByIdUrl, paymentId);
               var result = await _httpService.Delete<bool>(formatedUrl);
               return result;
          }

          public async Task<CoursePaymentResponse> UpdatePaymentByIdAsync(Guid paymentId, CoursePaymentResponse coursePaymentUpdate)
          {
               var formatedUrl = string.Format(updatePaymentByIdUrl, paymentId);
               var result = await _httpService.Put<CoursePaymentResponse>(formatedUrl, coursePaymentUpdate);
               return result;
          }

          public async Task<CoursePaymentResponse> GetPaymentByIdAsync(Guid paymentId)
          {
               var formatedUrl = string.Format(getPaymentByIdUrl, paymentId);
               var result = await _httpService.Get<CoursePaymentResponse>(formatedUrl);
               return result;
          }

          public async Task<IEnumerable<CoursePaymentResponse>> GetPaymentByCourseIdAsync(Guid courseId)
          {
               var formatedUrl = string.Format(getPaymentByCourseIdUrl, courseId);
               var result = await _httpService.Get<IEnumerable<CoursePaymentResponse>>(formatedUrl);
               return result;
          }

          public async Task<IEnumerable<CoursePaymentResponse>> GetPaymentByStudentTransactionsIdIdAsync(Guid studentTransactionsId)
          {
               var formatedUrl = string.Format(getPaymentByStudentTransactionsIdUrl, studentTransactionsId);
               var result = await _httpService.Get<IEnumerable<CoursePaymentResponse>>(formatedUrl);
               return result;
          }

     }
}
