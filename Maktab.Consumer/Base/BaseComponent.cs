using Maktab.Consumer.Dialogs;
using Maktab.Consumer.Helpers;
using Maktab.Core.Interfaces.Services;
using MaktabDataContracts.Enums;
using MaktabDataContracts.Responses.Children;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Maktab.Consumer.Base
{
     public abstract class BaseComponent<T> : ComponentBase, IDisposable where T : class
     {
          [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
          [Inject] protected ISnackbar Snackbar { get; set; } = default!;
          [Inject] protected ILogger<T> Logger { get; set; } = default!;
          [Inject] protected ISessionService SessionService { get; set; } = default!;

          [Parameter] public bool Loading { get; set; }
          [Parameter] public bool IsBusy { get; set; }
          [Parameter] public bool Validating { get; set; }
          protected bool IsDisposed { get; private set; }

          protected string _errorMessage = string.Empty;

          private RenderThrottle _throttle;
          private readonly CancellationTokenSource _cts = new();

          protected override void OnInitialized()
          {
               _throttle = new RenderThrottle(() => InvokeAsync(StateHasChanged));
               base.OnInitialized();
          }

          /// <summary>
          /// Request a throttled re-render of the component.
          /// </summary>
          protected void RequestRender(int delayMs = 250)
          {
               if (IsDisposed)
               {
                    return;
               }

               _throttle.RequestRender(delayMs);
          }

          /// <summary>
          /// Safely run async tasks tied to component lifecycle.
          /// Automatically cancels if component is disposed.
          /// </summary>
          protected async Task RunSafeAsync(Func<CancellationToken, Task> action)
          {
               if (IsDisposed)
               {
                    return;
               }

               try
               {
                    await action(_cts.Token);
               }
               catch (OperationCanceledException)
               {
                    Logger.LogDebug("Operation was canceled in {Component}", typeof(T).Name);
               }
               catch (Exception ex)
               {
                    Logger.LogError(ex, "Unhandled exception in {Component}", typeof(T).Name);
                    Snackbar.Add("An unexpected error occurred.", Severity.Error);
               }
          }

          /// <summary>
          /// Runs a periodic async task safely until the component is disposed.
          /// </summary>
          protected async Task RunSafePeriodicAsync(
              Func<CancellationToken, Task> action,
              TimeSpan interval,
              bool runImmediately = true)
          {
               if (IsDisposed)
               {
                    return;
               }

               try
               {
                    if (runImmediately)
                         await action(_cts.Token);

                    while (!_cts.Token.IsCancellationRequested)
                    {
                         await Task.Delay(interval, _cts.Token);
                         if (!_cts.Token.IsCancellationRequested)
                              await action(_cts.Token);
                    }
               }
               catch (OperationCanceledException)
               {
                    Logger.LogDebug("Periodic task was canceled in {Component}", typeof(T).Name);
               }
               catch (Exception ex)
               {
                    Logger.LogError(ex, "Error in periodic task in {Component}", typeof(T).Name);
                    Snackbar.Add("Background task failed.", Severity.Warning);
               }
          }

          /// <summary>
          /// Clean up resources when component is disposed.
          /// </summary>
          public void Dispose()
          {
               IsDisposed = true;
               _cts.Cancel();
               _cts.Dispose();
          }


          protected virtual void ShowError(string message)
          {
               if (IsDisposed)
               {
                    return;
               }

               Snackbar.Add(message, Severity.Error);
          }

          protected virtual void ShowSuccess(string message)
          {
               if (IsDisposed)
               {
                    return;
               }

               Snackbar.Add(message, Severity.Success);
          }

          protected virtual void ShowInformation(string message)
          {
               if (IsDisposed)
               {
                    return;
               }

               Snackbar.Add(message, Severity.Info);
          }

          protected virtual void ShowWarning(string message)
          {
               if (IsDisposed)
               {
                    return;
               }

               Snackbar.Add(message, Severity.Warning);
          }

          protected override Task OnInitializedAsync()
          {
               Logger.LogInformation("{Component} initialized", typeof(T).Name);
               return base.OnInitializedAsync();
          }

          protected virtual async Task InvokeUserAction( Func<Task> userAction)
          {
               if (IsDisposed)
               {
                    return;
               }

               try
               {
                    IsBusy = true;
                    _cts.Token.ThrowIfCancellationRequested();
                    RequestRender(50);

                    await userAction();
                    ////await RunSafeAsync(async (a) =>
                    ////{
                    ////     a.ThrowIfCancellationRequested();
                    ////     await userAction();
                    ////});

                    //await RunSafeAsync(async token =>
                    //{
                    //     await userAction();
                    //});
               }
               catch (OperationCanceledException)
               {
                    Logger.LogDebug("Operation was canceled in {Component}", typeof(T).Name);
               }
               catch (UnauthorizedAccessException)
               {
                    _errorMessage = "Please provide valid credentials.";
                    NavigationManager.NavigateTo(Constants.LogoutRoute);
               }
               catch (Exception ex)
               {
                    _errorMessage = "System was not able to complete your request. Please try again later in a moment.";
                    Snackbar.Add(_errorMessage, Severity.Error);
                    Logger.LogError("Exception in {Component}: {Error}", typeof(T).Name, ex);
               }
               finally
               {
                    IsBusy = false;
               }
          }

          protected virtual Task HandlerUserAction(ref bool flag, Func<Task> userAction)
          {
               if (IsDisposed)
               {
                    return Task.CompletedTask;
               }

               try
               {
                    flag = true;
                    RequestRender(50);
                    return InvokeUserAction(userAction);
               }
               finally
               {
                    flag = false;
                    RequestRender(50);
               }
          }

          protected virtual IEnumerable<string> PasswordStrength(string pw)
          {
               return ValidationHelper.PasswordStrength(pw);
          }

          protected virtual IEnumerable<string> ValidateCanadianIndividualTaxCode(string sin)
          {
               return ValidationHelper.ValidateCanadianIndividualTaxCode(sin);
          }

          protected async Task<DialogResult?> OpenAddChildDialog(IDialogService dialogService, Guid familyId)
          {
               var parameters = new DialogParameters { ["FamilyId"] = familyId };
               var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true };

               var dialog = await dialogService.ShowAsync<AddChildDialog>(null, parameters, options);
               var result = await dialog.Result;
               return result;
          }

          protected async Task<DialogResult?> OpenEditChildDialog(IDialogService dialogService, ChildResponse childResponse)
          {
               var parameters = new DialogParameters { ["Child"] = childResponse };
               var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true };

               var dialog = await dialogService.ShowAsync<EditChildDialog>(null, parameters, options);
               var result = await dialog.Result;
               return result;
          }

          protected async Task<DialogResult?> OpenAddAddressDialog(IDialogService dialogService, Guid connectedId)
          {
               var parameters = new DialogParameters { ["ConnectedId"] = connectedId };
               var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = false, CloseButton = true };

               var dialog = await dialogService.ShowAsync<AddAddressDialog>("Add Address", parameters, options);
               var result = await dialog.Result;

               return result;
          }

          protected async Task<DialogResult?> OpenAddContactDialog(IDialogService dialogService, Guid familyId)
          {
               var parameters = new DialogParameters { ["FamilyId"] = familyId };
               var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = false, CloseButton = true };

               var dialog = await dialogService.ShowAsync<AddContactDialog>("Add Contact", parameters, options);
               var result = await dialog.Result;
               return result;
          }

          protected Color GetEnrollmentStatusColor(EnrollmentStatus enrollmentStatus)
          {
               switch (enrollmentStatus)
               {
                    case EnrollmentStatus.Awaiting:
                         return Color.Warning;
                    case EnrollmentStatus.Enrolled:
                         return Color.Info;
                    case EnrollmentStatus.Registered:
                         return Color.Success;
                    case EnrollmentStatus.Refunded:
                         return Color.Tertiary;
                    case EnrollmentStatus.Cancelled:
                         return Color.Error;
                    default:
                         return Color.Default;
               }
          }
     }
}
