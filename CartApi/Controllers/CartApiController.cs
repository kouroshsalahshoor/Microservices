using AutoMapper;
using CartApi.Data;
using CartApi.Models;
using CartApi.Services.IServices;
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
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private readonly ResponseDto _response;

        public CartApiController(ApplicationDbContext db, IMapper mapper, IProductService productService, ICouponService couponService)
        {
            _db = db;
            _mapper = mapper;
            _productService = productService;
            _couponService = couponService;
            _response = new();
        }

        [HttpGet("{userId}")]
        public async Task<ResponseDto> Get(string userId)
        {
            try
            {
                var cart = new CartDto();
                cart.CartHeader = _mapper.Map<CartHeaderDto>(await _db.CartHeaders.FirstAsync(x => x.UserId == userId));
                cart.CartDetails = _mapper.Map<List<CartDetailDto>>(await _db.CartDetails.Where(x => x.CartHeaderId == cart.CartHeader.Id).ToListAsync());

                var productDtos = await _productService.Get();

                foreach (var item in cart.CartDetails)
                {
                    item.Product = productDtos.FirstOrDefault(x => x.Id == item.ProductId);
                    cart.CartHeader.Total += item.Count * item.Product.Price;
                }

                //apply coupon
                if (string.IsNullOrEmpty(cart.CartHeader.CouponCode) == false)
                {
                    var coupon = await _couponService.Get(cart.CartHeader.CouponCode);
                    if (coupon is not null && cart.CartHeader.Total > coupon.MinAmount)
                    {
                        cart.CartHeader.Total -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }

                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.Errors.Add(ex.Message);
            }
            return _response;
        }

        [HttpPost("applycoupon")]
        public async Task<ResponseDto> ApplyCoupon([FromBody] CartDto dto)
        {
            try
            {
                var cartHeader = await _db.CartHeaders.FirstAsync(x => x.UserId == dto.CartHeader.UserId);
                cartHeader.CouponCode = dto.CartHeader?.CouponCode;
                _db.CartHeaders.Update(cartHeader);
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.Errors.Add(ex.Message);
            }
            return _response;
        }

        [HttpPost("removecoupon")]
        public async Task<ResponseDto> RemoveCoupon([FromBody] CartDto dto)
        {
            try
            {
                var cartHeader = await _db.CartHeaders.FirstAsync(x => x.UserId == dto.CartHeader.UserId);
                cartHeader.CouponCode = "";
                _db.CartHeaders.Update(cartHeader);
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.Errors.Add(ex.Message);
            }
            return _response;
        }

        [HttpPost("addedit")]
        public async Task<ResponseDto> AddEdit([FromBody] CartDto dto)
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

        [HttpPost("remove")]
        public async Task<ResponseDto> Remove([FromBody] int cartDetailsId)
        {
            try
            {
                var cartDetail = await _db.CartDetails.FirstAsync(x => x.Id == cartDetailsId);
                _db.CartDetails.Remove(cartDetail);

                int count = _db.CartDetails.Where(x => x.CartHeaderId == cartDetail.CartHeaderId).Count();
                if (count == 1)
                {
                    var cartHeader = await _db.CartHeaders.FirstOrDefaultAsync(x => x.Id == cartDetail.CartHeaderId);
                    _db.CartHeaders.Remove(cartHeader);
                }
                await _db.SaveChangesAsync();

                _response.Result = true;
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
