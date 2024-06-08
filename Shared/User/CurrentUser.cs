namespace Shared.User;

public record CurrentUser(string Id, string UserName, string Email, string Phone, string FirstName, string LastName, List<string> Roles)
{
    public bool IsInRole(string role) => Roles.Contains(role);
}
