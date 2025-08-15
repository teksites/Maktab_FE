namespace Maktab.Core.Interfaces.Models
{
     public interface ISessionInfo
     {
          string AccessToken { get; } 
          string RefreshToken { get; }

          Guid UserId { get; }
          public Guid SessionId { get; }


          DateTime LoginTime { get;  }
          DateTime ExpiresIn { get; }
     }
}
