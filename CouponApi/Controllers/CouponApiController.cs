using CouponApi.Data;
using CouponApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace CouponApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ResponseDto _response;

        public CouponApiController(ApplicationDbContext db)
        {
            _db = db;
            _response = new();
        }

        [HttpGet]
        public async Task<ResponseDto> Get()
        {
            try
            {
                var models = await _db.Coupons.ToListAsync();
                _response.IsSuccessful = true;
                _response.Result = models;
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
                var model = await _db.Coupons.FirstOrDefaultAsync(x=>x.Id == id);
                _response.IsSuccessful = true;
                _response.Result = model;
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
