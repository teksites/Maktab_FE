using Maktab.Consumer.State;
using Maktab.Core.Interfaces.Services;
using Maktab.Domain.Services;
using Maktab.Models.Models;
using MaktabDataContracts.Responses.Course;

namespace Maktab.Consumer.Extensions
{
     public static class UserStateServiceExtension
     {
          public static async Task LoadChildrenData(this UserStateService userStateService, IChildrenService childrenService, Guid familyId, bool forceReload = false)
          {
               if (userStateService.ParentState.Children == null || forceReload)
               {
                    var children = await childrenService.GetChildrenByFamilyIdAsync(familyId);
                    if (children?.Any() == true)
                    {
                         userStateService.ParentState.SetChildren(children);
                    }
                    else
                    {
                         userStateService.ParentState.ClearChildren();
                    }
               }
          }

          public static async Task LoadUserProfileData(this UserStateService userStateService, IUserService userService, Guid userId, bool forceReload = false)
          {
               if (userStateService.ParentState.Profile == null || forceReload)
               {
                    var profile = await userService.GetUserByIdAsync(userId);
                    userStateService.ParentState.SetProfile(profile);
               }
          }

          public static async Task LoadUserSpouseData(this UserStateService userStateService, IUserService userService, Guid userId, Guid familyId, bool forceReload = false)
          {
               if (userStateService.ParentState.Spouse == null || forceReload)
               {
                    var familyDetails = await userService.GetFamilyByFamilyId(familyId);
                    if (familyDetails?.Any() == true)
                    {
                         var spouse = familyDetails.FirstOrDefault(x => x.UserId != userId);
                         if (spouse != null)
                         {
                              userStateService.ParentState.SetSpouse(spouse);
                         }
                    }
               }
          }

          public static async Task LoadUserExtendedProfileData(this UserStateService userStateService, IUserService userService, Guid userId, bool forceReload = false)
          {
               if (userStateService.ParentState.ExtendedProfileInfo  == null || forceReload)
               {
                    var profile = await userService.GetExtendedInfoByUserIdAsync(userId);
                    userStateService.ParentState.SetExtendedInfo(profile);
               }
          }


          public static async Task LoadCourseEnrollments(this UserStateService userStateService, ICourseEnrollmentService courseEnrollmentService, Guid familyId, bool forceReload = false)
          {
               if (userStateService.ParentState.CourseEnrollments == null || forceReload)
               {
                    var enrollments = await courseEnrollmentService.GetCourseEnrollmentsByFamilyIdAsync(familyId);
                    if (enrollments?.Any() == true)
                    {
                         userStateService.ParentState.SetCourseEnrollment(enrollments);
                    }
                    else
                    {
                         userStateService.ParentState.ClearCourseEnrollment();
                    }
               }
          }

          public static async Task LoadCourseTransactionsByInstituteId(this UserStateService userStateService, ICourseEnrollmentTransactionService courseEnrollmentTransactionService, IEnumerable<StudentCourseEnrollmentResponse> studentCourseEnrollments, IEnumerable<CourseResponseDetailed> courses, Guid familyId, bool forceReload = false)
          {
               if (userStateService.ParentState.CourseTransactions == null || forceReload)
               {
                    var enrolledCoursesIds = studentCourseEnrollments.Select(x => x.CourseId).ToList();
                    var enrolledInstitutes = courses.Where(x => enrolledCoursesIds.Contains(x.CourseId))?.Select(y => y.InstituteId).Distinct();

                    if (enrolledInstitutes?.Any() == true)
                    {
                         foreach (var instituteId in enrolledInstitutes)
                         {
                              var courseTransactions = await courseEnrollmentTransactionService.GetCourseEnrollmentTranasctionByFamilyAndInstituteIdAsync(familyId, instituteId);
                              if (courseTransactions?.Any() == true)
                              {
                                   foreach (var courseTransaction in courseTransactions)
                                   {
                                        userStateService.ParentState.AddCourseTransactions(courseTransaction);
                                   }
                              }
                         }
                    }
               }
          }

          public static async Task LoadCourseTransactionsByCourse(this UserStateService userStateService, ICourseEnrollmentTransactionService courseEnrollmentTransactionService, IEnumerable<StudentCourseEnrollmentResponse> studentCourseEnrollments, Guid familyId, bool forceReload = false)
          {
               if (userStateService.ParentState.CourseTransactions == null || forceReload)
               {
                    var enrolledCourseIds = studentCourseEnrollments.Select(x => x.CourseId).Distinct();
                    if (enrolledCourseIds?.Any() == true)
                    {
                         foreach (var courseId in enrolledCourseIds)
                         {
                              var courseTransactions = await courseEnrollmentTransactionService.GetCourseEnrollmentTranasctionByFamilyAndCourseIdAsync(familyId, courseId);
                              if (courseTransactions?.Any() == true)
                              {
                                   foreach (var courseTransaction in courseTransactions)
                                   {
                                        userStateService.ParentState.AddCourseTransactions(courseTransaction);
                                   }
                              }
                         }
                    }
               }
          }

          public static async Task LoadAddressesData(this UserStateService userStateService, IAddressService addressService, Guid connectedId, bool forceReload = false)
          {

               if (userStateService.ParentState.Addresses == null || forceReload)
               {
                    var familyAddress = await addressService.GetAddressByConnectedId(connectedId);
                    if (familyAddress != null)
                    {
                         userStateService.ParentState.AddAddress(familyAddress);
                    }
                    //else
                    //{
                    //     userStateService.ParentState.ClearAddress();
                    //}
               }
          }

          public static async Task LoadActiveInstitutesData(this UserStateService userStateService, IInstitutionService institutionService, bool forceReload = false)
          {
               if (userStateService.InstituteState.Institutes == null || forceReload)
               {
                    var institutes = await institutionService.GetAllActiveInstitutionsAsync();
                    if (institutes?.Any() == true)
                    {
                         userStateService.InstituteState.SetInstitutes(institutes);
                    }
                    else
                    {
                         userStateService.InstituteState.ClearInstitutes();
                    }
               }
          }

          public static async Task LoadActiveCoursesData(this UserStateService userStateService, ICourseService courseService, bool forceReload = false)
          {
               if (userStateService.InstituteState.Courses == null || forceReload)
               {
                    var courses = await courseService.GetCurrentActiveCoursesAsync();
                    if (courses?.Any() == true)
                    {
                         userStateService.InstituteState.SetCourses(courses);
                    }
                    else
                    {
                         userStateService.InstituteState.ClearCourses();
                    }
               }
          }
     }
}
