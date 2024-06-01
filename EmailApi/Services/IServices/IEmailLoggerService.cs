using Shared.Dtos.Cart;

namespace EmailApi.Services.IServices
{
    public interface IEmailLoggerService
    {
        Task Cart(CartDto dto);
        Task RegisterUser(string email);
    }
}
