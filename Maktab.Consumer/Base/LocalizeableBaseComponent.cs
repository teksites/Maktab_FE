using Maktab.Consumer.Dialogs;
using Maktab.Consumer.Localization;
using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Enums;
using MaktabDataContracts.Requests.Policies;
using MaktabDataContracts.Responses.Course;
using MaktabDataContracts.Responses.Institute;
using MaktabDataContracts.Responses.Transactions;
using MaktabDataContracts.Responses.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace Maktab.Consumer.Base
{
     public abstract class LocalizeableBaseComponent<T> : BaseComponent<T> where T : class
     {
          [Inject] protected IStringLocalizer<MaktabResources> L { get; set; }
          [Inject] protected IGlobalizationService GlobalizationService { get; set; }

          protected string GetAddressTypeLabel(AddressType type)
          {
               return type switch
               {
                    AddressType.Billing => L[MaktabResources.Address_Type_Billing],
                    AddressType.Institute => L[MaktabResources.Address_Type_Institute],
                    AddressType.Parent => L[MaktabResources.Address_Type_Parent],
                    AddressType.OtherContact => L[MaktabResources.Address_Type_OtherContact],
                    AddressType.Other => L[MaktabResources.Address_Type_Other],
                    _ => L[MaktabResources.Address_Type_Unknown]
               };
          }

          protected string GetEnrollmentStatusLabel(EnrollmentStatus enrollmentStatus)
          {
               return enrollmentStatus switch
               {
                    EnrollmentStatus.Awaiting => L[MaktabResources.EnrollmentStatus_Awaiting],
                    EnrollmentStatus.Enrolled => L[MaktabResources.EnrollmentStatus_Enrolled],
                    EnrollmentStatus.Registered => L[MaktabResources.EnrollmentStatus_Registered],
                    EnrollmentStatus.Refunded => L[MaktabResources.EnrollmentStatus_Refunded],
                    EnrollmentStatus.Cancelled => L[MaktabResources.EnrollmentStatus_Cancelled],
                    _ => L[MaktabResources.EnrollmentStatus_Unknown]
               };
          }

          protected string GetContactTypeLabel(ContactType type)
          {
               return type switch
               {
                    ContactType.Pickup => L[MaktabResources.Contact_Type_Pickup],
                    ContactType.Emergency => L[MaktabResources.Contact_Type_Emergency],
                    ContactType.Other => L[MaktabResources.Contact_Type_Other],
                    _ => L[MaktabResources.Contact_Type_Unknown]
               };
          }

          protected string GetRelationshipLabel(Relationship type)
          {
               return type switch
               {
                    Relationship.Father => L[MaktabResources.Relationship_Type_Father],
                    Relationship.Mother => L[MaktabResources.Relationship_Type_Mother],
                    Relationship.Grandparent => L[MaktabResources.Relationship_Type_Grandparent],
                    Relationship.Uncle => L[MaktabResources.Relationship_Type_Uncle],
                    Relationship.Aunt => L[MaktabResources.Relationship_Type_Aunt],
                    Relationship.Cousin => L[MaktabResources.Relationship_Type_Cousin],
                    Relationship.Relative => L[MaktabResources.Relationship_Type_Relative],
                    Relationship.Guardian => L[MaktabResources.Relationship_Type_Guardian],
                    Relationship.Teacher => L[MaktabResources.Relationship_Type_Teacher],
                    Relationship.FamilyFriend => L[MaktabResources.Relationship_Type_FamilyFriend],
                    _ => L[MaktabResources.Relationship_Type_Unknown]
               };
          }

          protected string GetPaymentStatusLabel(PaymentStatus type)
          {
               return type switch
               {
                    PaymentStatus.Paid => L[MaktabResources.Payment_Status_Paid],
                    PaymentStatus.PartiallyPaid => L[MaktabResources.Payment_Status_PartiallyPaid],
                    PaymentStatus.Unpaid => L[MaktabResources.Payment_Status_Unpaid],
                    _ => L[MaktabResources.Payment_Status_Unpaid]
               };
          }

          protected string GetGenderLabel(Gender type)
          {
               return type switch
               {
                    Gender.Male => L[MaktabResources.Gender_Type_Male],
                    Gender.Female => L[MaktabResources.Gender_Type_Female],
                    _ => L[MaktabResources.Gender_Type_Unknown]
               };
          }
          protected string GetUserType(UserType userType)
          {
               return userType switch
               {
                    UserType.Child => L[MaktabResources.Child],
                    UserType.Father => L[MaktabResources.Relationship_Type_Father],
                    UserType.Mother => L[MaktabResources.Relationship_Type_Mother],
                    UserType.Guardian => L[MaktabResources.Relationship_Type_Guardian],
                    _ => L[MaktabResources.Relationship_Type_Unknown]
               };
          }

          protected string GetInstituteName(InstituteResponse institute)
          {
               if (institute == null) return string.Empty;

               if (GlobalizationService.CurrentCulture.TwoLetterISOLanguageName == "fr")
               {
                    return institute.NameFr;
               }
               else
               {
                    return institute.Name;
               }
          }

          protected string GetInstituteDescription(InstituteResponse institute)
          {
               if (GlobalizationService.CurrentCulture.TwoLetterISOLanguageName == "fr")
               {
                    return institute?.DescriptionFr;
               }
               else
               {
                    return institute?.Description;
               }
          }

          protected string GetCourseName(CourseResponseDetailed course)
          {
               if (course == null) return string.Empty;

               if (GlobalizationService.CurrentCulture.TwoLetterISOLanguageName == "fr")
               {
                    return course.NameFr;
               }
               else
               {
                    return course.Name;
               }
          }

          protected string GetCourseDetails(CourseResponseDetailed course)
          {
               if (course == null) return string.Empty;

               if (GlobalizationService.CurrentCulture.TwoLetterISOLanguageName == "fr")
               {
                    return course.DetailsFr;
               }
               else
               {
                    return course.Details;
               }
          }

          protected string GetCourseDescription(CourseResponseDetailed course)
          {
               if (course == null) return string.Empty;

               if (GlobalizationService.CurrentCulture.TwoLetterISOLanguageName == "fr")
               {
                    return course.DescriptionFr;
               }
               else
               {
                    return course.Description;
               }
          }

          protected string GetCourseGroupTitle(CourseEnrollmentGroupResponse group)
          {
               if (group == null) return string.Empty;

               if (GlobalizationService.CurrentCulture.TwoLetterISOLanguageName == "fr")
               {
                    return group.GroupTitleFr;
               }
               else
               {
                    return group.GroupTitle;
               }
          }

          protected string GetConsentDescription(ChildConsent conset)
          {
               if (conset == null) return string.Empty;

               if (GlobalizationService.CurrentCulture.TwoLetterISOLanguageName == "fr")
               {
                    return conset.NameFr;
               }
               else
               {
                    return conset.Name;
               }
          }

          protected MarkupString GetConsentDescriptionMarkupString(ChildConsent conset)
          {
               return new MarkupString(GetConsentDescription(conset));
          }

          protected string GetPaymentDescription(FeeInstallment installment)
          {
               if (installment == null) return string.Empty;

               if (GlobalizationService.CurrentCulture.TwoLetterISOLanguageName == "fr")
               {
                    return installment.DescriptionFr;
               }
               else
               {
                    return installment.Description;
               }
          }

          protected Task<bool?> ShowMessageBoxAsync(IDialogService dialogService, string title, string message, string yesText, string noText = null)
          {
               var messageBoxOptions = new MessageBoxOptions
               {
                    Title = title,
                    Message = message,
                    YesText = yesText,
                    NoText = noText
               };
               var options = new DialogOptions { MaxWidth = MaxWidth.Small, CloseButton = true, BackdropClick = false };
               var resultTask = dialogService.ShowMessageBox(messageBoxOptions, options);
               return resultTask;
          }

          protected Task<bool?> ShowMessagePromptAsync(IDialogService dialogService, string title, string message)
          {
               return ShowMessageBoxAsync(
                    dialogService,
                    title,
                    message,
                    L[MaktabResources.OK]);
          }

          protected async Task<DialogResult?> OpenEnrollChildDialog(IDialogService dialogService)
          {
               var parameters = new DialogParameters { };
               var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true, BackdropClick = false };

               var dialog = await dialogService.ShowAsync<EnrollChildInCourseDialog>(L[MaktabResources.Enroll_Participant], parameters, options);
               var result = await dialog.Result;

               return result;
          }

          protected async Task<bool> HasValidFamilyDetails(IUserService userService, IDialogService dialogService, UserInformationResponse userInfo)
          {
               bool hasValidSpouse = false;
               bool hasEmergencyContact = false;
               var familyDetails = await userService.GetFamilyDetailInfoByFamilyId(userInfo.FamilyId);
               if (familyDetails != null)
               {
                    if (familyDetails.FamilyInformation?.Count() == 2)
                    {
                         var spouse = familyDetails.FamilyInformation.FirstOrDefault(x => x.UserId != userInfo.UserId && x.Relationship != userInfo.Relationship);
                         if (spouse != null)
                         {
                              hasValidSpouse = true;
                         }
                    }

                    if (familyDetails.OtherContacts?.Where(c => c.ContactType == MaktabDataContracts.Enums.ContactType.Emergency).Any() == true)
                    {
                         hasEmergencyContact = true;
                    }

               }

               if (!hasValidSpouse && !hasEmergencyContact)
               {
                    await ShowMessagePromptAsync(
                         dialogService,
                         L[MaktabResources.Error],
                         L[MaktabResources.Msg_Error_Add_Spouse_And_Emergency_Details_To_Add_Enrollment]);

                    return false;

               }
               else if (!hasValidSpouse)
               {
                    await ShowMessagePromptAsync(
                         dialogService,
                         L[MaktabResources.Error],
                         L[MaktabResources.Msg_Error_Add_Spouse_Details_To_Continue]);
                    return false;
               }
               else if (!hasEmergencyContact)
               {
                    await ShowMessagePromptAsync(
                         dialogService,
                         L[MaktabResources.Error],
                         L[MaktabResources.Msg_Error_Add_Emergency_Contact_To_Continue]);
                    return false;
               }

               return true;
          }

          protected async Task<bool> ValidateSpouseDataAsync(IUserService userService, IDialogService dialogService, UserInformationResponse userInfo)
          {
               bool hasValidSpouse = false;
               var familyDetails = await userService.GetFamilyDetailInfoByFamilyId(userInfo.FamilyId);
               if (familyDetails?.FamilyInformation?.Any() == true)
               {
                    var familyUserCount = familyDetails.FamilyInformation.Count();
                    if (familyUserCount == 1)
                    {
                         hasValidSpouse = true; // If there is only one user in the family, we consider it valid (the user themselves).
                    }
                    else if (familyUserCount == 2)
                    {
                         var spouse = familyDetails.FamilyInformation.FirstOrDefault(x => x.UserId != userInfo.UserId && x.Relationship != userInfo.Relationship);
                         if (spouse != null)
                         {
                              hasValidSpouse = true; // If there is a spouse in the family, we consider it valid.
                         }
                    }
               }

               if (!hasValidSpouse)
               {
                    await ShowMessagePromptAsync(
                              dialogService,
                              L[MaktabResources.Error],
                              L[MaktabResources.Msg_Error_Invalid_Spouse_Details]);
               }

               return hasValidSpouse;
          }
     }
}
