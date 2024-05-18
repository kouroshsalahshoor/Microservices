using Shared;
using Shared.Dtos;
using Shared.Front;

namespace BlazorWasm.Services.IServices
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> Get()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Get,
                Url = ApplicationConstants.ProductApi
            });
        }
        public async Task<ResponseDto?> Get(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Get,
                Url = ApplicationConstants.ProductApi + id.ToString()
            });
        }
        public async Task<ResponseDto?> Get(string category)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Get,
                Url = ApplicationConstants.ProductApi + $"category/{category}"
            });
        }
        public async Task<ResponseDto?> Create(ProductDto dto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Post,
                Url = ApplicationConstants.ProductApi,
                Data = dto
            });
        }
        public async Task<ResponseDto?> Update(ProductDto dto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Put,
                Url = ApplicationConstants.ProductApi,
                Data = dto
            });
        }
        public async Task<ResponseDto?> Delete(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Delete,
                Url = ApplicationConstants.ProductApi + id.ToString()
            });
        }
    }
}
