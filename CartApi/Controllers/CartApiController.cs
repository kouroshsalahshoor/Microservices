using AutoMapper;
using CartApi.Data;
using CartApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Dtos.Cart;

namespace CartApi.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;

        public CartApiController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new();
        }

        [HttpPost("addedit")]
        public async Task<ResponseDto> AddEdit(CartDto dto)
        {
            try
            {
                var cartHeader = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == dto.CartHeader.UserId);
                if (cartHeader == null)
                {
                    //create cartHeader
                    cartHeader = _mapper.Map<CartHeader>(dto.CartHeader);
                    await _db.CartHeaders.AddAsync(cartHeader);
                    await _db.SaveChangesAsync();

                    dto.CartDetails.First().CartHeaderId = cartHeader.Id;
                    await _db.CartDetails.AddAsync(_mapper.Map<CartDetail>(dto.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //detail with same product
                    var cartDetail = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(x =>
                    x.CartHeaderId == cartHeader.Id &&
                    x.ProductId == dto.CartDetails.First().ProductId);
                    if (cartDetail is null)
                    {
                        //create cartDetail
                        dto.CartDetails.First().CartHeaderId = cartHeader.Id;
                        await _db.CartDetails.AddAsync(_mapper.Map<CartDetail>(dto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        //update count
                        dto.CartDetails.First().Count += cartDetail.Count;
                        if (dto.CartDetails.First().Count < 0)
                        {
                            dto.CartDetails.First().Count = 0;
                        }
                        dto.CartDetails.First().CartHeaderId = cartDetail.CartHeaderId;
                        dto.CartDetails.First().Id = cartDetail.Id;

                        _db.CartDetails.Update(_mapper.Map<CartDetail>(dto.CartDetails.First()));
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = dto;
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.Errors.Add(ex.Message);
            }
            return _response;
        }
    }
}
