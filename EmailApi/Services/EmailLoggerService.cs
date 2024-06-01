using EmailApi.Data;
using EmailApi.Models;
using EmailApi.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.Cart;
using System.Text;

namespace EmailApi.Services
{
    public class EmailLoggerService : IEmailLoggerService
    {
        private DbContextOptions<ApplicationDbContext> _dbOptions;

        public EmailLoggerService(DbContextOptions<ApplicationDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task Cart(CartDto dto)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<br/>Cart Email:");
            stringBuilder.AppendLine("<br/>Total: " + dto.CartHeader.Total);
            stringBuilder.AppendLine("<br/>");
            stringBuilder.AppendLine("<ul>");
            foreach (var item in dto.CartDetails!)
            {
            stringBuilder.AppendLine("<li>");
            stringBuilder.AppendLine(item.Product!.Name + " x " + item.Count);
            stringBuilder.AppendLine("</li>");
            }
            stringBuilder.AppendLine("</ul>");

            await save(stringBuilder.ToString(), dto.CartHeader.Email);
        }
        public async Task RegisterUser(string email)
        {
            string message = "User Registeration Successful. <br/> Email : " + email;
            await save(message, "admin@x.x");
        }

        private async Task<bool> save(string message, string email)
        {
            try
            {
                EmailLog emailLog = new()
                {
                    Email = email,
                    Sent = DateTime.Now,
                    Message = message
                };
                await using var _db = new ApplicationDbContext(_dbOptions);
                await _db.EmailLogs.AddAsync(emailLog);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }
    }
}
