using System.Security.Claims;
using HomeFinance.Domain.Utils;

namespace HomeFinanceApi;

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