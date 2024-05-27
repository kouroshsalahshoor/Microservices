namespace EmailApi.Services.IServices
{
    public interface IEmailLoggerService
    {
        Task RegisterUser(string email);
    }
}
