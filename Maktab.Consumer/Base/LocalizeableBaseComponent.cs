using Maktab.Consumer.Localization;
using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Enums;
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

          protected string GetGenderLabel(Gender type)
          {
               return type switch
               {
                    Gender.Male => L[MaktabResources.Gender_Type_Male],
                    Gender.Female => L[MaktabResources.Gender_Type_Female],
                    _ => L[MaktabResources.Gender_Type_Unknown]
               };
          }
     }
}
