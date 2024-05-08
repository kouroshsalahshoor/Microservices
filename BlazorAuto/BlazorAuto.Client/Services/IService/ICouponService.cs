using Shared;
using Shared.Dtos;

namespace BlazorAuto.Client.Services.IService
{
    public interface ICouponService
    {
        Task<ResponseDto?> Get();
        Task<ResponseDto?> Get(int id);
        Task<ResponseDto?> Get(string code);
        Task<ResponseDto?> Create(CouponDto dto);
        Task<ResponseDto?> Update(CouponDto dto);
        Task<ResponseDto?> Delete(int id);
    }
}
