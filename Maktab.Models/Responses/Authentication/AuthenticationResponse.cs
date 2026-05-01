
namespace Maktab.Models.Responses.Authentication
{
     public class AuthenticationResponse
     {
          public string AccessToken { get; set; } = string.Empty;
          public string RefreshToken { get; set; } = string.Empty;

          public Guid UserId { get; set; }
          public Guid SessionId { get; set; }

          public Guid FamilyId { get; set; }


          public DateTime LoginTime { get; set; }
          public DateTime ExpiresIn { get; set; }
     }
}
