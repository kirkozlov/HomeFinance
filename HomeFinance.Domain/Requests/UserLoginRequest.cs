namespace HomeFinance.Domain.Requests;

public class UserLoginRequest
{
    public string UserNameOrEmail { get; set; }
    public string Password { get; set; }
}

public class UserLoginResponse
{
    public string Token { get; set; }
}