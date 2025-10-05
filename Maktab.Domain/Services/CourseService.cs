using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;

namespace Maktab.Domain.Services
{
     public class CourseService : BaseService, ICourseService
     {
          private readonly List<CourseResponseDetailed> _Courses;
          private Guid iccSchoolId = new Guid("FDCA4C86-DF0E-4CCC-BCE7-D4AE62F6E337");
          private Guid iccSchool2Id = new Guid("5AE84751-B58B-44D8-AC30-08F28E32BF15");


          public CourseService(IHttpService httpService, ILocalStorageService localStorageService)
          : base(httpService, localStorageService)
          {
               _Courses = GetCourses( new List<Guid>() { iccSchoolId, iccSchool2Id });

          }

          public async Task<IEnumerable<CourseResponseDetailed>> GetCoursesByInstitutionIdAsync(Guid institutionId)
          {
               return _Courses.Where(x => x.InstituteId == institutionId);
          }

          public async Task<CourseResponseDetailed> GetCourseByIdAsync(Guid courseId)
          {
               return _Courses.Find(x => x.CourseId == courseId);
          }

          private List<CourseResponseDetailed> GetCourses(IEnumerable<Guid> institutionIds)
          {
               var courses = new List<CourseResponseDetailed>()
               {
               new CourseResponseDetailed
            {
                CourseId = Guid.NewGuid(),
                InstituteId = institutionIds.First(),
                Name = "ICC Brossard Winter Camp",
                NameFr = "Camp d'hiver du ICC Brossard",
                IsActive = true,
                
                //Category = "winter camp",
                Description = "The day camp offers many fun activities that are suitable for children aged 4 to 13.  Every weekday, from December 23 to January 3, from 9 a.m. to 4 p.m., your child will have a great time doing empowering, educational and fun activities with our dynamic team leads!  The daycare will be open from 7 a.m. to 9 p.m. and from 4 pm to 5h30 pm. It is MANDATORY to read the policies and procedures document available here.  If you have any additional questions, we will be more than happy to answer you. When visiting the camp, please abide by the mosque dress code. We're looking forward to meeting you and your child soon, in shaa Allah!",
                DescriptionFr = "Offert durant les vacances hivernales, du 23 décembre au 3 janvier, de 09h00 à 16h00, en compagnie d'animateurs reconnus pour leur dynamisme, le camp d'hiver fait vivre aux enfants de 4 à 13 ans des expériences fort enrichissantes à travers des activités de loisir variées qui favorisent la vie de groupe et qui sont adaptées aux enfants.  Le service de garde sera disponible de 7h00 à 9h00 et de 16h00 à 17h30. Il est OBLIGATOIRE de lire attentivement le document accessible ici car il contient toutes les réponses aux questions que vous pourriez avoir. Si, après sa lecture, vous avez d'autres questions, il nous fera plaisir d'y répondre. Lors de vos visites au camp de jour, nous vous demandons de bien vouloir respecter le code vestimentaire de la mosquée, SVP. Au plaisir de vous voir bientôt avec votre enfant, in shaa Allah!",
               StartDate = DateTime.Now,
               EndDate = DateTime.Now.AddMonths(5),
               CanSelectMultipleEnrollmentGroups = true,
               
            },
          
            new CourseResponseDetailed
            {
                  CourseId = Guid.NewGuid(),
                InstituteId = institutionIds.Last(),
                Name = "Seerah of Prophet Muhammad (PBUH)",
                NameFr = "La Sira du Prophète Muhammad (PSL)",
                IsActive = true,
                //Category = "Islamic Studies",
                Description = "Explore the life, character, and mission of Prophet Muhammad (peace be upon him).",
                DescriptionFr = "Explorez la vie, le caractère et la mission du Prophète Muhammad (paix soit sur lui).",
                StartDate = DateTime.Now,
               EndDate = DateTime.Now.AddMonths(2),
                //ImageUrl = "images/courses/seerah.jpg",
                //Instructor = "Mufti Kareem Siddiqui",
                //Modules = new List<string>
                //{
                //    "Early Life in Makkah",
                //    "Prophethood & Revelation",
                //    "Migration to Madinah",
                //    "Key Battles and Lessons"
                //}
            }
               };
               return courses;
          }

          public async Task<IEnumerable<CourseResponseDetailed>> GetAllCoursesAsync()
          {
               return _Courses;
          }

          public Task<CourseResponseDetailed> AddCourseAsync(AddCourse addInstitute)
          {
               throw new NotImplementedException();
          }

          public Task<bool> IsCourseExistAsync(Guid instituteId, string courseName)
          {
               throw new NotImplementedException();
          }

          public Task<bool> RemoveCourseAsync(Guid instituteId)
          {
               throw new NotImplementedException();
          }

          public Task<bool> DeactivateCourseAsync(Guid instituteId)
          {
               throw new NotImplementedException();
          }
     }
}
