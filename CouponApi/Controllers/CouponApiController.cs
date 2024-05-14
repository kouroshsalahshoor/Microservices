using AutoMapper;
using CouponApi.Data;
using CouponApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Dtos;

namespace CouponApi.Controllers
{
    [Route("api/couponapi")]
    //[Route("api/[controller]")]
    [ApiController]
    public class CouponApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;

        public CouponApiController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        public async Task<ResponseDto> Get()
        {
            try
            {
                var models = await _db.Coupons.ToListAsync();
                _response.IsSuccessful = true;
                _response.Result = _mapper.Map<List<CouponDto>>(models);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.Errors.Add(ex.Message);
            }

            return _response;
        }

        [HttpGet("{id:int}")]
        public async Task<ResponseDto> Get(int id)
        {
            try
            {
                var model = await _db.Coupons.FirstAsync(x=>x.Id == id);
                _response.IsSuccessful = true;
                _response.Result = _mapper.Map<CouponDto>(model);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.Errors.Add(ex.Message);
            }

            return _response;
        }

        [HttpGet("getbycode/{code}")]
        public async Task<ResponseDto> Get(string code)
        {
            try
            {
                var model = await _db.Coupons.FirstAsync(x => x.Code.ToLower() == code.ToLower());
                _response.IsSuccessful = true;
                _response.Result = _mapper.Map<CouponDto>(model);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.Errors.Add(ex.Message);
            }

            return _response;
        }

        [HttpPost]
        public async Task<ResponseDto> Post([FromBody] CouponDto dto)
        {
            try
            {
                var model = _mapper.Map<Coupon>(dto);
                await _db.Coupons.AddAsync(model);
                await _db.SaveChangesAsync();
                _response.IsSuccessful = true;
                _response.Result = _mapper.Map<CouponDto>(model);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.Errors.Add(ex.Message);
            }

            return _response;
        }

        [HttpPut]
        public async Task<ResponseDto> Put([FromBody] CouponDto dto)
        {
            try
            {
                var model = _mapper.Map<Coupon>(dto);
                _db.Coupons.Update(model);
                await _db.SaveChangesAsync();
                _response.IsSuccessful = true;
                _response.Result = _mapper.Map<CouponDto>(model);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.Errors.Add(ex.Message);
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                var model = _db.Coupons.First(x => x.Id == id);
                _db.Coupons.Remove(model);
                await _db.SaveChangesAsync();
                _response.IsSuccessful = true;
                _response.Result = _mapper.Map<CouponDto>(model);
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
