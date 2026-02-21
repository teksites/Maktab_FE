namespace Maktab.Consumer.Helpers
{
     public static class Constants
     {
          public const string LoginRoute = "/account/login";
          public const string LogoutRoute = "/account/logout";
          public const string CodeValidationRoute = "/account/usercodevalidation";
          public const string ForgotPasswordRoute = "/account/forgotpassword";

          public const string ParentDashboardRoute = "/parent/dashboard";
          public const string AddParentProfileDetailRoute = "/parent/adddetails";
          public const string RegisterParentRoute = "/parent/register";
          public const string ParentProfileRoute = "/parent/profile";

          public const string AppLandingPage = "/landing/mainpage";

          public const string CourseDetailsRoute = "/courses/{0}";
          public const string CourseSearchRoute = "/courses/search";

          public const string ChildCourseAssignmentRoute = "/child/assign-course";

          public const string PaymentDetailsRoute = "/payment/payment-details";
          public const string PaymentInfoRoute = "/payment/payment-info";
     }
}
