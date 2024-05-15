using Shared;
using Shared.Dtos.Auth;

namespace BlazorWasm.Services.IServices
{
    public interface IAuthService
    {
        Task<ResponseDto?> Register(RegisterDto dto);
        Task<ResponseDto?> Login(LoginDto dto);
        Task<ResponseDto?> AssignToRole(RegisterDto dto);
    }
}
