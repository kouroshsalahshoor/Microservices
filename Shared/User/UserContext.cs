using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Shared.User;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser? GetCurrentUser()
    {
        var user = httpContextAccessor?.HttpContext?.User;
        if (user == null)
        {
            throw new InvalidOperationException("User Context is not present");
        }

        if (user.Identity == null || user.Identity.IsAuthenticated == false)
        {
            return null;
        }

        var userId = user.FindFirst(x => x.Type == ClaimTypes.Sid)!.Value;
        //var userId2 = user.FindFirst(x=> x.Type == ClaimTypes.NameIdentifier)!.Value;
        var userName = user.FindFirst(x => x.Type == ClaimTypes.Name)!.Value;
        var email = user.FindFirst(x => x.Type == ClaimTypes.Email)!.Value;
        var phone = user.FindFirst(x => x.Type.ToLower() == "phone")!.Value;
        var firstName = user.FindFirst(x => x.Type.ToLower() == "firstname")!.Value;
        var lastName = user.FindFirst(x => x.Type.ToLower() == "lastname")!.Value;
        var roles = user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();

        return new CurrentUser(userId, userName, email, phone, firstName, lastName, roles);
    }
}
