using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Models;
using Shared;
using Shared.Dtos;
using Shared.Front;

namespace ProductApi.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize]
    public class ProductApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;

        public ProductApiController(ApplicationDbContext db, IMapper mapper)
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
                var models = await _db.Products.ToListAsync();
                _response.IsSuccessful = true;
                _response.Result = _mapper.Map<List<ProductDto>>(models);
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
                var model = await _db.Products.FirstAsync(x => x.Id == id);
                _response.IsSuccessful = true;
                _response.Result = _mapper.Map<ProductDto>(model);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.Errors.Add(ex.Message);
            }

            return _response;
        }

        [HttpGet("getbycategory/{category}")]
        public async Task<ResponseDto> Get(string category)
        {
            try
            {
                var models = await _db.Products.Where(x => x.Category.ToLower() == category.ToLower()).ToListAsync();
                _response.IsSuccessful = true;
                _response.Result = _mapper.Map<List<ProductDto>>(models);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.Errors.Add(ex.Message);
            }

            return _response;
        }

        [HttpPost]
        [Authorize(Roles = ApplicationConstants.Role_Admin)]
        public async Task<ResponseDto> Post([FromBody] ProductDto dto)
        {
            try
            {
                var model = _mapper.Map<Product>(dto);
                await _db.Products.AddAsync(model);
                await _db.SaveChangesAsync();
                _response.IsSuccessful = true;
                _response.Result = _mapper.Map<ProductDto>(model);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.Errors.Add(ex.Message);
            }

            return _response;
        }

        [HttpPut]
        [Authorize(Roles = ApplicationConstants.Role_Admin)]
        public async Task<ResponseDto> Put([FromBody] ProductDto dto)
        {
            try
            {
                var model = _mapper.Map<Product>(dto);
                _db.Products.Update(model);
                await _db.SaveChangesAsync();
                _response.IsSuccessful = true;
                _response.Result = _mapper.Map<ProductDto>(model);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.Errors.Add(ex.Message);
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = ApplicationConstants.Role_Admin)]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                var model = _db.Products.First(x => x.Id == id);
                _db.Products.Remove(model);
                await _db.SaveChangesAsync();
                _response.IsSuccessful = true;
                _response.Result = _mapper.Map<ProductDto>(model);
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
