using Shared;
using Shared.Dtos.Auth;

namespace AuthApi.Services.IServices
{
    public interface IAuthService
    {
        Task<ResponseDto> Register(RegisterDto dto);
        Task<LoginResponseDto> Login(LoginDto dto);
        Task<ResponseDto> AssignToRole(string userName, string role);
        Task<ResponseDto> UpdateUser(UserDto dto);
    }
}
