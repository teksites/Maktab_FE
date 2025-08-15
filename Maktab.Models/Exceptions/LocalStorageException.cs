namespace Maktab.Models.Exceptions
{
     public class LocalStorageException : Exception
     {
          public LocalStorageException(string message, Exception innerException)
              : base(message, innerException) { }
     }
}
