using Shared;
using Shared.Front;

namespace BlazorAuto.Client.Services.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto dto);
    }
}
