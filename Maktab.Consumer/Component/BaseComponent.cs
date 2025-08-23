using Maktab.Core.Interfaces.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.RegularExpressions;

namespace Maktab.Consumer.Component
{
     public abstract class BaseComponent<T> : ComponentBase
     {
          [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
          [Inject] protected ISnackbar Snackbar { get; set; } = default!;
          //[Inject] protected IStringLocalizer<T> L { get; set; } = default!;
          [Inject] protected ILogger<T> Logger { get; set; } = default!;
          [Inject] protected ISessionService SessionService { get; set; } = default!;



          [Parameter] public bool Loading { get; set; }
          [Parameter] public bool IsBusy { get; set; }
          [Parameter] public bool Validating { get; set; }

          protected string errorMessage = string.Empty;

          protected void ShowError(string message)
          {
               Snackbar.Add(message, Severity.Error);
          }


          protected void ShowSuccess(string message)
          {
               Snackbar.Add(message, Severity.Success);
          }

          protected override Task OnInitializedAsync()
          {
               Logger.LogInformation("{Component} initialized", typeof(T).Name);
               return base.OnInitializedAsync();
          }

          protected async Task InitiateUserAction( Func<Task> userAction)
          {
               try
               {
                    IsBusy = true;

                    await userAction();
               }
               catch (UnauthorizedAccessException)
               {
                    errorMessage = "Please provide valid credentials.";
                    Snackbar.Add(errorMessage, Severity.Error);
                    NavigationManager.NavigateTo("/account/logout");
               }
               catch (Exception)
               {
                    errorMessage = "System was not able to complete your request. Please try again later in a moment.";
                    Snackbar.Add(errorMessage, Severity.Error);
               }
               finally
               {
                    IsBusy = false;
               }
          }

          protected IEnumerable<string> PasswordStrength(string pw)
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
