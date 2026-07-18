using Maktab.Consumer.Dialogs;
using Maktab.Consumer.Localization;
using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Responses.Users;
using MudBlazor;

namespace Maktab.Consumer.Base
{
     public abstract class BaseParentComponent<T> : LocalizeableBaseComponent<T>, IDisposable where T : class
     {
          protected bool IsEnrollmentPreValidationProcessing { get; private set; } = false;

          protected async Task<bool> PreEnrollmentValidation(IUserService userService, IDialogService dialogService, UserInformationResponse userInfo)
          {
               try
               {
                    IsEnrollmentPreValidationProcessing = true;

                    bool hasValidFamilyDetails = await HasValidFamilyDetails(userService, dialogService, userInfo);
                    return hasValidFamilyDetails;
               }
               catch (Exception ex)
               {
                    await ShowMessagePromptAsync(
                         dialogService,
                         L[MaktabResources.Error],
                         L[MaktabResources.Msg_Error_Validating_Family_Details]);
                    return false;
               }
               finally
               {
                    IsEnrollmentPreValidationProcessing = false;
               }
          }
     }
}
