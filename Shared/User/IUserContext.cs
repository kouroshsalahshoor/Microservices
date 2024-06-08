namespace Shared.User
{
    public interface IUserContext
    {
        CurrentUser? GetCurrentUser();
    }
}