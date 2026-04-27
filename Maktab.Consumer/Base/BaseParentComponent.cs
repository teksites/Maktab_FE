using Maktab.Consumer.Dialogs;
using Maktab.Consumer.Localization;
using MudBlazor;

namespace Maktab.Consumer.Base
{
     public abstract class BaseParentComponent<T> : LocalizeableBaseComponent<T>, IDisposable where T : class
     {
          protected async Task<DialogResult?> OpenEnrollChildDialog(IDialogService dialogService)
          {
               var parameters = new DialogParameters { };
               var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true };

               var dialog = await dialogService.ShowAsync<EnrollChildInCourseDialog>(L[MaktabResources.Enroll_Child], parameters, options);
               var result = await dialog.Result;

               return result;
               
          }
     }
}
