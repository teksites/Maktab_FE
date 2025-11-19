using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Maktab.Consumer.Base
{
     public class BaseDialogWithButtons<T> : BaseDialog<T> where T : class
     {
          [Parameter] public RenderFragment? DialogBody { get; set; }

          [Parameter] public EventCallback OnSave { get; set; }
          [Parameter] public EventCallback OnCancel { get; set; }

          [Parameter] public string SaveText { get; set; } = "Save";
          [Parameter] public string CancelText { get; set; } = "Cancel";
          [Parameter] public bool ShowActions { get; set; } = true;

          protected virtual async Task OnSaveClickedAsync()
          {
               if (OnSave.HasDelegate)
                    await OnSave.InvokeAsync();

               MudDialog.Close(DialogResult.Ok(true));
          }

          protected virtual async Task OnCancelClickedAsync()
          {
               if (OnCancel.HasDelegate)
                    await OnCancel.InvokeAsync();

               MudDialog.Cancel();
          }
     }
}
