using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;

namespace Maktab.Domain.Services
{
     public class CourseEnrollmentService : ICourseEnrollmentService
     {
          private IDictionary<Guid, List<StudentCourseEnrollmentResponse>> courseResponses = new Dictionary<Guid, List<StudentCourseEnrollmentResponse>>();

          public async Task<StudentCourseEnrollmentResponse> AddCourseEnrollmentAsync(Guid familyId, AddStudentCourseEnrollment studentCourseEnrollment)
          {
               if (!courseResponses.ContainsKey(familyId))
               {
                    courseResponses[familyId] = new List<StudentCourseEnrollmentResponse>();
               }

               StudentCourseEnrollmentResponse enrollment = CreateResponseFromRequest(familyId, studentCourseEnrollment);

               courseResponses[familyId].Add(enrollment);
               return enrollment;
          }

          private static StudentCourseEnrollmentResponse CreateResponseFromRequest(Guid familyId, AddStudentCourseEnrollment studentCourseEnrollment)
          {
               return new MaktabDataContracts.Responses.Course.StudentCourseEnrollmentResponse()
               {
                    StudentCourseEnrollmentId = Guid.NewGuid(),
                    GroupId = studentCourseEnrollment.GroupId,
                    ChildId = studentCourseEnrollment.ChildId,
                    FamilyId = familyId,
                    CreatedAt = DateTime.Now,
               };
          }

          public async Task<IEnumerable<StudentCourseEnrollmentResponse>> AddCourseEnrollmentsAsync(Guid familyId, IEnumerable<AddStudentCourseEnrollment> studentCourseEnrollments)
          {
               if (!courseResponses.ContainsKey(familyId))
               {
                    courseResponses[familyId] = new List<StudentCourseEnrollmentResponse>();
               }

               var enrollments = new List<StudentCourseEnrollmentResponse>();

               foreach( var request in studentCourseEnrollments)
               {
                    StudentCourseEnrollmentResponse enrollment = CreateResponseFromRequest(familyId, request);
                    enrollments.Add(enrollment);
               }

               courseResponses[familyId].AddRange(enrollments);
               return enrollments;
          }

          public async Task<bool> RemoveCourseEnrollmentAsync(Guid familyId, Guid enrollmentId)
          {
               if (courseResponses[familyId]?.Any() == true)
               {
                    var enrollment = courseResponses[familyId].Find(x => x.StudentCourseEnrollmentId == enrollmentId);
                    if (enrollment != null)
                    {
                         courseResponses[familyId].Remove(enrollment);
                         return true;
                    }
               }

               return false;
          }

          public async Task<StudentCourseTransactionResponse> GetCoursePaymentDetailsForFamily(Guid familyId)
          {

               if (courseResponses.ContainsKey(familyId))
               {
                    var list = courseResponses[familyId];

                    var transaction = new StudentCourseTransactionResponse()
                    {
                         FamilyId = familyId,
                         CreatedAt = DateTime.UtcNow,
                         PayableFee = 75 * list.Count,
                         StudentCourseEnrollmentId = list.Select(x => x.StudentCourseEnrollmentId).ToList(),
                         StudentCourseTransactionId = Guid.NewGuid(),
                         PaymentCode = $"ABCD-{familyId.ToString()}",
                         AmountDiscounted = (list.Count> 1)? 10 : 0,
                         TransactionStatus = MaktabDataContracts.Enums.TransactionStatus.AwaitingPayment
                    };

                    transaction.TotalPayable = transaction.PayableFee - transaction.AmountDiscounted;

                    return transaction;
               }

               return default;
          }

          public async Task<IEnumerable<StudentCourseEnrollmentResponse>> GetCourseEnrollmentsByFamilyIdAsync(Guid familyId)
          {
               if (courseResponses[familyId]?.Any() == true)
               {
                    return courseResponses[familyId];
               }

               return Enumerable.Empty<StudentCourseEnrollmentResponse>();
          }
     }
}
