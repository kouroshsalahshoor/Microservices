using AuthApi.Data;
using AuthApi.Models;
using AuthApi.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Dtos.Auth;

namespace AuthApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ResponseDto> Register(RegisterDto dto)
        {
            ApplicationUser user = new()
            {
                UserName = dto.UserName,
                Email = dto.Email,
                NormalizedUserName = dto.UserName.ToUpper(),
                NormalizedEmail = dto.Email.ToUpper(),
                PhoneNumber = dto.Phone,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            try
            {
                var result = await _userManager.CreateAsync(user, dto.Password);
                if (result.Succeeded)
                {
                    var userInDb = await _db.ApplicationUsers.FirstAsync(x => x.UserName == dto.UserName);
                    var userDto = new UserDto
                    {
                        Id = userInDb.Id,
                        UserName = userInDb.UserName!,
                        Email = userInDb.Email!,
                        Phone = userInDb.PhoneNumber!,
                        FirstName = userInDb.FirstName,
                        LastName = userInDb.LastName,
                    };

                    return new ResponseDto
                    {
                        IsSuccessful = true,
                        Result = userDto,
                    };
                }
                else
                {
                    return new ResponseDto
                    {
                        IsSuccessful = false,
                        Errors = result.Errors.Select(x => x.Description).ToList(),
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    IsSuccessful = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public async Task<LoginResponseDto> Login(LoginDto dto)
        {
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName!.ToLower() == dto.UserName.ToLower());
            var isValid = await _userManager.CheckPasswordAsync(user!, dto.Password);
            if (user is null || isValid == false)
            {
                return new LoginResponseDto();
            }

            return new LoginResponseDto
            {
                User = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    Phone = user.PhoneNumber!,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                },
                Token = _jwtTokenGenerator.GenerateToken(user)
            };
        }

        public async Task<ResponseDto> AssignToRole(string userName, string role)
        {
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName!.ToLower() == userName.ToLower());
            if (user is null) { return new ResponseDto { IsSuccessful = false, Errors = new List<string> { "No user found!" } }; }
            else
            {
                IdentityResult? result;
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    result = await _roleManager.CreateAsync(new IdentityRole { Name = role, NormalizedName = role.ToUpper() });
                    if (result.Succeeded == false)
                    {
                        return new ResponseDto { IsSuccessful = false, Errors = result.Errors.Select(x=> x.Description).ToList() };
                    }
                }

                result = await _userManager.AddToRoleAsync(user, role);
                if (result.Succeeded == false)
                {
                    return new ResponseDto { IsSuccessful = false, Errors = result.Errors.Select(x => x.Description).ToList() };
                }

                return new ResponseDto
                {
                    IsSuccessful = true,
                };
            }

        }
    }
}
