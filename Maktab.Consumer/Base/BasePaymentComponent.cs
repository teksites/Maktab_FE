using Maktab.Consumer.Dialogs;
using Maktab.Consumer.Localization;
using MaktabDataContracts.Responses.Course;
using MaktabDataContracts.Responses.Transactions;
using MudBlazor;
using static Maktab.Consumer.Pages.PaymentMethod.PaymentDetailPage;

namespace Maktab.Consumer.Base
{
     public abstract class BasePaymentComponent<T> : LocalizeableBaseComponent<T>
     where T : class
     {

          protected async Task OpenZeffyPaymentDialog(StudentCourseTransactionResponse courseTransaction, CourseResponseDetailed course, string userEmail)
          {
               const string url = "/PaymentMethod/Payment?amount={0}&code={1}&email={2}&course={3}";
               if (courseTransaction != null)
               {
                    string paymentCode = courseTransaction.PaymentCode;
                    decimal amountPayable = courseTransaction.TotalPayable - courseTransaction.TotalAmountPaid;

                    var navigationUrl = string.Format(url, amountPayable.ToString(), paymentCode, userEmail, GetCourseName(course));

                    //check if minimum amount due is greater than zero and less than total amount due, if so pass it as query param
                    decimal minimumPayable = CalculateMinumumAmountDue(courseTransaction);
                    if (minimumPayable > 0)
                    {
                         navigationUrl += $"&minimumAmount={minimumPayable.ToString()}";
                    }

                    NavigationManager.NavigateTo(navigationUrl, false);
               }
          }


          /// <summary>
          /// Calculates the minimum amount due for a given transaction.
          /// </summary>
          /// <param name="transaction">The transaction to calculate the minimum amount due for.</param>
          /// <returns>The minimum amount due.</returns>
          protected decimal CalculateMinumumAmountDue(StudentCourseTransactionResponse transaction)
          {
               decimal minimumAmount = transaction.MinimumPayable;
               if (minimumAmount <= 0)
               {
                    return 0;
               }

               return minimumAmount;
          }

          protected async Task OpenPaymentScheduleDialog(IDialogService dialogService, StudentCourseTransactionResponse studentCourseTransactionResponse)
          {
               if (studentCourseTransactionResponse == null)
               {
                    return;
               }

               var parameters = new DialogParameters { ["FeeInstallments"] = studentCourseTransactionResponse.FeeInstallments };
               var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = false, CloseOnEscapeKey = true, CloseButton = true };

               var dialog = await dialogService.ShowAsync<PaymentScheduleDialog>(L[MaktabResources.Payment_Schedule], parameters, options);
               var result = await dialog.Result;
          }


     }
}
