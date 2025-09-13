using Maktab.Core.Interfaces.Services;
using Maktab.Models.Models;
using MaktabDataContracts.Models;
using MaktabDataContracts.Responses.Children;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maktab.Domain.Services
{
     public class InstitutionService : BaseService, IInstitutionService
     {
          private readonly List<Course> _Courses;


          public InstitutionService(
                   IHttpService httpService,
                   ILocalStorageService localStorageService)
               : base(httpService, localStorageService)
          {
               _Courses = GetCourses();
          }

          public async Task<IEnumerable<Institution>> GeInstitutionByIdAsync(Guid institutionId)
          {
               var institutions = new List<Institution>()
               {
                    new Institution { Id = institutionId, Name = "ICC", Type="School" },
               };

               return institutions;
          }

          public async Task<IEnumerable<Course>> GeCoursesByInstitutionIdAsync(Guid institutionId)
          {
               return _Courses;
          }

          public async Task<Course> GetCourseByIdAsync(Guid courseId)
          {
               return _Courses.Find(x => x.Id == courseId);
          }

          private List<Course> GetCourses()
          {
               var courses = new List<Course>()
            {
                         new Course
                      {
                          Id = Guid.NewGuid(),
                          Title = "Introduction to Islamic Studies",
                          Category = "Islamic Studies",
                          Description = "Learn the foundations of Islamic beliefs, practices, and history.",
                          ImageUrl = "images/courses/islamic-studies.jpg",
                          Modules = new List<string>
                          {
                              "Overview of Islamic Beliefs",
                              "Pillars of Islam",
                              "Islamic History & Civilization",
                              "Contemporary Issues in Islam"
                          }
                      },
                      new Course
                      {
                          Id = Guid.NewGuid(),
                          Title = "Qur’an Recitation (Tajweed)",
                          Category = "Qur’an",
                          Description = "Master the art of Qur’an recitation with Tajweed rules.",
                          ImageUrl = "images/courses/quran-recitation.jpg",
                          Modules = new List<string>
                          {
                              "Introduction to Tajweed",
                              "Makhaarij (Articulation Points)",
                              "Rules of Noon and Meem",
                              "Practice & Recitation Sessions"
                          }
                      },
                      new Course
                      {
                          Id = Guid.NewGuid(),
                          Title = "Qur’an Tafseer (Interpretation)",
                          Category = "Qur’an",
                          Description = "Study selected Surahs with detailed Tafseer and context.",
                          ImageUrl = "images/courses/quran-tafseer.jpg",
                          Modules = new List<string>
                          {
                              "Introduction to Tafseer",
                              "Tafseer of Surah Al-Fatiha",
                              "Themes of Surah Al-Baqarah",
                              "Understanding Makki vs Madani Surahs"
                          }
                      },
            new Course
            {
                Id = Guid.NewGuid(),
                Title = "Seerah of Prophet Muhammad (PBUH)",
                Category = "Islamic Studies",
                Description = "Explore the life, character, and mission of Prophet Muhammad (peace be upon him).",
                ImageUrl = "images/courses/seerah.jpg",
                Modules = new List<string>
                {
                    "Early Life in Makkah",
                    "Prophethood & Revelation",
                    "Migration to Madinah",
                    "Key Battles and Lessons"
                }
            }
        };
               return courses;
          }
     }
}
