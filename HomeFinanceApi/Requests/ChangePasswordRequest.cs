namespace HomeFinanceApi.Requests;

public class ChangePasswordRequest
{
    public string UserNameOrEmail { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}