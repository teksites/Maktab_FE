using MaktabDataContracts.Responses.Course;
using MaktabDataContracts.Responses.Institute;

namespace Maktab.Consumer.State.Course
{
     public class InstituteState : BaseAppState
     {

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

          public void SetInstitute(IEnumerable<InstituteResponse> items)
          {
               _institutes = items.ToList();
               NotifyStateChanged();
          }

          public void AddInstitute(InstituteResponse child)
          {
               _institutes ??= new List<InstituteResponse>();

               _institutes.Add(child);
               NotifyStateChanged();
          }

          public bool RemoveInstitute(InstituteResponse child)
          {
               if (_institutes == null) return false;

               var result = _institutes.Remove(child);
               NotifyStateChanged();

               return result;
          }

          public void ClearInstitutes()
          {
               _institutes = null;
               NotifyStateChanged();
          }

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
               _courses = items.ToList();
               NotifyStateChanged();
          }

          public void AddCourse(CourseResponseDetailed course)
          {
               _courses ??= new List<CourseResponseDetailed>();

               _courses.Add(course);
               NotifyStateChanged();
          }

          public bool RemoveCourse(CourseResponseDetailed course)
          {
               if (_courses == null) return false;

               var result = _courses.Remove(course);
               NotifyStateChanged();

               return result;
          }

          public void ClearCourses()
          {
               _courses = null;
               NotifyStateChanged();
          }
     }
}
