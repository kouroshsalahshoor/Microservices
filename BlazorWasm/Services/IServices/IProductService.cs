using Shared;
using Shared.Dtos;

namespace BlazorWasm.Services.IServices
{
    public interface IProductService
    {
        Task<ResponseDto?> Get();
        Task<ResponseDto?> Get(int id);
        Task<ResponseDto?> Get(string category);
        Task<ResponseDto?> Create(ProductDto dto);
        Task<ResponseDto?> Update(ProductDto dto);
        Task<ResponseDto?> Delete(int id);
    }
}
