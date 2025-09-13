using Maktab.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maktab.Core.Interfaces.Services
{
     public interface IInstitutionService : IDomainService
     {
          Task<IEnumerable<Course>> GeCoursesByInstitutionIdAsync(Guid institutionId);
          Task<IEnumerable<Institution>> GeInstitutionByIdAsync(Guid institutionId);
          Task<Course> GetCourseByIdAsync(Guid courseId);
     }
}
