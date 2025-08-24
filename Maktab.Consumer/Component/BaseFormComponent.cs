using MudBlazor;

namespace Maktab.Consumer.Component
{
     public class BaseFormComponent<T> : BaseComponent<T> where T : class
     {
          protected MudForm _form;
          protected bool _isValid;

          protected void OnFormValidated()
          {
               // Clear error message when form is valid
               if (_form?.IsValid == true)
               {
                    _errorMessage = string.Empty;
               }
          }
     }
}
