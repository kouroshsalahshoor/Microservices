using Shared;
using Shared.Dtos.Cart;

namespace BlazorWasm.Services.IServices
{
    public interface ICartService
    {
        Task<ResponseDto?> Get(string userId);
        Task<ResponseDto?> AddEdit(CartDto dto);
        Task<ResponseDto?> Remove(int cartDetailId);
        Task<ResponseDto?> ApplyCoupon(CartDto dto);
        Task<ResponseDto?> EmailCart(CartDto dto);
    }
}
