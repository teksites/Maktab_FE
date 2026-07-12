using Maktab.Consumer.Localization;
using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Enums;
using MaktabDataContracts.Requests.Policies;
using MaktabDataContracts.Responses.Course;
using MaktabDataContracts.Responses.Institute;
using MaktabDataContracts.Responses.Transactions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

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
     }
}
