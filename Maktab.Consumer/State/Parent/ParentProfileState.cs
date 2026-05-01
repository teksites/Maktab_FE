using MaktabDataContracts.Responses.Addresses;
using MaktabDataContracts.Responses.Children;
using MaktabDataContracts.Responses.Course;
using MaktabDataContracts.Responses.OtherContacts;
using MaktabDataContracts.Responses.Transactions;
using MaktabDataContracts.Responses.Users;

namespace Maktab.Consumer.State.Parent
{
     public class ParentProfileState : BaseAppState
     {

          public UserInformationResponse? Profile { get; private set; }
          public void SetProfile(UserInformationResponse userInfo)
          {
               Profile = userInfo;
               NotifyStateChanged();
          }

          public UserInformationResponse? Spouse { get; private set; }
          public void SetSpouse(UserInformationResponse userInfo)
          {
               Spouse = userInfo;
               NotifyStateChanged();
          }

          public ExtendedUserInformationResponse? ExtendedProfileInfo { get; private set; }
          public void SetExtendedInfo(ExtendedUserInformationResponse userExtendedInfo)
          {
               ExtendedProfileInfo = userExtendedInfo;
               NotifyStateChanged();
          }

          private List<ChildResponse>? _children;
          public IReadOnlyList<ChildResponse> Children
          {
               get
               {
                    //if(_children == null )
                    //{
                    //     return Array.Empty<ChildResponse>();
                    //}

                    return _children;
               }
          }

          public void SetChildren(IEnumerable<ChildResponse> items)
          {
               _children = items.ToList();
               NotifyStateChanged();
          }

          public void AddChild(ChildResponse child)
          {
               _children ??= new List<ChildResponse>();

               _children.Add(child);
               NotifyStateChanged();
          }

          public bool RemoveChild(ChildResponse child)
          {
               if (_children == null) return false;

               var result = _children.Remove(child);
               NotifyStateChanged();

               return result;
          }

          public void ClearChildren()
          {
               _children = null;
               NotifyStateChanged();
          }

          public Lazy<object> ContactSyncLock { get; private set; } = new Lazy<object>();

          private List<OtherContactResponse>? _contacts;
          public IReadOnlyList<OtherContactResponse> Contacts
          {
               get
               {
                    //if (_contacts == null)
                    //{
                    //     return Array.Empty<OtherContactResponse>();
                    //}

                    return _contacts;
               }
          }

          public void SetContacts(IEnumerable<OtherContactResponse> items)
          {
               lock (ContactSyncLock)
               {
                    _contacts = items.ToList();
               }

               NotifyStateChanged();
          }

          public void AddContact(OtherContactResponse contact)
          {
               _contacts ??= new List<OtherContactResponse>();

               lock (ContactSyncLock)
               {
                    _contacts.Add(contact);
               }

               NotifyStateChanged();
          }

          public bool RemoveContact(OtherContactResponse contact)
          {
               if (_contacts == null) return false;

               lock (ContactSyncLock)
               {

                    var result = _contacts.Remove(contact);
                    NotifyStateChanged();

                    return result;
               }
          }

          public void ClearContact()
          {
               lock (ContactSyncLock)
               {
                    _contacts = null;
               }

               NotifyStateChanged();
          }

          public Lazy<object> AddressSyncLock { get; private set; } = new Lazy<object>();

          private List<AddressResponse>? _addresses;
          public IReadOnlyList<AddressResponse> Addresses
          {
               get
               {
                    //if (_addresses == null)
                    //{
                    //     return Array.Empty<AddressResponse>();
                    //}

                    return _addresses;
               }
          }

          public void SetAddresses(IEnumerable<AddressResponse> items)
          {
               lock (AddressSyncLock)
               {
                    _addresses = items.ToList();
                    NotifyStateChanged();
               }
          }

          public void AddAddress(AddressResponse contact)
          {
               _addresses ??= new List<AddressResponse>();

               lock (AddressSyncLock)
               {
                    _addresses.Add(contact);
                    NotifyStateChanged();
               }
          }

          public bool RemoveAddress(AddressResponse contact)
          {
               if (_addresses == null) return false;

               lock (AddressSyncLock)
               {
                    var result = _addresses.Remove(contact);
                    NotifyStateChanged();

                    return result;
               }
          }

          public void ClearAddress()
          {
               lock (AddressSyncLock)
               {
                    _addresses = null;
               }

               NotifyStateChanged();
          }


          public Lazy<object> EnrollmentSyncLock { get; private set; } = new Lazy<object>();


          private List<StudentCourseEnrollmentResponse>? _courseEnrollments;
          public IReadOnlyList<StudentCourseEnrollmentResponse> CourseEnrollments
          {
               get
               {
                    //if (_courseEnrollments == null)
                    //{
                    //     return Array.Empty<StudentCourseEnrollmentResponse>();
                    //}

                    return _courseEnrollments;
               }
          }


          public void SetCourseEnrollment(IEnumerable<StudentCourseEnrollmentResponse> items)
          {
               lock (EnrollmentSyncLock)
               {
                    _courseEnrollments = items?
                        .OrderBy(x => x.GroupTitle)
                        .ToList();
               }

               NotifyStateChanged();
          }

          public void AddCourseEnrollment(StudentCourseEnrollmentResponse enrollments)
          {
               lock (EnrollmentSyncLock)
               {
                    _courseEnrollments ??= new List<StudentCourseEnrollmentResponse>();

                    _courseEnrollments.Add(enrollments);
               }

               NotifyStateChanged();
          }

          public bool RemoveCourseEnrollment(StudentCourseEnrollmentResponse enrollments)
          {
               if (_courseEnrollments == null) return false;

               lock (EnrollmentSyncLock)
               {
                    var result = _courseEnrollments.Remove(enrollments);
                    NotifyStateChanged();

                    return result;
               }
          }

          public void ClearCourseEnrollment()
          {
               lock (EnrollmentSyncLock)
               {
                    _courseEnrollments = null;
               }

               NotifyStateChanged();
          }

          public Lazy<object> TransactionSyncLock { get; private set; } = new Lazy<object>();

          private List<StudentCourseTransactionResponse>? _courseTransactions;
          public IReadOnlyList<StudentCourseTransactionResponse> CourseTransactions
          {
               get
               {
                    //if (_courseTransactions == null)
                    //{
                    //     return Array.Empty<StudentCourseTransactionResponse>();
                    //}

                    return _courseTransactions;
               }
          }

          public void SetCourseTransactions(IEnumerable<StudentCourseTransactionResponse> items)
          {
               lock (TransactionSyncLock)
               {
                    _courseTransactions = items.ToList();
               }
               
               NotifyStateChanged();
          }

          public void AddCourseTransactions(StudentCourseTransactionResponse enrollments)
          {
               lock (TransactionSyncLock)
               {
                    _courseTransactions ??= new List<StudentCourseTransactionResponse>();

                    _courseTransactions.Add(enrollments);
               }

               NotifyStateChanged();
          }

          public bool RemoveCourseTransactions(StudentCourseTransactionResponse enrollments)
          {
               if (_courseTransactions == null) return false;

               lock (TransactionSyncLock)
               {

                    var result = _courseTransactions.Remove(enrollments);
                    NotifyStateChanged();
                    return result;
               }
          }

          public void ClearCourseTransactions()
          {
               lock (TransactionSyncLock)
               {
                    _courseTransactions = null;
               }

               NotifyStateChanged();
          }
     }
}
