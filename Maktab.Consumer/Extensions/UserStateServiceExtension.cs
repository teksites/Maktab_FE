using Maktab.Consumer.State;
using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Responses.Course;
using MudBlazor;

namespace Maktab.Consumer.Extensions
{
     public static class UserStateServiceExtension
     {
          public static async Task LoadChildrenData(this UserStateService userStateService, IChildrenService childrenService, Guid familyId, bool forceReload = false)
          {
               if (userStateService.ParentState.Children == null || forceReload)
               {
                    var childCollection = await childrenService.GetChildrenByFamilyIdAsync(familyId);
                    if (childCollection?.Any() == true)
                    {
                         var sortedCollection = childCollection.Where(x => x.IsActive).OrderBy(x => x.FirstName);
                         userStateService.ParentState.SetChildren(sortedCollection);
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
                         var orderedEnrollment = enrollments.OrderBy(x => x.ChildName);
                         userStateService.ParentState.SetCourseEnrollment(orderedEnrollment);
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

          public static async Task LoadCourseTransactionsByCourse(this UserStateService userStateService, ICourseEnrollmentTransactionService courseEnrollmentTransactionService, IEnumerable<Guid> courseIds, Guid familyId, bool forceReload = false)
          {
               if (userStateService.ParentState.CourseTransactions == null || forceReload)
               {
                    userStateService.ParentState.ClearCourseTransactions();
                    if (courseIds?.Any() == true)
                    {
                         foreach (var courseId in courseIds)
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

          public static async Task LoadCourseTransactionsByCourse(this UserStateService userStateService, ICourseEnrollmentTransactionService courseEnrollmentTransactionService, IEnumerable<StudentCourseEnrollmentResponse> studentCourseEnrollments, Guid familyId, bool forceReload = false)
          {
               if (userStateService.ParentState.CourseTransactions == null || forceReload)
               {
                    userStateService.ParentState.ClearCourseTransactions();
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
                    var familyAddress = await addressService.GetAddressesByConnectedId(connectedId);
                    if (familyAddress?.Any() == true)
                    {
                         userStateService.ParentState.SetAddresses(familyAddress);
                    }
               }
          }

          public static async Task LoadContactsData(this UserStateService userStateService, IContactService contactService, Guid connectedId, bool forceReload = false)
          {
               if (userStateService.ParentState.Addresses == null || forceReload)
               {
                    var familyContacts = await contactService.GetContactsByFamilyId(connectedId);
                    if (familyContacts?.Any() == true)
                    {
                         var sortedCollection = familyContacts.OrderBy(x => x.FirstName);
                         userStateService.ParentState.SetContacts(sortedCollection);
                    }
               }
          }

          public static async Task LoadActiveInstitutesData(this UserStateService userStateService, IInstitutionService institutionService, bool forceReload = false)
          {
               if (userStateService.InstituteState.Institutes == null || forceReload)
               {
                    var institutes = await institutionService.GetAllActiveInstitutionsAsync();
                    if (institutes?.Any() == true)
                    {
                         var sortedCollection = institutes.OrderBy(c => c.Name);
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
                         var sortedCourses = courses.OrderBy(c => c.Name);
                         userStateService.InstituteState.SetCourses(sortedCourses);
                    }
                    else
                    {
                         userStateService.InstituteState.ClearCourses();
                    }
               }
          }

          public static async Task LoadStudentConsentData(this UserStateService userStateService, IInstitutionService institutionService, bool forceReload = false)
          {
               if (userStateService.InstituteState.ChildConsents == null || forceReload)
               {
                    var consentPolicies = await institutionService.GetChildConsentPoliciesAsync();
                    if (consentPolicies?.Any() == true)
                    {
                         userStateService.InstituteState.SetChildConsent(consentPolicies);
                    }
                    else
                    {
                         userStateService.InstituteState.ClearChildConsent();
                    }
               }
          }
     }
}
