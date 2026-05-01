using Maktab.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maktab.Core.Interfaces.Services
{
     public interface INotificationService : IDomainService
     {
          Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid id);
     }
}
