using BlazorAuto.Client.Services.IService;
using Shared;
using Shared.Dtos;

namespace BlazorAuto.Client.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> Get()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Get,
                Url = ApplicationConstants.CouponApi
            });
        }
        public async Task<ResponseDto?> Get(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Get,
                Url = ApplicationConstants.CouponApi + id.ToString()
            });
        }
        public async Task<ResponseDto?> Get(string code)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Get,
                Url = ApplicationConstants.CouponApi + $"getbycode/{code}"
            });
        }
        public async Task<ResponseDto?> Create(CouponDto dto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Post,
                Url = ApplicationConstants.CouponApi,
                Data = dto
            });
        }
        public async Task<ResponseDto?> Update(CouponDto dto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Put,
                Url = ApplicationConstants.CouponApi,
                Data = dto
            });
        }
        public async Task<ResponseDto?> Delete(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Delete,
                Url = ApplicationConstants.CouponApi + id.ToString()
            });
        }
    }
}
