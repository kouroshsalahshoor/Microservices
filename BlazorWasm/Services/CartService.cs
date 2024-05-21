﻿using Shared;
using Shared.Dtos;
using Shared.Dtos.Cart;
using Shared.Front;

namespace BlazorWasm.Services.IServices
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;

        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> Get(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Get,
                Url = ApplicationConstants.CarthApi + userId
            });
        }
        public async Task<ResponseDto?> AddEdit(CartDto dto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Post,
                Url = ApplicationConstants.CarthApi + "AddEdit",
                Data = dto
            });
        }

        public async Task<ResponseDto?> Remove(int cartDetailId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Post,
                Url = ApplicationConstants.CarthApi + "Remove",
                Data = cartDetailId
            });
        }

        public async Task<ResponseDto?> ApplyCoupon(CartDto dto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Post,
                Url = ApplicationConstants.CarthApi + "ApplyCoupon",
                Data = dto
            });
        }

        public async Task<ResponseDto?> RemoveCoupon(CartDto dto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.Post,
                Url = ApplicationConstants.CarthApi + "RemoveCoupon",
                Data = dto
            });
        }
    }
}
