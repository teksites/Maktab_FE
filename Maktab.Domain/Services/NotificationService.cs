using Maktab.Core.Interfaces.Services;
using Maktab.Models.Models;

namespace Maktab.Domain.Services
{
     public class NotificationService : BaseService, INotificationService
     {
          public NotificationService(IHttpService httpService, ILocalStorageService localStorageService) 
          : base(httpService, localStorageService)
          {
          }


          public Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid id)
          {
               IEnumerable<Notification> notifications = GetNotifications();

               return Task.FromResult(notifications);
          }

          private static List<Notification> GetNotifications()
          {
               return new List<Notification>()
               {
               new Notification { Message = "Quran class rescheduled", Timestamp = DateTime.Now.AddDays(-1) },
               new Notification { Message = "New Islamic Studies course available", Timestamp = DateTime.Now.AddDays(-2) }
               };
          }
     }
}
