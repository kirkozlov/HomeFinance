namespace HomeFinanceApi.Requests;

public class UserLoginRequest
{
    public string UserNameOrEmail { get; set; }
    public string Password { get; set; }
}