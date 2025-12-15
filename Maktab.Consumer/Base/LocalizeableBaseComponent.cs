using Maktab.Consumer.Localization;
using Maktab.Core.Interfaces.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Maktab.Consumer.Base
{
     public abstract class LocalizeableBaseComponent<T> : BaseComponent<T> where T : class
     {
          [Inject] protected IStringLocalizer<MaktabResources> L { get; set; }
          [Inject] protected IGlobalizationService GlobalizationService { get; set; }
     }
}
