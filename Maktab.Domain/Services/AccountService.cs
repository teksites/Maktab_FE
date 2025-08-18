using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Requests.Users;
using MaktabDataContracts.Responses.Users;
using Microsoft.AspNetCore.Components;

namespace Maktab.Domain.Services
{
     public class AccountService : BaseService, IAccountService
     {

          


          private NavigationManager _navigationManager;
          private string _userKey = "user";

          public AccountService(
              IHttpService httpService,
              NavigationManager navigationManager,
              ILocalStorageService localStorageService)
          : base(httpService, localStorageService)
          {
               _navigationManager = navigationManager;
          }





          public async Task Initialize()
          {
               //User = await _localStorageService.GetItem< Models.Models.User >(_userKey);
          }

          //public async Task Login(Login model)
          //{
          //     User = await _httpService.Post<Models.Models.User>("/users/authenticate", model);
          //     await _localStorageService.SetItem(_userKey, User);
          //}

          //public async Task Logout()
          //{
          //     //User = null;
          //     await _localStorageService.RemoveItem(_userKey);
          //     _navigationManager.NavigateTo("account/login");
          //}

          //public async Task Register(AddUser model)
          //{
          //     await _httpService.Post("/users/register", model);
          //}

          //public async Task<IList<Models.Models.User>> GetAll()
          //{
          //     return await _httpService.Get<IList<Models.Models.User>>("/users");
          //}

          //public async Task<Models.Models.User> GetById(string id)
          //{
          //     return await _httpService.Get<Models.Models.User>($"/users/{id}");
          //}

          //public async Task Update(string id, EditUser model)
          //{
          //     await _httpService.Put($"/users/{id}", model);

          //     // update stored user if the logged in user updated their own record
          //     if (id == User.Id)
          //     {
          //          // update local storage
          //          User.FirstName = model.FirstName;
          //          User.LastName = model.LastName;
          //          User.Username = model.Username;
          //          await _localStorageService.SetItem(_userKey, User);
          //     }
          //}

          public async Task Delete(string id)
          {
               await _httpService.Delete($"/users/{id}");

               // auto logout if the logged in user deleted their own record
               //if (id == User.Id)
               //     await Logout();
          }
     }
}
