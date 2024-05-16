using AuthApi.Models;

namespace AuthApi.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser user, List<string> roles);
    }
}
