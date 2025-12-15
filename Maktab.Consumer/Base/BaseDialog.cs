using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Maktab.Consumer.Base
{
     public abstract class BaseDialog<T> : LocalizeableBaseComponent<T> where T : class
     {
          [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = default!;
     }
}
