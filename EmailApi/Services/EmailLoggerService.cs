using EmailApi.Data;
using EmailApi.Models;
using EmailApi.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace EmailApi.Services
{
    public class EmailLoggerService : IEmailLoggerService
    {
        private DbContextOptions<ApplicationDbContext> _dbOptions;

        public EmailLoggerService(DbContextOptions<ApplicationDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }
        public async Task RegisterUser(string email)
        {
            string message = "User Registeration Successful. <br/> Email : " + email;
            await saveAndEmail(message, "admin@x.x");
        }

        private async Task<bool> saveAndEmail(string message, string email)
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
                return false;
            }
        }
    }
}
