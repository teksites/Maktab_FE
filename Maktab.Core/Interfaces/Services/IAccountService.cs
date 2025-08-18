
using MaktabDataContracts.Requests.Users;
using MaktabDataContracts.Responses.Users;

namespace Maktab.Core.Interfaces.Services
{
     public interface IAccountService : IDomainService
     {
          //User User { get; }
          Task Initialize();


          //Task Login(Login model);
          //Task Logout();
          //Task Register(AddUser model);
          //Task<IList<User>> GetAll();
          //Task<UserInformationResponse> GetById(Guid id);
          //Task Update(string id, EditUser model);
          //Task Delete(string id);
     }
}
