using Maktab.Core.Interfaces.Services;
using Maktab.Infrastructure.Converters;
using Maktab.Infrastructure.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Maktab.Infrastructure.Services
{
     public class HttpService : IHttpService
     {
          private HttpClient _httpClient;
          private NavigationManager _navigationManager;
          private ILocalStorageService _localStorageService;
          private IConfiguration _configuration;
          private JsonSerializerOptions _serializerOptions;

          public HttpService(
              HttpClient httpClient,
              NavigationManager navigationManager,
              ILocalStorageService localStorageService,
              IConfiguration configuration)
          {
               _httpClient = httpClient;
               _navigationManager = navigationManager;
               _localStorageService = localStorageService;
               _configuration = configuration;

               _serializerOptions = new JsonSerializerOptions
               {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true,
               };

               _serializerOptions.Converters.Add(new JsonStringEnumConverter());
               _serializerOptions.Converters.Add(new BoolConverter());
          }

          public async Task<T> Get<T>(string uri)
          {
               var request = createRequest(HttpMethod.Get, uri);
               return await sendRequest<T>(request);
          }

          public async Task Post(string uri, object value)
          {
               var request = createRequest(HttpMethod.Post, uri, value);
               await sendRequest(request);
          }

          public async Task<T> Post<T>(string uri, object? value = null, bool autoLogout = true)
          {
               var request = createRequest(HttpMethod.Post, uri, value);
               return await sendRequest<T>(request, autoLogout);
          }

          public async Task Put(string uri, object value = null, bool autoLogout = true)
          {
               var request = createRequest(HttpMethod.Put, uri, value);
               await sendRequest(request, autoLogout);
          }

          public async Task<T> Put<T>(string uri, object value)
          {
               var request = createRequest(HttpMethod.Put, uri);
               return await sendRequest<T>(request);
          }

          public async Task Delete(string uri)
          {
               var request = createRequest(HttpMethod.Delete, uri);
               await sendRequest(request);
          }

          public async Task<T> Delete<T>(string uri)
          {
               var request = createRequest(HttpMethod.Delete, uri);
               return await sendRequest<T>(request);
          }

          // helper methods

          private HttpRequestMessage createRequest(HttpMethod method, string uri, object? value = null)
          {
               var request = new HttpRequestMessage(method, uri);
               if (value != null)
                    request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
               return request;
          }

          private async Task sendRequest(HttpRequestMessage request, bool autoLogout = true, CancellationToken cancellationToken = default)
          {
               await addJwtHeader(request);
               await addSessionHeaderInfo(request);

               // send request
               using var response = await _httpClient.SendAsync(request, cancellationToken);

               // auto logout on 401 response
               if (autoLogout && response.StatusCode == HttpStatusCode.Unauthorized)
               {
                    _navigationManager.NavigateTo("account/logout");
                    return;
               }

               await handleErrors(response);
          }

          private async Task<T> sendRequest<T>(HttpRequestMessage request, bool autoLogout = true, CancellationToken cancellationToken = default)
          {
               await addJwtHeader(request);

               await addSessionHeaderInfo(request);

               // send request
               using var response = await _httpClient.SendAsync(request, cancellationToken);

               // auto logout on 401 response
               if (autoLogout && response.StatusCode == HttpStatusCode.Unauthorized)
               {
                    _navigationManager.NavigateTo("account/logout");
                    return default;
               }

               if (autoLogout)
               {
                    await handleErrors(response);
               }

               return await response.Content.ReadFromJsonAsync<T>(_serializerOptions, cancellationToken);
          }

          private async Task addJwtHeader(HttpRequestMessage request)
          {
               // add jwt auth header if user is logged in and request is to the api url
               var token = await _localStorageService.GetItem<String>(Constants.AccessTokenKey, string.Empty);
               //var isApiUrl = !request.RequestUri.IsAbsoluteUri;
               if (!string.IsNullOrEmpty(token))
               {
                    //&& isApiUrl)
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
               }
          }

          private async Task addSessionHeaderInfo(HttpRequestMessage request)
          {
               var sessionId = await _localStorageService.GetItem<Guid>(Constants.SessionIdKey, Guid.Empty);
               if (sessionId != Guid.Empty)
               {
                    await addCustomHeaders(request, Constants.SessionIdKey, sessionId.ToString());
               }
          }

          private async Task addCustomHeaders(HttpRequestMessage request, string key, string value)
          {
               request.Headers.Add(key, value);
          }

          private async Task handleErrors(HttpResponseMessage response)
          {
               if(response.StatusCode == HttpStatusCode.Unauthorized)
               {
                    throw new UnauthorizedAccessException(response.ReasonPhrase);
               }
               // throw exception on error response
               if (!response.IsSuccessStatusCode)
               {
                    var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                    throw new Exception(error["message"]);
               }
          }
     }
}
