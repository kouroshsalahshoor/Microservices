using Shared;

namespace BlazorAuto.Client.Services.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto dto);
    }
}
