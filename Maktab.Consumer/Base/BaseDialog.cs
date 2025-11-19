using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Maktab.Consumer.Base
{
     public class BaseDialog<T> : BaseComponent<T> where T : class
     {
          [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = default!;
     }
}
