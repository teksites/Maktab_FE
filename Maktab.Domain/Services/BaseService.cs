using Maktab.Core.Interfaces.Services;
using System.Web;

namespace Maktab.Domain.Services
{
     public abstract class BaseService
     {
          protected IHttpService _httpService;
          protected ILocalStorageService _localStorageService;


          public BaseService(IHttpService httpService, ILocalStorageService localStorageService)
          {
               _httpService = httpService;
               _localStorageService = localStorageService;
          }


          protected static string AddQueryParameter(string relativeUrl, string paramName, string paramValue)
          {
               // Use UriBuilder to handle the URL
               var uriBuilder = new UriBuilder("http://dummybase" + relativeUrl); // Add a dummy base for UriBuilder to work
               var query = HttpUtility.ParseQueryString(uriBuilder.Query);

               // Add or update the query parameter
               query[paramName] = paramValue;

               // Update the query in the UriBuilder
               uriBuilder.Query = query.ToString();

               // Remove the dummy base and return the relative URL
               return uriBuilder.Path + (string.IsNullOrEmpty(uriBuilder.Query) ? "" : "?" + uriBuilder.Query);
          }
     }
}
