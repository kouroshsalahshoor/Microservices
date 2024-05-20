using Shared.Dtos;

namespace CartApi.Services.IServices
{
    public interface IProductService
    {
        Task<List<ProductDto>> Get();
    }
}
