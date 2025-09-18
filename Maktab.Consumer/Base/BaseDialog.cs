using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Maktab.Consumer.Base
{
     public class BaseDialog<T> : BaseComponent<T> where T : class
     {
          [CascadingParameter]
          protected IMudDialogInstance MudDialog { get; set; } = default!;

          [Parameter] public RenderFragment? DialogBody { get; set; }

          [Parameter] public EventCallback OnSave { get; set; }
          [Parameter] public EventCallback OnCancel { get; set; }

          [Parameter] public string SaveText { get; set; } = "Save";
          [Parameter] public string CancelText { get; set; } = "Cancel";
          [Parameter] public bool ShowActions { get; set; } = true;

          protected async Task OnSaveClicked()
          {
               if (OnSave.HasDelegate)
                    await OnSave.InvokeAsync();

               MudDialog.Close(DialogResult.Ok(true));
          }

          protected async Task OnCancelClicked()
          {
               if (OnCancel.HasDelegate)
                    await OnCancel.InvokeAsync();

               MudDialog.Cancel();
          }
     }
}
