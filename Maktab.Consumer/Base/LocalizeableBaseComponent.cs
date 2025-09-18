using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Maktab.Consumer.Base
{
     public class LocalizeableBaseComponent<T> : BaseComponent<T> where T : class
     {
          [Inject] protected IStringLocalizer<T> L { get; set; } = default!;
     }
}
