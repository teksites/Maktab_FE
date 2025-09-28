using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.Course;
using MaktabDataContracts.Responses.Course;

namespace Maktab.Domain.Services
{
     public class CourseService : BaseService, ICourseService
     {
          private readonly List<CourseResponse> _Courses;
          private Guid iccSchoolId = new Guid("FDCA4C86-DF0E-4CCC-BCE7-D4AE62F6E337");
          private Guid iccSchool2Id = new Guid("5AE84751-B58B-44D8-AC30-08F28E32BF15");


          public CourseService(IHttpService httpService, ILocalStorageService localStorageService)
          : base(httpService, localStorageService)
          {
               _Courses = GetCourses( new List<Guid>() { iccSchoolId, iccSchool2Id });

          }

          public async Task<IEnumerable<CourseResponse>> GetCoursesByInstitutionIdAsync(Guid institutionId)
          {
               return await Task.FromResult(_Courses);
          }

          public async Task<CourseResponse> GetCourseByIdAsync(Guid courseId)
          {
               return _Courses.Find(x => x.CourseId == courseId);
          }

          private List<CourseResponse> GetCourses(IEnumerable<Guid> institutionIds)
          {
               var courses = new List<CourseResponse>()
               {
               new CourseResponse
            {
                CourseId = Guid.NewGuid(),
                InstituteId = institutionIds.First(),
                Name = "Introduction to Islamic Studies",
                NameFr = "Introduction aux études islamiques",
                IsActive = true,
                
                //Category = "Islamic Studies",
                Description = "Learn the foundations of Islamic beliefs, practices, and history.",
                DescriptionFr = "Apprenez les fondements des croyances, des pratiques et de l'histoire islamiques.",
                //ImageUrl = "images/courses/islamic-studies.jpg",
                //Instructor = "Dr. Abdullah Al-Hassan",
                //Modules = new List<string>
                //{
                //    "Overview of Islamic Beliefs",
                //    "Pillars of Islam",
                //    "Islamic History & Civilization",
                //    "Contemporary Issues in Islam"
                //}
            },
            new CourseResponse
            {
                  CourseId = Guid.NewGuid(),
                InstituteId = institutionIds.First(),
                Name = "Quran Recitation (Tajweed)",
                NameFr = "Récitation du Coran (Tajwid)",
                IsActive = true,
                //Category = "Quran",
                Description = "Master the art of Qur’an recitation with Tajweed rules.",
                DescriptionFr = "Maîtrisez l'art de la récitation du Coran avec les règles de Tajweed.",
                //ImageUrl = "images/courses/quran-recitation.jpg",
                //Instructor = "Shaykh Ahmad Khan",
                //Modules = new List<string>
                //{
                //    "Introduction to Tajweed",
                //    "Makhaarij (Articulation Points)",
                //    "Rules of Noon and Meem",
                //    "Practice & Recitation Sessions"
                //}
                
            },
            new CourseResponse
            {
                  CourseId = Guid.NewGuid(),
                InstituteId = institutionIds.Last(),
                Name = "Quran Tafseer (Interpretation)",
                NameFr = "Tafseer du Coran (Interprétation)",
                IsActive = true,
                //Category = "Quran",
                Description = "Study selected Surahs with detailed Tafseer and context.",
                DescriptionFr = "Étudiez les sourates sélectionnées avec un Tafseer détaillé et un contexte.",
                //ImageUrl = "images/courses/quran-tafseer.jpg",
                //Instructor = "Ustadh Fatimah Ali",
                //Modules = new List<string>
                //{
                //    "Introduction to Tafseer",
                //    "Tafseer of Surah Al-Fatiha",
                //    "Themes of Surah Al-Baqarah",
                //    "Understanding Makki vs Madani Surahs"
                //}
            },
            new CourseResponse
            {
                  CourseId = Guid.NewGuid(),
                InstituteId = institutionIds.Last(),
                Name = "Seerah of Prophet Muhammad (PBUH)",
                NameFr = "La Sira du Prophète Muhammad (PSL)",
                IsActive = true,
                //Category = "Islamic Studies",
                Description = "Explore the life, character, and mission of Prophet Muhammad (peace be upon him).",
                DescriptionFr = "Explorez la vie, le caractère et la mission du Prophète Muhammad (paix soit sur lui).",
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

          public Task<IEnumerable<CourseResponse>> GetAllCoursesAsync()
          {
               throw new NotImplementedException();
          }

          public Task<CourseResponse> AddInstitutionAsync(AddCourse addInstitute)
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
