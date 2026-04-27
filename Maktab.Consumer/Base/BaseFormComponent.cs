using MudBlazor;

namespace Maktab.Consumer.Base
{
     public abstract class BaseFormComponent<T> : LocalizeableBaseComponent<T> where T : class
     {
          protected MudForm _form;
          protected bool _isValid;

          protected virtual void OnFormValidated()
          {
               // Clear error message when form is valid
               if (_form?.IsValid == true)
               {
                    _errorMessage = string.Empty;
               }
          }
     }
}
