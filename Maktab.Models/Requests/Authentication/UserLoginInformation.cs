namespace Maktab.Models.Requests.Authentication
{
    public class UserLoginInformation
    {
        public string UserName { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
