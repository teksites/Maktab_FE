namespace Maktab.Models.Models
{
     public class User : BaseEntity
     {
          public string Id { get; set; }
          public string FirstName { get; set; }
          public string LastName { get; set; }
          public string Username { get; set; }
          public string Token { get; set; }
          public bool IsDeleting { get; set; }
     }
}
