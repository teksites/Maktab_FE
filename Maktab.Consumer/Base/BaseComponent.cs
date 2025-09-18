using Maktab.Consumer.Helpers;
using Maktab.Core.Interfaces.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.RegularExpressions;

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
          protected void RequestRender(int delayMs = 100)
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
                    Snackbar.Add(_errorMessage, Severity.Error);
                    NavigationManager.NavigateTo("/account/logout");
               }
               catch (Exception)
               {
                    _errorMessage = "System was not able to complete your request. Please try again later in a moment.";
                    Snackbar.Add(_errorMessage, Severity.Error);
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
               if (string.IsNullOrWhiteSpace(pw))
               {
                    yield return "Password is required!";
                    yield break;
               }
               if (pw.Length < 8)
                    yield return "Password must be at least 8 characters long.";
               if (!Regex.IsMatch(pw, @"[A-Z]"))
                    yield return "Password must contain at least one uppercase letter.";
               if (!Regex.IsMatch(pw, @"[a-z]"))
                    yield return "Password must contain at least one lowercase letter.";
               if (!Regex.IsMatch(pw, @"[0-9]"))
                    yield return "Password must contain at least one digit.";
          }

          

          
     }
}
