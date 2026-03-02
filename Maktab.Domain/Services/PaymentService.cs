using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;
using MaktabDataContracts.Responses.Transactions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maktab.Domain.Services
{
     /// <summary>
     /// Payment service with comprehensive error handling, validation, and logging.
     /// Production-ready implementation for backend API integration.
     /// </summary>
     public class PaymentService : BaseService, IPaymentService
     {
          // API Endpoints (properly formatted, not "formated")
          private const string getPaymentByCourseIdUrl = @"/api/course-payments/course/{0}";
          private const string getPaymentByStudentTransactionsIdUrl = @"/api/course-payments/studenttransactions/{0}";
          private const string getPaymentByIdUrl = @"/api/course-payments/{0}";
          private const string addPaymentUrl = @"/api/course-payments";
          private const string updatePaymentByIdUrl = @"/api/course-payments/{0}";
          private const string removePaymentByIdUrl = @"/api/course-payments/{0}";

          private readonly ILogger<PaymentService> _logger;

          public PaymentService(
               IHttpService httpService,
               ILocalStorageService localStorageService,
               ILogger<PaymentService> logger)
               : base(httpService, localStorageService)
          {
               _logger = logger ?? throw new ArgumentNullException(nameof(logger));
          }

          /// <summary>
          /// Add payment with validation and error handling
          /// </summary>
          public async Task<StudentCourseTransactionResponse> AddtoPaymentAsync(AddStudentCourseTransaction addCourseTransaction)
          {
               try
               {
                    if (addCourseTransaction == null)
                    {
                         _logger.LogWarning("AddtPaymentAsync called with null transaction");
                         throw new ArgumentNullException(nameof(addCourseTransaction), "Course transaction cannot be null");
                    }

                    _logger.LogInformation("Adding new payment");

                    var result = await _httpService.Post<StudentCourseTransactionResponse>(addPaymentUrl, addCourseTransaction);

                    if (result == null)
                    {
                         _logger.LogError("Server returned null response when adding payment");
                         throw new InvalidOperationException("Server did not return payment confirmation");
                    }

                    _logger.LogInformation("Successfully added payment");
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error adding payment");
                    throw;
               }
          }

          /// <summary>
          /// Remove payment by ID (backend uses DELETE - RESTful)
          /// </summary>
          public async Task<bool> RemovePaymentByIdAsync(Guid paymentId)
          {
               try
               {
                    if (paymentId == Guid.Empty)
                    {
                         _logger.LogWarning("RemovePaymentByIdAsync called with empty GUID");
                         throw new ArgumentException("Payment ID cannot be empty", nameof(paymentId));
                    }

                    _logger.LogInformation("Removing payment {PaymentId}", paymentId);

                    var formattedUrl = string.Format(removePaymentByIdUrl, paymentId);
                    var result = await _httpService.Delete<bool>(formattedUrl);

                    _logger.LogInformation("Successfully removed payment {PaymentId}", paymentId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error removing payment {PaymentId}", paymentId);
                    throw;
               }
          }

          /// <summary>
          /// Update payment (backend uses PUT - RESTful)
          /// </summary>
          public async Task<CoursePaymentResponse> UpdatePaymentByIdAsync(Guid paymentId, CoursePaymentResponse coursePaymentUpdate)
          {
               try
               {
                    if (paymentId == Guid.Empty)
                    {
                         _logger.LogWarning("UpdatePaymentByIdAsync called with empty GUID");
                         throw new ArgumentException("Payment ID cannot be empty", nameof(paymentId));
                    }

                    if (coursePaymentUpdate == null)
                    {
                         _logger.LogWarning("UpdatePaymentByIdAsync called with null payment");
                         throw new ArgumentNullException(nameof(coursePaymentUpdate), "Payment cannot be null");
                    }

                    _logger.LogInformation("Updating payment {PaymentId}", paymentId);

                    var formattedUrl = string.Format(updatePaymentByIdUrl, paymentId);
                    var result = await _httpService.Put<CoursePaymentResponse>(formattedUrl, coursePaymentUpdate);

                    if (result == null)
                    {
                         _logger.LogError("Server returned null response when updating payment");
                         throw new InvalidOperationException("Server did not return updated payment");
                    }

                    _logger.LogInformation("Successfully updated payment {PaymentId}", paymentId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error updating payment {PaymentId}", paymentId);
                    throw;
               }
          }

          /// <summary>
          /// Get payment by ID with error handling
          /// </summary>
          public async Task<CoursePaymentResponse> GetPaymentByIdAsync(Guid paymentId)
          {
               try
               {
                    if (paymentId == Guid.Empty)
                    {
                         _logger.LogWarning("GetPaymentByIdAsync called with empty GUID");
                         throw new ArgumentException("Payment ID cannot be empty", nameof(paymentId));
                    }

                    _logger.LogInformation("Fetching payment {PaymentId}", paymentId);

                    var formattedUrl = string.Format(getPaymentByIdUrl, paymentId);
                    var result = await _httpService.Get<CoursePaymentResponse>(formattedUrl);

                    if (result == null)
                    {
                         _logger.LogWarning("Payment {PaymentId} not found", paymentId);
                         return null;
                    }

                    _logger.LogInformation("Successfully fetched payment {PaymentId}", paymentId);
                    return result;
               }
               catch (HttpRequestException ex)
               {
                    _logger.LogError(ex, "Network error fetching payment {PaymentId}", paymentId);
                    throw;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Unexpected error fetching payment {PaymentId}", paymentId);
                    throw;
               }
          }

          /// <summary>
          /// Get payments by course ID with error handling
          /// </summary>
          public async Task<IEnumerable<CoursePaymentResponse>> GetPaymentByCourseIdAsync(Guid courseId)
          {
               try
               {
                    if (courseId == Guid.Empty)
                    {
                         _logger.LogWarning("GetPaymentByCourseIdAsync called with empty GUID");
                         throw new ArgumentException("Course ID cannot be empty", nameof(courseId));
                    }

                    _logger.LogInformation("Fetching payments for course {CourseId}", courseId);

                    var formattedUrl = string.Format(getPaymentByCourseIdUrl, courseId);
                    var result = await _httpService.Get<IEnumerable<CoursePaymentResponse>>(formattedUrl);

                    if (result == null)
                    {
                         _logger.LogWarning("No payments found for course {CourseId}", courseId);
                         return new List<CoursePaymentResponse>();
                    }

                    _logger.LogInformation("Successfully fetched payments for course {CourseId}", courseId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error fetching payments for course {CourseId}", courseId);
                    throw;
               }
          }

          /// <summary>
          /// Get payments by student transaction ID with error handling
          /// </summary>
          public async Task<IEnumerable<CoursePaymentResponse>> GetPaymentByStudentTransactionsIdIdAsync(Guid studentTransactionsId)
          {
               try
               {
                    if (studentTransactionsId == Guid.Empty)
                    {
                         _logger.LogWarning("GetPaymentByStudentTransactionsIdIdAsync called with empty GUID");
                         throw new ArgumentException("Student transaction ID cannot be empty", nameof(studentTransactionsId));
                    }

                    _logger.LogInformation("Fetching payments for student transaction {StudentTransactionId}", studentTransactionsId);

                    var formattedUrl = string.Format(getPaymentByStudentTransactionsIdUrl, studentTransactionsId);
                    var result = await _httpService.Get<IEnumerable<CoursePaymentResponse>>(formattedUrl);

                    if (result == null)
                    {
                         _logger.LogWarning("No payments found for student transaction {StudentTransactionId}", studentTransactionsId);
                         return new List<CoursePaymentResponse>();
                    }

                    _logger.LogInformation("Successfully fetched payments for student transaction {StudentTransactionId}", studentTransactionsId);
                    return result;
               }
               catch (Exception ex)
               {
                    _logger.LogError(ex, "Error fetching payments for student transaction {StudentTransactionId}", studentTransactionsId);
                    throw;
               }
          }
     }
}
