using HomeFinance.Domain.Utils;

class UserService : IUserService
{
    public string UserId { get; }

    public UserService(string userId)
    {
        UserId = userId;
    }
}
