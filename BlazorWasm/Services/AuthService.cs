﻿using BlazorWasm.Services.IServices;
using Microsoft.JSInterop;
using Shared;
using Shared.Dtos.Auth;
using Shared.Front;

namespace BlazorWasm.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> Register(RegisterDto dto)
        {
            if (string.IsNullOrEmpty(dto.Role) || (dto.Role != ApplicationConstants.Role_User && dto.Role != ApplicationConstants.Role_Admin))
            {
                dto.Role = ApplicationConstants.Role_User;
            }

            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Post,
                Url = ApplicationConstants.AuthApi + "register",
                Data = dto
            });
        }

        public async Task<ResponseDto?> Login(LoginDto dto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Post,
                Url = ApplicationConstants.AuthApi + "login",
                Data = dto
            });
        }

        public async Task<ResponseDto?> AssignToRole(RegisterDto dto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Post,
                Url = ApplicationConstants.AuthApi + "assigntorole",
                Data = dto
            });
        }
    }
}
