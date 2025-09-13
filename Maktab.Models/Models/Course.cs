namespace Maktab.Models.Models
{
     public class Course
     {
          public Guid Id { get; set; }
          public string Title { get; set; } = string.Empty;
          public string Description { get; set; } = string.Empty;
          public string Category { get; set; } = string.Empty;
          public string ImageUrl { get; set; } = string.Empty;

          public IEnumerable<string> Modules { get; set; } = Array.Empty<string>();

          public string Instructor { get; set; } = string.Empty;
     }
}
