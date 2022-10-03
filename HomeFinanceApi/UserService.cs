using System.Security.Claims;

namespace HomeFinanceApi;

public interface IUserService
{
    string UserId { get; }
}

public class UserService: IUserService
{
    public HttpContext? HttpContext { get; set; }
    public ClaimsPrincipal User => HttpContext!.User;
    public string UserId=> User.Claims.First(i => i.Type == "UserId").Value;
    public UserService(IHttpContextAccessor httpContext)
    {
        this.HttpContext= httpContext.HttpContext;
    }
}