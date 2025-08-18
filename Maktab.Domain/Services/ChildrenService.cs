using Maktab.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maktab.Domain.Services
{
     public class ChildrenService : BaseService, IChildrenService
     {
          public ChildrenService(IHttpService httpService, ILocalStorageService localStorageService) 
          : base(httpService, localStorageService)
          {
          }
     }
}
