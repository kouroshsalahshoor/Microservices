using Shared;
using Shared.Front;

namespace BlazorWasm.Services.IServices
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto dto);
        Task Logout();
    }
}
