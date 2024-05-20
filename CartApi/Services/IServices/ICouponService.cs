using Shared.Dtos;

namespace CartApi.Services.IServices
{
    public interface ICouponService
    {
        Task<CouponDto> Get(string code);
    }
}
