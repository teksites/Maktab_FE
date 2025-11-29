using MaktabDataContracts.Responses.Course;
using MaktabDataContracts.Responses.Institute;

namespace Maktab.Consumer.State.Course
{
     public class InstituteState : BaseAppState
     {
          public Lazy<object> InstituteSyncLock { get; private set; } = new Lazy<object>();

          private List<InstituteResponse>? _institutes;
          public IReadOnlyList<InstituteResponse> Institutes
          {
               get
               {
                    //if (_institutes == null)
                    //{
                    //     return Array.Empty<InstituteResponse>();
                    //}

                    return _institutes;
               }
          }

          public void SetInstitutes(IEnumerable<InstituteResponse> items)
          {
               lock (InstituteSyncLock)
               {
                    _institutes = items.ToList();
               }

               NotifyStateChanged();
          }

          public void AddInstitute(InstituteResponse child)
          {
               lock (InstituteSyncLock)
               {
                    _institutes ??= new List<InstituteResponse>();

                    _institutes.Add(child);
               }

               NotifyStateChanged();
          }

          public bool RemoveInstitute(InstituteResponse child)
          {
               if (_institutes == null) return false;

               lock (InstituteSyncLock)
               {
                    var result = _institutes.Remove(child);
                    NotifyStateChanged();

                    return result;
               }
          }

          public void ClearInstitutes()
          {
               lock (InstituteSyncLock)
               {
                    _institutes = null;
               }

               NotifyStateChanged();
          }

          public Lazy<object> CourseSyncLock { get; private set; } = new Lazy<object>();


          private List<CourseResponseDetailed>? _courses;
          public IReadOnlyList<CourseResponseDetailed> Courses
          {
               get
               {
                    //if (_courses == null)
                    //{
                    //     return Array.Empty<CourseResponseDetailed>();
                    //}

                    return _courses;
               }
          }

          public void SetCourses(IEnumerable<CourseResponseDetailed> items)
          {
               lock (CourseSyncLock)
               {
                    _courses = items.ToList();
                    NotifyStateChanged();
               }
          }

          public void AddCourse(CourseResponseDetailed course)
          {
               lock (CourseSyncLock)
               {
                    _courses ??= new List<CourseResponseDetailed>();

                    _courses.Add(course);
               }

               NotifyStateChanged();
          }

          public bool RemoveCourse(CourseResponseDetailed course)
          {
               if (_courses == null) return false;

               lock (CourseSyncLock)
               {
                    var result = _courses.Remove(course);
                    NotifyStateChanged();

                    return result;
               }
          }

          public void ClearCourses()
          {
               lock (CourseSyncLock)
               {
                    _courses = null;
               }

               NotifyStateChanged();
          }
     }
}
